using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CraftableItem : Resource, ICraftable
{
    protected int RequiredAmount;

    public CraftableItem(int requiredAmount)
    {
        RequiredAmount = requiredAmount;
    }

    public abstract Resource[] GetRequiredResources();

    public bool CanCraft(Storage storage)
    {
        Resource[] requiredResources = GetRequiredResources();
        
        foreach (Resource resource in requiredResources)
        {
            if (!storage.Contains(resource.GetName(), resource.GetAmount()))
            {
                return false;
            }
        }
        
        return true;
    }

    public void Craft(Storage storage)
    {
        Resource[] requiredResources = GetRequiredResources();
        
        foreach (Resource resource in requiredResources)
        {
            storage.Take(resource.GetName(), resource.GetAmount());
        }
        
        storage.Store(this, this.GetAmount());
    }
}