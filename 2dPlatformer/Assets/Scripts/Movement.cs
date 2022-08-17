using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer _renderer;
    
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            _renderer.flipX = true;
            transform.Translate(_speed * Time.deltaTime, 0, 0);
            _animator.SetFloat(Animator.StringToHash("Speed"), _speed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _renderer.flipX = false;
            _animator.SetFloat(Animator.StringToHash("Speed"), _speed);
            transform.Translate(_speed * Time.deltaTime * -1, 0, 0);
            
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            float jumpForce = 5f;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
            _rigidbody2D.freezeRotation = true;
        }
        else
        {
            _animator.SetFloat(Animator.StringToHash("Speed"), 0);
        }
    }
}
