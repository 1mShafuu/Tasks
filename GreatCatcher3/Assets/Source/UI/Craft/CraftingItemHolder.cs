using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingItemHolder : ItemsHolderMenu
{
    [SerializeField] private Crafting _crafting;

    protected override void AddItem(ResourceUI resource)
    {
        Add(resource);
    }
    
    private void Add(ResourceUI resource)
    {
        var view = Instantiate(Template, ItemContainer.transform);
        view.Render(resource, _crafting);
    }
}
