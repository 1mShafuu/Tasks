using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using UnityEngine.UI;

public class SaveProgressButton : MonoBehaviour
{
    [SerializeField] private Button _saveButton;
    [SerializeField] private Player _player;

    private Wallet _wallet;

    private void OnEnable()
    {
        _saveButton.onClick.AddListener(OnSaveButtonClicked);
    }

    private void OnDisable()
    {
        _saveButton.onClick.RemoveListener(OnSaveButtonClicked);
    }

    private void OnSaveButtonClicked()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized)
        {
            PlayerAccount.SetPlayerData(_player.Level.ToString() + _wallet.Money.ToString());
        }
#endif
    }
}
