using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : NetworkBehaviour
{
    [SerializeField]
    private Health health;

    [SerializeField]
    private Image healthBar;

    public override void OnNetworkSpawn()
    {
        if (!IsClient) return;

        health.currentHealth.OnValueChanged += HandleHealthChange;
    }

    public override void OnNetworkDespawn()
    {
        {
            if (!IsClient) return;

            health.currentHealth.OnValueChanged -= HandleHealthChange;
        }
    }

    private void HandleHealthChange(int previousValue, int newValue)
    {
        healthBar.fillAmount = (float)newValue / health.maxHealth;
    }
}
