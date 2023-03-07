using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrader : MonoBehaviour
{
   
   [SerializeField] private Player _player;

   private Wallet _playerWallet;

   public int UpgradePrice { get; private set; } = 2000;

   public event Action LevelIncreased;

   private void Awake()
   {
      _playerWallet = _player.GetComponent<Wallet>();
   }

   public bool TryUpgradePlayer()
   {
      if (_player.TryGetComponent(out Wallet wallet))
      {
         _playerWallet = wallet;
         const int priceMultiplier = 5;

         if (_playerWallet.Money >= UpgradePrice)
         {
            _playerWallet.ChangeMoney(-UpgradePrice);
            UpgradePrice *= priceMultiplier;
            LevelIncreased?.Invoke();
            return true;
         }
      }
      
      return false;
   }
}
