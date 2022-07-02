using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void HostGame()
    {
        _gameManager.HostGame();
    }

    public void JoinGame()
    {
        _gameManager.JoinGame();
    }
}
