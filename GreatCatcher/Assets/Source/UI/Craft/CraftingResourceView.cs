using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lean.Localization;
using UnityEngine;

public class CraftingResourceView : ResourceView
{
    private Crafting _crafting;

    public override void Render(ResourceUI resource, Crafting crafting = null)
    {
        _crafting = crafting;
        ResourceUIElement = resource;
        
        if (ResourcesTranslations.ResourcesTranslationsDictionary.Count != 0)
        {
            foreach (var resourceTranslation in ResourcesTranslations.ResourcesTranslationsDictionary)
            {
                if (resourceTranslation.Key == resource.GetName())
                {
                    Label.text = resourceTranslation.Value;
                    Debug.Log(Label.text);
                }
            }
        }
        else
        {
            Label.text = resource.GetName();
        }

        Icon.sprite = resource.ResourceImage.sprite;

        foreach (var name in resource.GetCraftedResources())
        {
            if (RequiredResources.text == "")
            {
                RequiredResources.text += $" 1x {name}";
            }
            else
            {
                RequiredResources.text += $" - 1x {name} ";
            }
        }

        RequiredResources.text = RequiredResources.text.TrimEnd('-');

        string tempText = RequiredResources.text;
        var splitedTempText = tempText.Split(" ");

        foreach (var name in ResourcesTranslations.ResourcesTranslationsDictionary)
        {
            var foundWord = splitedTempText.FirstOrDefault(word => word == name.Key);
            
            if (foundWord != null)
            {
                tempText = tempText.Replace(foundWord, name.Value);
            }
        }

        RequiredResources.text = tempText;
    }

    protected override void OnButtonClicked()
    {
        _crafting.CraftResource(ResourceUIElement);
    }
}
