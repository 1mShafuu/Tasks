using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
   private const float MaxHealth = 100f;
   private const float MinHealth = 0f;
   
   public event UnityAction HealthChanged;

   public float Health { get; private set; }

   public Player()
   {
      Health = 88f;
   }

   public void TakeDamage(float value)
   {
      Health -= value;
      NormalizeHealthValue();
   }

   public void TakeHealth(float value)
   {
      Health += value;
      NormalizeHealthValue();
   }
   
   private void NormalizeHealthValue()
   {
      Health = Mathf.Clamp(Health, MinHealth, MaxHealth);
      HealthChanged?.Invoke();
   }
}
