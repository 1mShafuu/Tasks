using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SurfaceSlider))]
public class PhysicsMovement : MonoBehaviour
{
    private const int SpeedIncreaseFactor = 2;
    
    [SerializeField] private float _currentSpeed;
    
    private Rigidbody _rigidbody;
    private SurfaceSlider _surfaceSlider;
    private Animator _animator;
    private Coroutine _coroutine;
    private float _increasedSpeed;
    private float _defaultSpeed;
    private bool _isColliding;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _surfaceSlider = GetComponent<SurfaceSlider>();
        _defaultSpeed = _currentSpeed;
        _increasedSpeed = _currentSpeed * SpeedIncreaseFactor;
    }

    public void Move(Vector3 direction)
    {
        const float maxDegreeDelta = 10f;
        
        RaycastHit hitInfo;
        
        if (Physics.Raycast(transform.position + (direction + Vector3.up) * 0.7f, direction.normalized, out hitInfo, 0.5f))
        {
            if (hitInfo.collider.isTrigger == false && !hitInfo.collider.TryGetComponent(out InvisibleWallToIgnore wall))
            {
                //Debug.Log("Препятствие обнаружено: " + hitInfo.collider.name);
                return; // Останавливаем выполнение метода, если есть препятствие
            }
        }
        
        if (direction != Vector3.zero)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _animator.Play("Run");
            }
            
            Quaternion toRotation = Quaternion.LookRotation(direction,Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,toRotation,maxDegreeDelta);
        }
        else if (!_animator.GetBool("IsCatching"))
        {
            _animator.Play("Idle");
        }

        Vector3 directionAlongSurface = _surfaceSlider.Project(direction.normalized);
        Vector3 offset = directionAlongSurface * (_currentSpeed * Time.deltaTime);
        Vector3 newVectorPosition = _rigidbody.position + offset;
        
        _rigidbody.MovePosition(newVectorPosition);
    }

    public void IncreaseSpeed()
    {
        const float waitingSeconds = 50f;
        
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _coroutine = StartCoroutine(SpeedChanger( waitingSeconds));
    }

    private IEnumerator SpeedChanger( float waitingSeconds)
    {
        bool watchAdBuff = true;
        var waitForSeconds = new WaitForSeconds(waitingSeconds);
        
        while (watchAdBuff)
        {
            _currentSpeed = _increasedSpeed;
            yield return waitForSeconds;
            watchAdBuff = false;
            _currentSpeed = _defaultSpeed;
        }
    }
}
