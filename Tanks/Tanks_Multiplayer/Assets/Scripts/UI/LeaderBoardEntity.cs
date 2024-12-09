using Unity.Collections;
using UnityEngine;
using TMPro;
using System;
using Unity.Netcode;

public class LeaderBoardEntity : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;
    [SerializeField] 
    private Color myColour = Color.red;

    public ulong clientId { get; private set; }
    public FixedString32Bytes playerName { get; private set; }

    public int coins = 0;

    public void Initialize(ulong ClientID, FixedString32Bytes playerName, int Coins)
    {
        clientId = ClientID;
        this.playerName = playerName;

        UpdateCoins(Coins);
    }

    public void UpdateCoins(int coins)
    {
      

        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            textMesh.color = myColour;
        }


        this.coins = coins;

        UpdateText();

    }

    public void UpdateText()
    {
        textMesh.text = $"{transform.GetSiblingIndex() + 1}. {playerName} ({coins})";
    }
}
