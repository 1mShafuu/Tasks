using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private AnimalsUnloader _unloader;
    private CatchArea _catchArea;

    private void Awake()
    {
        _unloader = GetComponent<AnimalsUnloader>();
        _catchArea = GetComponentInChildren<CatchArea>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out UnloadArea area))
        {
            _unloader.Unload();
        }

        if (other.TryGetComponent(out PrairieEntrance entrance))
        {
            _catchArea.enabled = true;
        }

        if (other.TryGetComponent(out SafeArea safeArea))
        {
            _catchArea.enabled = false;
        }
    }
}
