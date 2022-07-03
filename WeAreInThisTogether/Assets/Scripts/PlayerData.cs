using System;
using Unity.Netcode;

[Serializable]
public class PlayerData : INetworkSerializable
{
    private string _username;
    private ulong _clientId;

    public string Username
    {
        get => _username;
        set => _username = value;
    }

    public ulong ClientId
    {
        get => _clientId;
        set => _clientId = value;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref _username);
        serializer.SerializeValue(ref _clientId);
    }
}