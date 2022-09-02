using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
   private const float ChangeValue = 10;
   private const float MaxHealth = 100f;
   private const float MinHealth = 0f;
   
   [SerializeField] private UnityEvent PlayerHealthChanged;
   
   private float _health;

   public float Health => _health;

   public Player()
   {
      _health = 100f;
   }

   public void TakeDamage()
   {
      if (_health > MinHealth)
      {
         _health -= ChangeValue;
         PlayerHealthChanged.Invoke();
      }
   }

   public void TakeHeal()
   {
      if (_health < MaxHealth)
      {
         _health += ChangeValue;
         PlayerHealthChanged.Invoke();
      }
   }
}
