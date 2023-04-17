using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingResourceView : ResourceView
{
    private Crafting _crafting;

    public override void Render(ResourceUI resource, Crafting crafting = null)
    {
        _crafting = crafting;
        ResourceUIElement = resource;
        // Debug.Log(_resourceUI);
        Label.text = resource.GetName();
        Icon.sprite = resource.ResourceImage.sprite;

        foreach (var name in resource.GetCraftedResources())
        {
            RequiredResources.text += $"Required - 1x {name} ";
        }
    }

    protected override void OnButtonClicked()
    {
        _crafting.CraftResource(ResourceUIElement);
    }
}
