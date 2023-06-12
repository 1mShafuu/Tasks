using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AviaryNotification : Notification
{
   [SerializeField] private UpgradeYardButton _upgradeYardButton;

   private Coroutine _coroutine;
   
   private void OnEnable()
   {
      _upgradeYardButton.YardNotUpgraded += OnYardNotUpgraded;
   }

   private void OnDisable()
   {
      _upgradeYardButton.YardNotUpgraded -= OnYardNotUpgraded;
   }

   private void Start()
   {
      transform.localScale = Vector2.zero;
   }

   private void OnYardNotUpgraded()
   {
      if (_coroutine != null)
      {
         StopCoroutine(_coroutine);
      }
      
      _coroutine = StartCoroutine(NotificationShown(this));
   }

   protected override void Open()
   {
      transform.LeanScale(Vector2.one, 0.8f);
   }

   protected override void Close()
   {
      transform.LeanScale(Vector2.zero, 1f).setEaseInBack();
   }
}
