using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private AnimalsUnloader _unloader;
    private CatchArea _catchArea;
    private Player _player;
    
    private void Awake()
    {
        _player = GetComponent<Player>();
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
            _catchArea.gameObject.SetActive(true);
            _catchArea.enabled = true;
        }

        if (other.TryGetComponent(out SafeArea safeArea))
        {
            _catchArea.gameObject.SetActive(false);
            _catchArea.enabled = false;
        }

        if (other.TryGetComponent(out UpgradePlayer upgrader))
        {
            _player.InitUpgrader(upgrader);
            upgrader.TryUpgradePlayer();
        }
    }
}
