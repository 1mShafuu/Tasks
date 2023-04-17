using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingItemHolder : ItemsHolderMenu
{
    [SerializeField] private Crafting _crafting;
    
    protected override void AddItem(ResourceUI resource)
    {
        var view = Instantiate(Template, ItemContainer.transform);
        view.Render(resource, _crafting);
    }
}
