using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : NetworkBehaviour
{
    private static HashSet<Player> _players = new HashSet<Player>();

    public static Player LocalPlayerSingleton { get; private set; }
    public PlayerData PlayerData { get; set; }


    public static event Action OnPlayerDataSyncCallback;
    
    private void OnEnable()
    {
        _players.Add(this);
    }

    private void OnDisable()
    {
        _players.Remove(this);
    }

    private void Awake()
    {
        if (IsOwner)
        {
            if(LocalPlayerSingleton != null)
                Debug.LogError($"Singleton duplicated! {this.GetType()}");
            LocalPlayerSingleton = this;
        }
    }

    private void Start()
    {
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();


        if (IsOwner)
        {
            // Todo: now its just for debug, should be done differently in the end         
            GetPlayerDataFromDebugMenu();
            
            SendPlayerData();
        }
    }

    private PlayerData GetPlayerDataFromDebugMenu()
    {
        var debugMenu = FindObjectOfType<DebugMenu>();
        var playerData = debugMenu.CreatePlayerData();
        PlayerData = playerData;
        return playerData;
    }


    public static Player GetPlayer(ulong clientId)
        => _players.First(player => player.PlayerData.ClientId == clientId);

    #region Rpc
    public void SendPlayerData()
        => SendPlayerDataServerRpc(PlayerData);
    [ServerRpc]
    public void SendPlayerDataServerRpc(PlayerData playerData)
    {
        playerData.ClientId = OwnerClientId;
        PlayerData = playerData;
        SyncPlayerDataClientRpc(_players.Select(x => x.PlayerData).ToArray());
    }

    [ClientRpc]
    public void SyncPlayerDataClientRpc(PlayerData[] playerData)
    {
        foreach (var player in _players)
        {
            player.PlayerData = playerData.First(x => x.ClientId == player.OwnerClientId);
        }

        OnPlayerDataSyncCallback?.Invoke();
    }
    #endregion

}