using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CraftableItem : Resource, ICraftable
{
    private Dictionary<string, int> _requiredResources;
    
    public abstract Resource[] GetRequiredResources();
    
    public bool CanCraft(Storage storage)
    {
        Resource[] requiredResources = GetRequiredResources();
        _requiredResources = new Dictionary<string, int>();

        foreach (var resource in requiredResources)
        {
            string key = resource.GetName();
            
            if (_requiredResources.ContainsKey(key))
            {
                _requiredResources[key] += 1;
            }
            else
            {
                _requiredResources[key] = 1;
            }
        }

        foreach (string key in _requiredResources.Keys)
        {
            int amount = _requiredResources[key];
            
            if (!storage.Contains(key, amount))
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
            if (storage.TryTake(resource.GetName(), resource.GetAmount()))
            {
                storage.Take(resource.GetName(), resource.GetAmount());
            }
        }
        
        storage.Store(this, GetAmount());
    }
}