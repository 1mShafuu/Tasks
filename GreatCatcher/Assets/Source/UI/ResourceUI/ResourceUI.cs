using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ResourceUI : MonoBehaviour
{
    [SerializeField] protected TMP_Text Label;
    [SerializeField] protected Image Image;

    public TMP_Text Text => Label;
    public Image ResourceImage => Image;

    public abstract string GetName();

    public abstract List<string> GetCraftedResources();

    public abstract int GetPrice();
    
    protected List<string> GetRequiredResourcesList(CraftableItem item)
    {
        var requiredResources = item.GetRequiredResources();
        List<string> requiredResourcesNames = new List<string>();

        foreach (var resource in requiredResources)
        {
            requiredResourcesNames.Add(resource.GetName());
        }

        return requiredResourcesNames;
    }
}
