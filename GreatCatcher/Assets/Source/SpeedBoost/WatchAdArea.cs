using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchAdArea : MonoBehaviour
{
    [SerializeField] private WatchAdNotification _adNotification;
    [SerializeField] private Player _player;

    private PhysicsMovement _movement;
    
    public event Action PlayerEntered;
    public event Action PlayerExit;

    private void Awake()
    {
        _movement = _player.GetComponent<PhysicsMovement>();
    }

    private void OnEnable()
    {
        _adNotification.WatchAdButtonClicked += OnWatchAdButtonClicked;
    }

    private void OnDisable()
    {
        _adNotification.WatchAdButtonClicked -= OnWatchAdButtonClicked;
    }

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

    private void OnWatchAdButtonClicked()
    {
        _movement.IncreaseSpeed();
    }
}
