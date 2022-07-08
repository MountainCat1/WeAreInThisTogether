using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class Missile : NetworkBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private NetworkRigidbody2D _networkRigidbody2D;
    private Collider2D _collider2D;

    public float speed = 1f;
    
    private readonly NetworkVariable<Vector2> _direction = new NetworkVariable<Vector2>();
    public Vector2 Direction { 
        get => _direction.Value;
        set => _direction.Value = value;
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _networkRigidbody2D = GetComponent<NetworkRigidbody2D>();
    }

    private void FixedUpdate()
    {
        var move = Direction * (speed * Time.deltaTime);
        _rigidbody2D.MovePosition((Vector2)transform.position + move);
    }
}
