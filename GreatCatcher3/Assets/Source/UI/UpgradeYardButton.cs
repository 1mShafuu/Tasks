using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeYardButton : MonoBehaviour
{

   [SerializeField] private Button _upgradeYardAreaButton;
   [SerializeField] private Player _player;
   [SerializeField] private List<GameObject> _yards = new List<GameObject>();
   [SerializeField] private PlayerInfoHolder _playerInfoHolder;
   
   private int _upgradePrice = 10000;
   private Wallet _wallet;

   public event Action YardUpgraded;
   public event Action YardNotUpgraded;

   private void OnEnable()
   { 
      _playerInfoHolder.AddFieldChangedCallback(OnStatsGained);
      _upgradeYardAreaButton.onClick.AddListener(OnButtonClicked);
      _wallet = _player.gameObject.GetComponent<Wallet>();
   }

   private void OnDisable()
   {
      _upgradeYardAreaButton.onClick.RemoveListener(OnButtonClicked);
   }
   
   private void OnButtonClicked()
   {
      if (_wallet.Money < _upgradePrice)
      {
         YardNotUpgraded?.Invoke();
         return;
      }
      
      const int upgradePriceModifier = 5;
      int nextActiveYardIndex = 0;
      _wallet.ChangeMoney(-_upgradePrice);

      for (int index = 0; index < _yards.Count; index++)
      {
         if (_yards[index].activeSelf)
         {
            _yards[index].SetActive(false);
            nextActiveYardIndex = index + 1;
         }
      }
         
      _yards[nextActiveYardIndex].SetActive(true);
      _upgradePrice *= upgradePriceModifier;
      YardUpgraded?.Invoke();
   }

   private void OnStatsGained()
   {
      var targetYardLevel = _playerInfoHolder.PlayerInfoStats.Yard - 1;
      
      for (int index = 0; index < _yards.Count; index++)
      {
         if (targetYardLevel == index)
         {
            _yards[index].SetActive(true);
         }
         else
         {
            _yards[index].SetActive(false);
         }
      }
   }
}
