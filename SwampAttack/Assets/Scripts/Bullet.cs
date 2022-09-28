using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _power;

    private static Vector2 _shootingVector;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
     {
         _rigidbody2D.AddForce(Vector2.left * _power + _shootingVector);
     }
    
    /*
    private void Update()
    {
        transform.Translate( Time.deltaTime * _speed * _shootingVector);
    }
    */
 
     public static void SetBulletVector(Vector2 movementVector)
     {
         _shootingVector = new Vector2(movementVector.x, movementVector.y);
     }

     private void OnTriggerEnter2D(Collider2D collision)
     {
         if (collision.gameObject.TryGetComponent(out Enemy enemy))
         {
             enemy.TakeDamage(_damage);
         }
         
         Destroy(gameObject);
     }
 }
