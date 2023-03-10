using System;
using System.Collections;
using System.Collections.Generic;
using Agava.YandexGames;
using UnityEngine;

public class Wallet : MonoBehaviour
{
   [SerializeField] private Game _game;
   
   public int Money { get; private set; } = 0;

   public event Action<int> BalanceChanged;

   private void OnEnable()
   {
      _game.GameEnded += OnGameEnded;
   }

   private void OnDisable()
   {
      _game.GameEnded -= OnGameEnded;
   }

   private void Start()
   {
      BalanceChanged?.Invoke(Money);
#if !UNITY_WEBGL || UNITY_EDITOR
      return;
#endif
      PlayerAccount.GetPlayerData((data) => Money=Convert.ToInt32(data.Substring(1)));
   }

   public void ChangeMoney(int value)
   {
      if (Money + value >= 0)
      {
         Money += value;
      }
      
      BalanceChanged?.Invoke(Money);
   }

   private void OnGameEnded()
   {
      Money = 0;
   }
}
