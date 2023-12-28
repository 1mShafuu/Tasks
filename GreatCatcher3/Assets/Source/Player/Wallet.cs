using System;
using System.Collections;
using System.Collections.Generic;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.Serialization;

public class Wallet : MonoBehaviour
{
   [SerializeField] private Game _game;
   [SerializeField] private PlayerInfoHolder _playerInfoHolder;
   
   public int Money { get; private set; } = 0;

   public event Action<int> BalanceChanged;

   private void OnEnable()
   {
      _game.GameStarted += OnGameStarted;
      _game.GameEnded += OnGameEnded;
      _playerInfoHolder.AddFieldChangedCallback(OnStatsGained);
   }

   private void OnDisable()
   {
      _game.GameStarted -= OnGameStarted;
      _game.GameEnded -= OnGameEnded;
   }

   private void Start()
   {
      //BalanceChanged?.Invoke(Money);
   }
   
   public void ChangeMoney(int value)
   {
      if (Money + value >= 0)
      {
         Money += value;
      }
      
      BalanceChanged?.Invoke(Money);
   }

   private void OnGameStarted()
   {
      BalanceChanged?.Invoke(Money);
   }

   private void OnGameEnded()
   {
      Money = 0;
   }
   
   private void OnStatsGained()
   {
      ChangeMoney(_playerInfoHolder.PlayerInfoStats.Wallet);
   }
}
