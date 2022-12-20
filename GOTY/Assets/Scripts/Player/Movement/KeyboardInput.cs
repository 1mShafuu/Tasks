using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movement;

    private void Update()
    {
        var pressedKeyDown = Input.GetKeyDown(KeyCode.S);
        var pressedKeyRight = Input.GetKeyDown(KeyCode.D);
        var pressedKeyLeft = Input.GetKeyDown(KeyCode.A);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _movement.JumpUp();
        }

        if (pressedKeyDown)
        {
            _movement.JumpDown();
        }
        
        _movement.Move(pressedKeyLeft, pressedKeyRight);
    }
}
