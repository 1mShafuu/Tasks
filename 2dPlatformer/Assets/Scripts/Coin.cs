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
        var coinAmount = 1;
        
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.AddCoin(coinAmount);
            PlayerEntered?.Invoke();
            Destroy(gameObject);
        }
    }
}
