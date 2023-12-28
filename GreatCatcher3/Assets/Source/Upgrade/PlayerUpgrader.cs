using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrader : MonoBehaviour
{
   [SerializeField] private Player _player;
   [SerializeField] private ParticleSystem _particleSystem;
   
   private Wallet _playerWallet;

   public int UpgradePrice { get; private set; } = 6000;

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
         const float priceMultiplier = 2.5f;

         if (_playerWallet.Money >= UpgradePrice)
         {
            _particleSystem.Play();
            _playerWallet.ChangeMoney(-UpgradePrice);
            UpgradePrice = (int)(UpgradePrice * priceMultiplier);
            LevelIncreased?.Invoke();
            return true;
         }
      }
      
      return false;
   }
}
