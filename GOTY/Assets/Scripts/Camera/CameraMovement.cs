using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _targetPlayer;
    [SerializeField] private Vector3 _cameraOffset;
    
    private void Update()
    {
        transform.position = _targetPlayer.position + _cameraOffset;
    }
}
