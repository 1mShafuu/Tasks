using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
   private const float MaxHealth = 100f;
   private const float MinHealth = 0f;
   
   public UnityAction PlayerHealthChanged;
   
   private float _health;

   public float Health => _health;

   public Player()
   {
      _health = 88f;
   }

   public void TakeDamage(float value)
   {
      _health -= value;
      NormalizeHealthValue();
   }

   public void TakeHealth(float value)
   {
      _health += value;
      NormalizeHealthValue();
   }
   
   private void NormalizeHealthValue()
   {
      _health = Mathf.Clamp(_health, MinHealth, MaxHealth);
      PlayerHealthChanged?.Invoke();
   }
}
