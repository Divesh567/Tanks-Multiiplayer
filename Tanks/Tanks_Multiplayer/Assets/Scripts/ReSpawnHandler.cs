using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ReSpawnHandler : NetworkBehaviour
{
    [SerializeField] private NetworkObject playerPrefab;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }

        TankSetup[] players = FindObjectsByType<TankSetup>(sortMode: FindObjectsSortMode.None);
        foreach (TankSetup player in players)
        {
            HandlePlayerSpawned(player);
        }

        TankSetup.OnPlayerSpawned += HandlePlayerSpawned;
        TankSetup.OnPlayerDespawned += HandlePlayerDespawned;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) { return; }
        
        TankSetup.OnPlayerSpawned -= HandlePlayerSpawned;
        TankSetup.OnPlayerDespawned -= HandlePlayerDespawned;
    }

    private void HandlePlayerSpawned(TankSetup player)
    {
        player.Health.OnDie -= HandlePlayerDie;
        player.Health.OnDie +=  HandlePlayerDie;
    }

    private void HandlePlayerDespawned(TankSetup player)
    {
        player.Health.OnDie -= HandlePlayerDie;
    }

    private void HandlePlayerDie(TankSetup player)
    {
        Debug.Log($"Player Died: {player.PlayerName.Value}");
     
        StartCoroutine(RespawnPlayer(player , player.OwnerClientId));

    }

    private IEnumerator RespawnPlayer(TankSetup player ,ulong ownerClientId)
    {

        yield return new WaitForSeconds(1f);

        player.gameObject.SetActive(false);
        Destroy(player.gameObject);

        yield return new WaitForSeconds(1f);

        yield return null;

        NetworkObject playerInstance = Instantiate(
            playerPrefab, SpawnPoint.GetRandomSpawnPos(), Quaternion.identity);

        playerInstance.SpawnAsPlayerObject(ownerClientId);
    }
}
