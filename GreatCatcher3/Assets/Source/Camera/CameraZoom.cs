using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Transform _targetPlayer;
    [SerializeField] private Vector3 _cameraOffset = new(0, 7.25f, -5.35f);
    [SerializeField] private float _landscapeFOV = 75f; // FOV в альбомной ориентации
    [SerializeField] private float _portraitFOV = 90f;  // FOV в портретной ориентации

    private Camera _camera;
    private float _targetFOV;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _targetFOV = _landscapeFOV;
        UpdateCameraPositionAndFOV();
    }

    private void Update()
    {
        UpdateCameraPositionAndFOV();
    }

    private void UpdateCameraPositionAndFOV()
    {
        transform.position = _targetPlayer.position + _cameraOffset;

        if (Screen.width > Screen.height)
        {
            // Альбомный режим
            _targetFOV = _landscapeFOV;
        }
        else
        {
            // Портретный режим
            _targetFOV = _portraitFOV;
        }

        // Плавно изменяем FOV с использованием Lerp
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _targetFOV, Time.deltaTime * 5f);
    }
}