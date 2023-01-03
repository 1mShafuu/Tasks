using UnityEngine;

public class SurfaceSlider : MonoBehaviour
{
    private Vector3 _normal;
    
    private void OnCollisionStay(Collision collision)
    {
        _normal = collision.contacts[0].normal;
    }

    public Vector3 Project(Vector3 forward)
    {
        return forward - _normal * Vector3.Dot(forward, _normal);
    }
}