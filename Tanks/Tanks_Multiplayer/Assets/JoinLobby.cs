using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class JoinLobby : MonoBehaviour
{
    public Button joinButton;

    // Start is called before the first frame update
    void Start()
    {
        joinButton.onClick.AddListener(OnJoinLobbyButtonPressed);
    }

    private void OnJoinLobbyButtonPressed()
    {
        NetworkManager.Singleton.StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
