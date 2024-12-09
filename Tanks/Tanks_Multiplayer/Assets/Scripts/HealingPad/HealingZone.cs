using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class HealingZone : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Image healPowerBar;

    [Header("Settings")]
    [SerializeField] private int maxHealPower = 30;
    [SerializeField] private float healCooldown = 60f;
    [SerializeField] private float healTickRate = 1f;
    [SerializeField] private int coinsPerTick = 10;
    [SerializeField] private int healthPerTick = 10;


    private List<TankSetup> playersInZone = new List<TankSetup>();

    private float remainingCooldown;
    private float tickTimer;

    private NetworkVariable<int> HealPower = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            HealPower.OnValueChanged += HandleHealPowerChanged;
            HandleHealPowerChanged(0, HealPower.Value);
        }

        if (IsServer)
        {
            HealPower.Value = maxHealPower;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            HealPower.OnValueChanged -= HandleHealPowerChanged;
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!IsServer) { return; }

        if (!col.attachedRigidbody.TryGetComponent<TankSetup>(out TankSetup player)) { return; }

        playersInZone.Add(player);

        Debug.Log($"Entered: {player.PlayerName.Value}");
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!IsServer) { return; }

        if (!col.attachedRigidbody.TryGetComponent<TankSetup>(out TankSetup player)) { return; }

        playersInZone.Remove(player);

        Debug.Log($"Left: {player.PlayerName.Value}");
    }

    private void Update()
    {
        if (!IsServer) { return; }

        if (remainingCooldown > 0f)
        {
            remainingCooldown -= Time.deltaTime;

            if (remainingCooldown <= 0f)
            {
                HealPower.Value = maxHealPower;
            }
            else
            {
                return;
            }
        }

        tickTimer += Time.deltaTime;
        if (tickTimer >= 1 / healTickRate)
        {
            foreach (TankSetup player in playersInZone)
            {
                if (HealPower.Value == 0) { break; }

                if (player.Health.currentHealth.Value == player.Health.maxHealth) { continue; }

                if (player.Wallet.TotalCoins.Value < coinsPerTick) { continue; }

                Debug.Log($"Left: {player.PlayerName.Value}");
                player.Wallet.SpendCoins(coinsPerTick);
                player.Health.RestoreHealth(healthPerTick);

                HealPower.Value -= 1;

                if (HealPower.Value == 0)
                {
                    remainingCooldown = healCooldown;
                }
            }

            tickTimer = tickTimer % (1 / healTickRate);
        }

    }

    private void HandleHealPowerChanged(int oldHealPower, int newHealPower)
    {
        healPowerBar.fillAmount = (float)newHealPower / maxHealPower;
    }


}