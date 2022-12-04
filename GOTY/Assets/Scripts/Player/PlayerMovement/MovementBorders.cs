using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBorders : MonoBehaviour
{
    public float LeftBorder { get; private set; }
    public float RightBorder { get; private set; }
    public Transform CenterPosition { get; private set; }
    
    private void Awake()
    {
        GetBorders();
    }

    private void GetBorders()
    {
        float avoidDistance = 4.5f;
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit);
        
        if (hit.collider != null)
        {
            CenterPosition = hit.collider.transform;
            LeftBorder = CenterPosition.position.z - avoidDistance;
            RightBorder = LeftBorder + avoidDistance + avoidDistance;
        }
    }
}
