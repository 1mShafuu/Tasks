using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchAdArea : MonoBehaviour
{
    public event Action PlayerEntered;
    public event Action PlayerExit;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            PlayerEntered?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            PlayerExit?.Invoke();
        }
    }
}
