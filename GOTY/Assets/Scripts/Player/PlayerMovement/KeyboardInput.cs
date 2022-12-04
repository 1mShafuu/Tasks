using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movement;

    // private void FixedUpdate()
    // {
    //     float horizontal = Input.GetAxis(axisName: "Horizontal");
    //     var keyDownRight = Input.GetKeyDown(KeyCode.D);
    //     var keyDownLeft = Input.GetKeyDown(KeyCode.A);
    //    // _movement.Move(new Vector3(1, 0, -horizontal));
    //     _movement.Move(new Vector3(0, 0, -horizontal), keyDownLeft, keyDownRight);
    // }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _movement.Jump();
        }
        
        var keyDownRight = Input.GetKeyDown(KeyCode.D);
        var keyDownLeft = Input.GetKeyDown(KeyCode.A);
        // _movement.Move(new Vector3(1, 0, -horizontal));
        _movement.Move(keyDownLeft, keyDownRight);
    }
}
