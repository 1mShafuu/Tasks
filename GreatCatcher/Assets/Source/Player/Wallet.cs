using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
   public int Money { get; private set; } = 5000;

   public event Action<int> BalanceChanged;

   private void Start()
   {
      BalanceChanged?.Invoke(Money);
   }

   public void ChangeMoney(int value)
   {
      if (Money >= Mathf.Abs(value))
      {
         Money += value;
      }
      
      BalanceChanged?.Invoke(Money);
   }
}
