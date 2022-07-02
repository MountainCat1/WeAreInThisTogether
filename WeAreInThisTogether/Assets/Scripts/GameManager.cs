using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private NetworkManager _networkManager;
    
    private void Awake()
    {
        // Singleton
        if(Instance != null)
            Debug.LogError($"Singleton duplicated! {typeof(GameManager)}");
        Instance = this;
        
        // Assign event handlers of _networkManager
        _networkManager.OnClientConnectedCallback += NetworkManagerOnOnClientConnectedCallback;
        _networkManager.OnClientDisconnectCallback += NetworkManagerOnOnClientDisconnectCallback;
    }

    private void NetworkManagerOnOnClientDisconnectCallback(ulong clientId)
    {
        Debug.Log($"Client ({clientId}) disconnected!");
    }

    private void NetworkManagerOnOnClientConnectedCallback(ulong clientId)
    {
        Debug.Log($"Client ({clientId}) connected!");
    }

    public void HostGame()
    {
        if (_networkManager.IsClient || _networkManager.IsHost || _networkManager.IsClient)
        {
            Debug.LogError("Can't host. Network manager is already connected client or server!");
            return;
        }
        
        Debug.Log("Starting a game as host...");
        _networkManager.StartHost();
        
    }

    public void JoinGame()
    {
        if (_networkManager.IsClient || _networkManager.IsHost || _networkManager.IsClient)
        {
            Debug.LogError("Can't join game. Network manager is already connected client or server!");
            return;
        }
        
        Debug.Log("Joining a game as client...");
        _networkManager.StartClient();
    }

    public void LeaveGame()
    {
        if (!_networkManager.IsClient)
        {
            Debug.LogError("Cannot leave game. You are not a connected client!");
            return;
        }
        
        Debug.Log("Shutting down network manager...");
        _networkManager.Shutdown();
    }
}
