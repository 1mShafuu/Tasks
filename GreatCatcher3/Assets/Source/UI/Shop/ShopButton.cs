using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : OpenMenuButton
{
    [SerializeField] private ShopMenu _shop;
    
    protected override void OnButtonClicked()
    {
        _shop.Open();
    }
}
