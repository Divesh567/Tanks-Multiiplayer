using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class LeaderBoard : NetworkBehaviour
{
    [SerializeField]
    private Transform leaderboardEntityHolder;

    [SerializeField]
    private LeaderBoardEntity entityPrefab;

    NetworkList<LeaderBoardEntityState> leaderboardEntities = new NetworkList<LeaderBoardEntityState>();
    private List<LeaderBoardEntity> entityDisplays = new List<LeaderBoardEntity>();
    [SerializeField] private int entitiesToDisplay = 8;

    private void Awake()
    {
        leaderboardEntities = new NetworkList<LeaderBoardEntityState>();
    }
    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            leaderboardEntities.OnListChanged += HandleLeaderboardEntitiesChanged;

            foreach (LeaderBoardEntityState entity in leaderboardEntities)
            {
                HandleLeaderboardEntitiesChanged(new NetworkListEvent<LeaderBoardEntityState>
                {
                    Type = NetworkListEvent<LeaderBoardEntityState>.EventType.Add,
                    Value = entity
                });
            }
        }

        if (IsServer)
        {
            TankSetup[] players = FindObjectsByType<TankSetup>(FindObjectsSortMode.None);
            foreach (TankSetup player in players)
            {
                HandlePlayerSpawned(player);
            }

            TankSetup.OnPlayerSpawned += HandlePlayerSpawned;
            TankSetup.OnPlayerDespawned += HandlePlayerDespawned;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            leaderboardEntities.OnListChanged -= HandleLeaderboardEntitiesChanged;
        }

        if (IsServer)
        {
            TankSetup.OnPlayerSpawned -= HandlePlayerSpawned;
            TankSetup.OnPlayerDespawned -= HandlePlayerDespawned;
        }
    }

    private void HandleLeaderboardEntitiesChanged(NetworkListEvent<LeaderBoardEntityState> changeEvent)
    {
        switch (changeEvent.Type)
        {
            case NetworkListEvent<LeaderBoardEntityState>.EventType.Add:
                if (!entityDisplays.Any(x => x.clientId == changeEvent.Value.clientId))
                {
                    LeaderBoardEntity leaderboardEntity =
                        Instantiate(entityPrefab, leaderboardEntityHolder);
                    leaderboardEntity.Initialize(
                        changeEvent.Value.clientId,
                        changeEvent.Value.playerName,
                        changeEvent.Value.coins);
                    entityDisplays.Add(leaderboardEntity);
                }

                break;
            case NetworkListEvent<LeaderBoardEntityState>.EventType.Remove:
                LeaderBoardEntity displayToRemove =
                   entityDisplays.FirstOrDefault(x => x.clientId == changeEvent.Value.clientId);
                if (displayToRemove != null)
                {
                    displayToRemove.transform.SetParent(null);
                    Destroy(displayToRemove.gameObject);
                    entityDisplays.Remove(displayToRemove);
                }
                break;
            case NetworkListEvent<LeaderBoardEntityState>.EventType.Value:
                LeaderBoardEntity entityUpdated =
                 entityDisplays.FirstOrDefault(x => x.clientId == changeEvent.Value.clientId);

                if (entityUpdated != null) 
                    entityUpdated.UpdateCoins(changeEvent.Value.coins);
                break;
        }

        entityDisplays.Sort((x, y) => y.coins.CompareTo(x.coins));

        for (int i = 0; i < entityDisplays.Count; i++)
        {
            entityDisplays[i].transform.SetSiblingIndex(i);
            entityDisplays[i].UpdateText();
            entityDisplays[i].gameObject.SetActive(i <= entitiesToDisplay - 1);
        }

        LeaderBoardEntity myDisplay =
            entityDisplays.FirstOrDefault(x => x.clientId == NetworkManager.Singleton.LocalClientId);

        if (myDisplay != null)
        {
            if (myDisplay.transform.GetSiblingIndex() >= entitiesToDisplay)
            {
                leaderboardEntityHolder.GetChild(entitiesToDisplay - 1).gameObject.SetActive(false);
                myDisplay.gameObject.SetActive(true);
            }
        }

    }

    private void HandlePlayerSpawned(TankSetup player)
    {
        leaderboardEntities.Add(new LeaderBoardEntityState
        {
            clientId = player.OwnerClientId,
            playerName = player.PlayerName.Value,
            coins = 0
        });

        player.Wallet.TotalCoins.OnValueChanged += (oldCoins, newCoins) =>
            HandleCoinsChanged(player.OwnerClientId, newCoins);

    }

    private void HandlePlayerDespawned(TankSetup player)
    {
        foreach (LeaderBoardEntityState entity in leaderboardEntities)
        {
            if (entity.clientId != player.OwnerClientId) { continue; }

            leaderboardEntities.Remove(entity);
            break;
        }

        player.Wallet.TotalCoins.OnValueChanged -= (oldCoins, newCoins) =>
            HandleCoinsChanged(player.OwnerClientId, newCoins);

    }

    private void HandleCoinsChanged(ulong clientId, int newCoins)
    {
        for (int i = 0; i < leaderboardEntities.Count; i++)
        {
            if (leaderboardEntities[i].clientId != clientId) { continue; }

            leaderboardEntities[i] = new LeaderBoardEntityState
            {
                clientId = leaderboardEntities[i].clientId,
                playerName = leaderboardEntities[i].playerName,
                coins = newCoins
            };

            return;
        }
    }

}
