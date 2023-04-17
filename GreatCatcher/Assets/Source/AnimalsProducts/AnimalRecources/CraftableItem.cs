using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CraftableItem : Resource, ICraftable
{
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
            var c = 0;
            Debug.Log($"{++c} {storage.TryTake(resource, resource.GetAmount())}");
            if (storage.TryTake(resource, resource.GetAmount()))
            {
                storage.Take(resource.GetName(), resource.GetAmount());
            }
        }
        
        storage.Store(this, this.GetAmount());
    }
}