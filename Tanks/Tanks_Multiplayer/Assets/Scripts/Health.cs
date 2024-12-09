using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [field: SerializeField]
    public int maxHealth { get; private set; }

    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();

    private bool isDead;

    public Action<TankSetup> OnDie;
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        currentHealth.Value = maxHealth;

        currentHealth.OnValueChanged +=  CheckDeath;

    }

    public override void OnNetworkDespawn()
    {
        currentHealth.OnValueChanged -= CheckDeath;
    }


    public void ReduceHealth(int damage)
    {
        if (isDead) return;

        ModifyHealth(-damage);
    }

    public void RestoreHealth(int healValue)
    {
        ModifyHealth(healValue);
    }


    private void ModifyHealth(int value)
    {
        if (isDead) return;

        currentHealth.Value = Mathf.Clamp(currentHealth.Value + value, 0, maxHealth);
    }

    private void CheckDeath(int previousValue, int newValue)
    {
        if (isDead) return;

        if (currentHealth.Value == 0)
        {
            isDead = true;
            OnDie?.Invoke(GetComponent<TankSetup>());

        }
    }
}
