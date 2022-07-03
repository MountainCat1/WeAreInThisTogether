using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusDisplay;
    [SerializeField] private TMP_InputField playerNameInputField;
    
    private GameManager _gameManager;
    private NetworkManager _networkManager;
    
    private void Start()
    {
        _gameManager = GameManager.Singleton;
        _networkManager = NetworkManager.Singleton;
    }

    private void Update()
    {
        UpdateStatusDisplay();
    }
    
    public void HostGame()
    {
        _gameManager.HostGame();
    }

    public void JoinGame()
    {
        _gameManager.JoinGame();
    }
    
    public void LeaveGame()
    {
        _gameManager.LeaveGame();
    }


    private void UpdateStatusDisplay()
    {
        if (_networkManager.IsHost)
        {
            statusDisplay.text = "You are a host";
            return;
        }  
        if (_networkManager.IsServer)
        {
            statusDisplay.text = "You are a server";
            return;
        }
        if (_networkManager.IsClient)
        {
            statusDisplay.text = "You are a client";
            return;
        }
        statusDisplay.text = "Disconnected";
    }

    public PlayerData CreatePlayerData()
    {
        return new PlayerData()
        {
            Username = playerNameInputField.text 
        };
    }
}
