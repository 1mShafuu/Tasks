using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _targetPlayer;

    private void Update()
    {
        transform.position = _targetPlayer.position + new Vector3(-4.5f,2.1f,0);
    }
}
