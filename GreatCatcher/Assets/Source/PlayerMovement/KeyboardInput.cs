using System;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private PhysicsMovement _movement;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //Vector3 movement = transform.right * horizontal + transform.forward * vertical;
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        //transform.forward = movement;
        //_characterController.Move(movement * (Time.deltaTime * 7f));
        //_movement.Move(new Vector3(horizontal, 0, vertical));
        _movement.Move(movement);
    }
}
