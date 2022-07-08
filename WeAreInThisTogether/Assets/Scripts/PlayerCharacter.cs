using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(NetworkTransform))]
public class PlayerCharacter : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI nameDisplay;

    [SerializeField] private Missile missilePrefab;
    [SerializeField] private float movementSpeed = 1f;

    private NetworkTransform _networkTransform;
    private Player _player;

    private void Awake()
    {
        Player.OnPlayerDataSyncCallback += PlayerOnOnPlayerDataSyncCallback;
        
    }

    private void Start()
    {
        PlayerOnOnPlayerDataSyncCallback();
        _networkTransform = GetComponent<NetworkTransform>();
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
        
        HandleMovement();
        
        if(Input.GetMouseButtonDown(0))
            HandleShooting();
    }

    private void HandleShooting()
    {
        var mousePosition = GameManager.Singleton.Camera.ScreenToWorldPoint(Input.mousePosition);
        var direction = (mousePosition - transform.position).normalized;

        var missile = SpawnMissile(direction);

        if (IsServer)
        {
            missile.NetworkObject.Spawn();
        }
        else
        {
            SpawnMissileServerRpc(direction);
        }
    }

    [ServerRpc]
    void SpawnMissileServerRpc(Vector2 direction)
    {
        SpawnMissile(direction);
        //SpawnMissileClientRpc(direction);
    }

    /*[ClientRpc]
    private void SpawnMissileClientRpc(Vector2 direction)
    {
        SpawnMissile(direction);
    }*/

    Missile SpawnMissile(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation  = Quaternion.AngleAxis(angle, Vector3.forward);
        
        var go = Instantiate(missilePrefab.gameObject, transform.position, rotation, null);
        var missile = go.GetComponent<Missile>();

        missile.Direction = direction.normalized;
        
        return missile;
    }

    private void HandleMovement()
    {
        var movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        var step = Time.deltaTime * movementSpeed;

        transform.position = (Vector2)transform.position + (movement * step);
    }
}
