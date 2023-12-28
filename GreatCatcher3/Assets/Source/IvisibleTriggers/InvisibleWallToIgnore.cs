using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWallToIgnore : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out Animal animal)) return;
        
        if (animal.IsCaught)
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider>(), GetComponent<BoxCollider>(), true);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, new Vector3(2, 5, 20));
    }
}
