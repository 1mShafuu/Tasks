using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private Text _text;

    private Player _player;
    
    public void SetCoins(int value)
    {
        _text.text = value.ToString();
    }

    private void OnEnable()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<Player>();
        }
        
        _player.AddListener(SetCoins);
    }

    private void OnDisable()
    {
        _player.RemoveListener(SetCoins);
    }
}
