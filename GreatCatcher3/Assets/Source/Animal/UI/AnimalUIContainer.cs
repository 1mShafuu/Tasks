using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalUIContainer : MonoBehaviour
{
    public CatchBar CatchBar { get; private set; }
    public EquipSlotLock LockImage { get; private set; }
    public Animal AnimalToCatch { get; private set; }

    public event Action<GameObject> AnimalDiscovered;

    private void Awake()
    {
        CatchBar = GetComponentInChildren<CatchBar>();
        LockImage = GetComponentInChildren<EquipSlotLock>();
        AnimalToCatch = GetComponent<Animal>();
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

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CatchArea catchArea))
        {
            LockImage.Close();
            CatchBar.TurnOffCanvas();
        }
    }
}
