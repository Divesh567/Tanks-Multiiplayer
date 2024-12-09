using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;
using Unity.Netcode;

public class ClientNetworkTransform : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner;
    }

    protected override void Update()
    {
        CanCommitToTransform = IsOwner;
        base.Update();
        if (!IsHost && NetworkManager != null && NetworkManager.IsConnectedClient && CanCommitToTransform)
        {
            TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
        }
    }
}
