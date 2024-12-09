using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using TMPro;
using Unity.Collections;
using System;
using System.Threading.Tasks;

public class TankSetup : NetworkBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField] 
    private SpriteRenderer minimapIconRenderer;
    [SerializeField] 
    private Color ownerColour;

    [field: SerializeField]
    public Health Health { get; private set; }

    [field: SerializeField] 
    public CoinWallet Wallet { get; private set; }


    private int cameraPriority = 15;

    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();


    public static event Action<TankSetup> OnPlayerSpawned;
    public static event Action<TankSetup> OnPlayerDespawned;
    
    public override void OnNetworkSpawn()
    {

        if (IsServer)
        {
            UserData userData = null;

            userData = HostSingleton.Instance.hostGameManger.networkServer.GetUserDataByClientId(OwnerClientId);

            PlayerName.Value = userData.userName;

            StartCoroutine(SetupPlayerCoroutine());
          
        }

        if (IsOwner)
        {
            virtualCamera.Priority = cameraPriority;
            minimapIconRenderer.color = ownerColour;
        }
    }


    IEnumerator SetupPlayerCoroutine()
    {
        yield return new WaitForSeconds(2f);

        if(IsSpawned)
        OnPlayerSpawned.Invoke(this);
    }
    public async Task SetupPlayer()
    {
        await Task.Delay(1000);

        OnPlayerSpawned.Invoke(this);
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            OnPlayerDespawned.Invoke(this);
        }
    }


    IEnumerator getName()
    {
        yield return new WaitForSeconds(5);

        if (IsServer)
        {


          
        }
    }
}
