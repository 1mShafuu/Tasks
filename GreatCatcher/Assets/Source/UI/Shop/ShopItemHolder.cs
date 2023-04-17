using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemHolder : ItemsHolderMenu
{
    [SerializeField] private Wallet _wallet;
    
    protected override void AddItem(ResourceUI resource)
    {
        var view = Instantiate(Template, ItemContainer.transform);
        view.Render(resource);
    }
}
