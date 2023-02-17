using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int _level = 1;
    private Wallet _wallet;
    private UpgradePlayer _upgrader;
    
    public int Level => _level;

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

    public void InitUpgrader(UpgradePlayer upgrader)
    {
        _upgrader = upgrader.GetComponent<UpgradePlayer>();
        _upgrader.LevelIncreased += OnLevelChanged;
    }
    
    private void OnLevelChanged()
    {
        _level++;
        Debug.Log(_level);
    }
}
