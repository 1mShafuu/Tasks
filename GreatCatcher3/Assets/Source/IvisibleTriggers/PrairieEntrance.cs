using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrairieEntrance : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(2, 2, 40));
    }
}
