using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
   public int Money { get; private set; }

   public void ChangeMoney(int value)
   {
      Money += value;
   }
}
