using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> OnCollected;

    private Vector3 previousPosition;

    private void Update()
    {
        if (previousPosition != transform.position)
        {
            ShowCoin(true);
        }

        previousPosition = transform.position;
    }

    public override int Collect()
    {
        if (!IsServer)
        {
            ShowCoin(false);
            return 0;
        }

        if (isCollected) { return 0; }

        isCollected = true;

        OnCollected?.Invoke(this);

        return coinValue;
    }

    public void Reset()
    {
        isCollected = false;
    }
}
