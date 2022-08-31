using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   private const float _changeValue = 10;
   private const float maxHealth = 100f;
   private const float minHealth = 0f;
   
   private float _health;

   public float Health => _health;

   public Player()
   {
      _health = 100f;
   }

   public void TakeDamage()
   {
      if (_health > minHealth)
      {
         _health -= _changeValue;
      }
   }

   public void TakeHeal()
   {
      if (_health < maxHealth)
      {
         _health += _changeValue;
      }
   }
}
