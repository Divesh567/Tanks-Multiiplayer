using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileSpawner : NetworkBehaviour
{
    [Header("References")]
    [SerializeField]
    private InputReader inputReader;
    [SerializeField]
    private Transform projectleSpawnPoint;
    [SerializeField]
    private GameObject clientProjectlePrefab;
    [SerializeField]
    private GameObject serverProjectlePrefab;
    [SerializeField]
    private GameObject muzzleFlash;
    [SerializeField]
    private CoinWallet wallet;
    [SerializeField]
    private Collider2D tankCollider;

    [Header("Settings")]
    [SerializeField]
    private float projectleSpeed;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float muzzleFlashDuration;
    private float muzzleFlashCurrentTime;
    private Vector2 firePos;
    private bool doFire;
    [SerializeField]
    private int costToFire;
    private float timer;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        inputReader.OnFirePressed += HandleFire;

    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        inputReader.OnFirePressed -= HandleFire;
    }
    private void Update()
    {
        if (!IsOwner) return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }


        if (muzzleFlashCurrentTime > 0)
        {
            muzzleFlashCurrentTime -= Time.deltaTime;

            if (muzzleFlashCurrentTime < 0)
            {
                muzzleFlash.SetActive(false);
            }
        }

        if (!IsOwner) return;

        if (!doFire) return;

        if (timer > 0) { return; }

        if (wallet.TotalCoins.Value < costToFire) { return; }



        SpawnProjectileServerRpc(projectleSpawnPoint.position, projectleSpawnPoint.transform.up);

        SpawnDummyProjectile(projectleSpawnPoint.position, projectleSpawnPoint.transform.up);

        timer = 1 / fireRate;

        doFire = false;
    }

    [ServerRpc]
    private void SpawnProjectileServerRpc(Vector2 spawnPoint, Vector3 spawnDirection)
    {
        if (wallet.TotalCoins.Value < costToFire) { return; }

        wallet.SpendCoins(costToFire);


        var newProjectile = Instantiate(serverProjectlePrefab, spawnPoint, Quaternion.identity);

        newProjectile.transform.up = spawnDirection;

        Physics2D.IgnoreCollision(tankCollider, newProjectile.GetComponent<Collider2D>());

        if(newProjectile.TryGetComponent(out DealDamage dealDamage))
        {
            dealDamage.ownerId = OwnerClientId;
        }

        if (newProjectile.TryGetComponent(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectleSpeed;
        }

        SpawnProjectileClientRpc(spawnPoint, spawnDirection);
    }

    [ClientRpc]
    private void SpawnProjectileClientRpc(Vector2 spawnPoint, Vector3 spawnDirection)
    {
        if (IsOwner) return;

        SpawnDummyProjectile(spawnPoint, spawnDirection);
    }

    private void SpawnDummyProjectile(Vector2 spawnPoint, Vector3 spawnDirection)
    {
        muzzleFlash.SetActive(true);
        muzzleFlashCurrentTime = muzzleFlashDuration;

        var newProjectile = Instantiate(clientProjectlePrefab, spawnPoint, Quaternion.identity);

        newProjectile.transform.up = spawnDirection;

        Physics2D.IgnoreCollision(tankCollider, newProjectile.GetComponent<Collider2D>());

      

        if(newProjectile.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectleSpeed;
        }
    }

    private void HandleFire(bool isFired , Vector2 firePos)
    {
        

        doFire = isFired;
        this.firePos =  firePos;
    }
}
