using System;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private PhysicsMovement _movement;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.Play("Idle");
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //Vector3 movement = transform.right * horizontal + transform.forward * vertical;
        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
        //transform.forward = movement;
        //_characterController.Move(movement * (Time.deltaTime * 7f));
        //_movement.Move(new Vector3(horizontal, 0, vertical));
        _movement.Move(movementDirection);
    }
}
