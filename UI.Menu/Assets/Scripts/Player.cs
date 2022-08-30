using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   private const float _changeValue = 10;
   
   private float _health;

   public float Health => _health;

   public Player()
   {
      _health = 100f;
   }

   public void TakeDamage()
   {
      _health -= _changeValue;
   }

   public void TakeHeal()
   {
      _health += _changeValue;
   }
}
