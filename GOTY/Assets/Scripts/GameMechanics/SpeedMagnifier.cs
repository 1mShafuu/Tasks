using UnityEngine;
using UnityEngine.Events;

public class SpeedMagnifier : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movement;
    
    private const float SpeedIncreaseMeasure = 0.5f;
    
    private float _speed;
    
    public float StartSpeed { get; private set; }

    public event UnityAction<float> SpeedChanged;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        _speed = _movement.MovementSpeed;
        StartSpeed = _movement.MovementSpeed;
    }

    public void SpeedChange()
    {
        _speed += SpeedIncreaseMeasure;
        SpeedChanged?.Invoke(_speed);
    }

    public void ResetSpeed()
    {
        _speed = StartSpeed;
    }
}
