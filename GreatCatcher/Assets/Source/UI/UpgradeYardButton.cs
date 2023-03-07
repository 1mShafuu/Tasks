using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeYardButton : MonoBehaviour
{

   [SerializeField] private Button _upgradeYardAreaButton;
   [SerializeField] private Player _player;
   [SerializeField] private List<GameObject> _yards = new List<GameObject>();

   private int _upgradePrice = 20000;
   private Wallet _wallet;

   public event Action YardUpgraded;
   
   private void Start()
   {
      _wallet = _player.gameObject.GetComponent<Wallet>();
   }

   private void OnEnable()
   {
      _upgradeYardAreaButton.onClick.AddListener(OnButtonClicked);
   }

   private void OnDisable()
   {
      _upgradeYardAreaButton.onClick.RemoveListener(OnButtonClicked);
   }

   private void OnButtonClicked()
   {
      int nextActiveYardIndex = 0;
      
      if (_wallet.Money >= _upgradePrice)
      {
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
         _upgradePrice *= 2;
         YardUpgraded?.Invoke();
      }
   }
}
