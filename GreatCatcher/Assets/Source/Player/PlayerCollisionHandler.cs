using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private AnimalsUnloader _unloader;
    private CatchArea _catchArea;
    private Player _player;
    private ArrowRenderer _arrowRenderer;
    
    private void Awake()
    {
        _player = GetComponent<Player>();
        _unloader = GetComponent<AnimalsUnloader>();
        _catchArea = GetComponentInChildren<CatchArea>();
        _arrowRenderer = GetComponentInChildren<ArrowRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out UnloadArea area))
        {
            _unloader.Unload();
            _arrowRenderer.gameObject.SetActive(false);
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

        if (other.TryGetComponent(out PlayerUpgrader upgrader))
        {
            _player.InitUpgrader(upgrader);
            upgrader.TryUpgradePlayer();
        }

        if (other.TryGetComponent(out ArrowStopper stopper))
        {
            _arrowRenderer.gameObject.SetActive(false);
        }
    }
}
