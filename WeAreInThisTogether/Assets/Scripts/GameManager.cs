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
        if(Instance != null)
            Debug.LogError($"Singleton duplicated! {typeof(GameManager)}");
        
        Instance = this;
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
}
