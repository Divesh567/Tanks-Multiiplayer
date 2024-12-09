using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameHud : MonoBehaviour
{
    public void LeaveGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            HostSingleton.Instance.hostGameManger.ShutDown();
        }

        ClientSingleton.Instance.clientGameManager.Disconnect();
    }

}
