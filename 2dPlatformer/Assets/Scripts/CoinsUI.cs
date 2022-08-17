using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Player _player;

    private void OnEnable()
    {
        _player.AddListener(SetCoins);
    }

    private void OnDisable()
    {
        _player.RemoveListener(SetCoins);
    }
    
    public void SetCoins(int value)
    {
        _text.text = value.ToString();
    }
}
