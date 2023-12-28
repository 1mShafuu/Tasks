using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopResourceView : ResourceView
{
    [SerializeField] private TMP_Text _amountText;
    
    private WalletUI _walletUI;
    private Wallet _wallet;
    private Storage _storage;

    private void Awake()
    {
        _walletUI = GetComponentInParent<WalletUI>();
        _storage = _walletUI.GetComponentInChildren<CraftingResourcesInfo>().Storage;
        _wallet = _walletUI.Wallet;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _storage.ResourcesChanged += OnResourcesChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _storage.ResourcesChanged -= OnResourcesChanged;
    }
    
    public override void Render(ResourceUI resource, Crafting crafting = null)
    {
        ResourceUIElement = resource;

        if (ResourcesTranslations.ResourcesTranslationsDictionary.Count != 0)
        {
            foreach (var resourceTranslation in ResourcesTranslations.ResourcesTranslationsDictionary)
            {
                if (resourceTranslation.Key == resource.GetName())
                {
                    Label.text = resourceTranslation.Value;
                }
            }
        }
        else
        {
            Label.text = resource.GetName();
        }

        Icon.sprite = resource.ResourceImage.sprite;
    }

    protected override void OnButtonClicked()
    {
        const int resourceAmountToSell = 1;
        
        if (_storage.TryTake(ResourceUIElement.GetName(), resourceAmountToSell))
        {
            _storage.Take(ResourceUIElement.GetName(),resourceAmountToSell);
            _wallet.ChangeMoney(ResourceUIElement.GetPrice());
        }
    }

    private void OnResourcesChanged(string resourceName, int resourceAmount)
    {
        if (ResourceUIElement.GetName() == resourceName)
        {
            _amountText.text = resourceAmount.ToString();
        }
    }
}
