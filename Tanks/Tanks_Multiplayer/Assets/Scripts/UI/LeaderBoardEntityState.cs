using System;
using Unity.Collections;
using Unity.Netcode;

public struct LeaderBoardEntityState : INetworkSerializable, IEquatable<LeaderBoardEntityState>
{
    public ulong clientId;
    public FixedString32Bytes playerName;
    public int coins;

    public bool Equals(LeaderBoardEntityState other)
    {
        return clientId == other.clientId && 
                playerName.Equals(other.playerName) &&
                coins == other.coins;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref coins);
    }
}