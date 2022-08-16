using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    [SerializeField] private UnityEvent PlayerEntered;
    
    private Rigidbody2D _rigidbody;
    
    private void Start()
    {
        _rigidbody = gameObject.AddComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            Debug.Log("COIN EARNED!!!");
            player.AddCoin(1);
            PlayerEntered?.Invoke();
            Destroy(gameObject);
        }
    }
}
