using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoHolder : MonoBehaviour
{
   private PlayerInfo _playerInfo;

   public PlayerInfo PlayerInfoStats
   {
      get { return _playerInfo; }
      
      private set
      {
         if (value != _playerInfo)
         {
            _playerInfo = value;
           // Debug.Log($" ХОЛДЕР prop  {PlayerInfoStats.Yard}    {PlayerInfoStats.Wallet}  {PlayerInfoStats.Level}");
            NotifyFieldChanged();
         }
      }
   }
   
   private List<Action> _onFieldChangedCallbacks = new List<Action>();

   public void GetPlayerStats(PlayerInfo stats)
   {
      PlayerInfoStats = stats;
      //Debug.Log($" ХОЛДЕР method  {PlayerInfoStats.Yard}    {PlayerInfoStats.Wallet}  {PlayerInfoStats.Level}");
   }
   
   public void AddFieldChangedCallback(Action callback)
   {
      _onFieldChangedCallbacks.Add(callback);
   }

   public void RemoveFieldChangedCallback(Action callback)
   {
      _onFieldChangedCallbacks.Remove(callback);
   }

   private void NotifyFieldChanged()
   {
      foreach (var callback in _onFieldChangedCallbacks)
      {
         callback?.Invoke();
      }
   }
}
