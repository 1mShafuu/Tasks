using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SurfaceSlider), typeof(SpeedMagnifier))]
public class PlayerMovement : MonoBehaviour
{
    private const float JumpForceDown = -15f;
    private const float LineDistance = 4f;
    private const float MaxDistanceToGroundCheck = 2 * 0.5f + 0.3f;
    
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SurfaceSlider _surfaceSlider;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForceUp;
    [SerializeField] private LayerMask _whatIsGround;
    
    private bool _isGrounded;
    private bool _isReadyToJump;
    private RaycastHit _hit;
    private SpeedMagnifier _speedMagnifier;
   // private MovementBorders _borders;
   // private float _leftBorder;
   // private float _rightBorder;
    private int _moveLine = 1;
    
    public float MovementSpeed => _movementSpeed;

    private void Awake()
    {
   //     _borders = GetComponent<MovementBorders>();
        _isReadyToJump = true;
        _rigidbody = GetComponent<Rigidbody>();
        _speedMagnifier = GetComponent<SpeedMagnifier>();
    }

    private void OnEnable()
    {
        _speedMagnifier.SpeedChanged += OnSpeedChanged;
    }

    private void OnDisable()
    {
        _speedMagnifier.SpeedChanged -= OnSpeedChanged;
    }
    
    private void Start()
    {
        transform.position += Vector3.up;
    }

    private void Update()
    {
        //Debug.Log($"{_leftBorder} {_rightBorder}");
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, MaxDistanceToGroundCheck, _whatIsGround);
    }
    
    public void Move(bool keyDownLeft, bool keyDownRight)
    {
        const int leftMostLane = 0;
        const int rightMostLane = 2;
        //var distanceToEdge = _rigidbody.position.z;
        Vector3 directionAlongSurface = _surfaceSlider.Project(Vector3.right.normalized);
        Vector3 offset = directionAlongSurface * (_movementSpeed * Time.deltaTime);

        if (keyDownRight)
        {
            if (_moveLine < rightMostLane)
            {
                _moveLine++;
                _rigidbody.position += Vector3.back * LineDistance;
            }
        }

        if (keyDownLeft)
        {
            if (_moveLine > leftMostLane)
            { 
                _moveLine--; 
                _rigidbody.position += Vector3.forward * LineDistance;
            }
        }
        
        var newVectorPosition = _rigidbody.position + offset;
        _rigidbody.MovePosition(newVectorPosition);
    }

    public void JumpUp()
    {
        if(_isReadyToJump && _isGrounded)
        {
            _isReadyToJump = false;
            _rigidbody.AddForce(transform.up * _jumpForceUp, ForceMode.Impulse);
            ResetJump();
        }
    }

    public void JumpDown()
    {
        if (_isGrounded == false)
        {
            _rigidbody.AddForce(transform.up * JumpForceDown, ForceMode.VelocityChange);
        }
    }

    public void ResetMovement()
    {
        _moveLine = 1;
        _speedMagnifier.ResetSpeed();
        _movementSpeed = _speedMagnifier.StartSpeed;
    }
    
    private void ResetJump()
    {
        _isReadyToJump = true;
    }

    private void OnSpeedChanged(float speed)
    {
       // Debug.Log($"REACt");
        _movementSpeed = speed;
    }
}
