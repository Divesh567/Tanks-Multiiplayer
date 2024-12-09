using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostLobby : MonoBehaviour
{
    public Button hostButton;

    // Start is called before the first frame update
    void Start()
    {
        hostButton.onClick.AddListener(OnHostLobbyPressed);
    }

    private void OnHostLobbyPressed()
    {
        NetworkManager.Singleton.StartHost();
    }


}
