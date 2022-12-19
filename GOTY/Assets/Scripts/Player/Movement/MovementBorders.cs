using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBorders : MonoBehaviour
{
    public float LeftBorder { get; private set; }
    public float RightBorder { get; private set; }
    public Vector3 CenterPosition { get; private set; }
    
    private void OnEnable()
    {
        GetBorders();
    }

    private void GetBorders()
    {
        float offsetDefinitionBoundaries = 4f;
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit);
        
        if (hit.collider != null)
        {
            CenterPosition = hit.collider.transform.position;
            LeftBorder = CenterPosition.z - offsetDefinitionBoundaries;
            RightBorder = LeftBorder + offsetDefinitionBoundaries + offsetDefinitionBoundaries;
        }
    }
}
