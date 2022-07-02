using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager Instance { get; private set; }
    
    [SerializeField] private NetworkManager _networkManager;

    private void Awake()
    {
        if(Instance != null)
            Debug.LogError($"Singleton duplicated! {typeof(GameManager)}");
        
        Instance = this;
    }

    public void HostGame()
    {
        Debug.Log("Starting a game host");
        _networkManager.StartHost();
        
    }
}
