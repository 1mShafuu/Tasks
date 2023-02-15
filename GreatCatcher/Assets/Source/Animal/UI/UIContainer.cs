using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContainer : MonoBehaviour
{
    public CatchBar CatchBar { get; private set; }

    public event Action<GameObject> AnimalDiscovered;

    private void Awake()
    {
        CatchBar = GetComponentInChildren<CatchBar>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out CatchArea catchArea))
        {
            if (catchArea.enabled)
            {
                AnimalDiscovered?.Invoke(other.gameObject);
            }
        }
    }
}
