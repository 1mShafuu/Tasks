using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePlayer : MonoBehaviour
{
   
   [SerializeField] private Player _player;

   private Wallet _playerWallet;

   public int UpgradePrice { get; private set; } = 4500;

   public event Action<int> LevelIncreased;

   private void Awake()
   {
      _playerWallet = _player.GetComponent<Wallet>();
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.TryGetComponent(out Player player))
      {
         TryUpgradePlayer();
      }
   }

   public bool TryUpgradePlayer()
   {
      if (_player.TryGetComponent(out Wallet wallet))
      {
         _playerWallet = wallet;
         var playerLevel = _player.Level;
         Debug.Log(playerLevel);
         
         if (_playerWallet.Money >= UpgradePrice)
         {
            _playerWallet.ChangeMoney(-UpgradePrice);
            playerLevel++;
            LevelIncreased?.Invoke(playerLevel);
            Debug.Log(playerLevel);
            return true;
         }
      }
      
      return false;
   }
}
