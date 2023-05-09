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
      _game.GameStarted += OnGameStarted;
   }

   private void OnDisable()
   {
      _game.GameStarted -= OnGameStarted;
   }

   private IEnumerator Start()
   {
      int possibleMoneyBalance = 0;
      BalanceChanged?.Invoke(Money);

      yield return null;
#if UNITY_WEBGL && !UNITY_EDITOR
      yield return YandexGamesSdk.Initialize();

       if (PlayerAccount.IsAuthorized && YandexGamesSdk.IsInitialized)
       {
          PlayerAccount.GetPlayerData((data) => 
            possibleMoneyBalance = Convert.ToInt32(data.Substring(1))
          ); 
       }

       Debug.Log(possibleMoneyBalance);
       
       if (possibleMoneyBalance != 0)
       {
          Money = possibleMoneyBalance;
          BalanceChanged?.Invoke(Money);
       }
#endif
   }

   // ReSharper disable Unity.PerformanceAnalysis
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
      Money = 0;
      BalanceChanged?.Invoke(Money);
   }
}
