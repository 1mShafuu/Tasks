using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int _level = 1;
    private Wallet _wallet;
    private PlayerUpgrader _upgrader;
    
    public int Level => _level;
    public int MaxLevel { get; private set; } = 3;

    private void Awake()
    {
        _wallet = GetComponent<Wallet>();
    }
    
    private void OnDisable()
    {
        if (_upgrader != null)
        {
            _upgrader.LevelIncreased -= OnLevelChanged;
        }
    }

    public void InitUpgrader(PlayerUpgrader upgrader)
    {
        _upgrader = upgrader.GetComponent<PlayerUpgrader>();
        _upgrader.LevelIncreased += OnLevelChanged;
    }
    
    private void OnLevelChanged()
    {
        if (_level >= MaxLevel) return;
        _level++;
    }
}
