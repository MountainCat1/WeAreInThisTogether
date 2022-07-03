using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(NetworkTransform))]
public class PlayerCharacter : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI nameDisplay; 
        
    [SerializeField] private float speed = 1f;

    private NetworkTransform _networkTransform;
    private Player _player;
    
    private void Awake()
    {
        _networkTransform = GetComponent<NetworkTransform>();
        
        Player.OnPlayerDataSyncCallback += PlayerOnOnPlayerDataSyncCallback;
        PlayerOnOnPlayerDataSyncCallback();
    }

    private void PlayerOnOnPlayerDataSyncCallback()
    {
        if (_player == null)
            _player = Player.GetPlayer(OwnerClientId);

        nameDisplay.text = _player.PlayerData.Username;
    }

    public override void OnNetworkSpawn()
    {
        /*if(!NetworkObject.IsOwner)
            Destroy(this);*/
    }

    private void Update()
    {
        if(!IsOwner)
            return;
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Sending ping to the server...");
            PingServerRpc(Time.frameCount, "hello, world"); // Client -> Server
        }
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Sending ping to clients...");
            PingClientRpc(Time.frameCount, "hello, world i am a server!"); // Server -> Client
        }
        
        HandleMovement();
    }

    private void HandleMovement()
    {
        var movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        var step = Time.deltaTime * speed;

        transform.position = (Vector2)transform.position + (movement * step);
    }

    [ServerRpc(Delivery = RpcDelivery.Reliable)]
    void PingServerRpc(int somenumber, string sometext)
    {
        Debug.Log($"Super message from ({OwnerClientId}): {sometext}");
    }
    
    [ClientRpc]
    void PingClientRpc(int somenumber, string sometext)
    {
        Debug.Log($"Super message from ({OwnerClientId}): {sometext}");
    }
}
