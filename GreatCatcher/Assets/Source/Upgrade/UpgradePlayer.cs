using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePlayer : MonoBehaviour
{
   
   [SerializeField] private Player _player;

   private Wallet _playerWallet;

   public int UpgradePrice { get; private set; } = 4500;

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

         if (_playerWallet.Money >= UpgradePrice)
         {
            _playerWallet.ChangeMoney(-UpgradePrice);
            LevelIncreased?.Invoke();
            Debug.Log(_player.Level);
            
            return true;
         }
      }
      
      return false;
   }
}
