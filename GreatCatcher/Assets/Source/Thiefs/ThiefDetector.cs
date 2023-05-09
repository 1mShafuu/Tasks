using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefDetector : MonoBehaviour
{
    public event Action ThiefDetected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            ThiefDetected?.Invoke();
        }
    }
}
