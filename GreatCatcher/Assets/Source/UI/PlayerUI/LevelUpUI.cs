using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerUpgrader _playerUpgrader;
    [SerializeField] private TMP_Text _text;

    private void Start()
    {
        _text.text = _playerUpgrader.UpgradePrice.ToString();
    }

    private void OnEnable()
    {
        _playerUpgrader.LevelIncreased += OnLevelIncreased;
    }

    private void OnDisable()
    {
        _playerUpgrader.LevelIncreased -= OnLevelIncreased;
    }

    private void OnLevelIncreased()
    {
        if (_player.Level < Player.MaxLevel - 1)
        {
            _text.text = _playerUpgrader.UpgradePrice.ToString();
        }
        else
        {
            _text.text = "LEVEL MAXED";
        }
    }
}
