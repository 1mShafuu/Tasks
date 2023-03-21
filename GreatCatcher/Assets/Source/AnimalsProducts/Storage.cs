using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private Dictionary<string, int> _resources = new Dictionary<string, int>();
    
    public void Store(Resource resource, int amount)
    {
        string resourceName = resource.GetName();
        
        if (_resources.ContainsKey(resourceName))
        {
            _resources[resourceName] += amount;
        }
        else
        {
            _resources.Add(resourceName, amount);
        }
    }

    public bool TryTake(Resource resource, int amount)
    {
        string resourceName = resource.GetName();
        
        if (_resources.ContainsKey(resourceName) && _resources[resourceName] >= amount)
        {
            _resources[resourceName] -= amount;
            return true;
        }
        
        return false;
    }

    public bool Contains(string resourceName, int amount)
    {
        return _resources.ContainsKey(resourceName) && _resources[resourceName] >= amount;
    }
    
    public void Take(string resourceName, int amount)
    {
        if (_resources.ContainsKey(resourceName))
        {
            _resources[resourceName] -= amount;
        }
    }
    
    public void ShowResources()
    {
        foreach (var resource in _resources)
        {
            Debug.Log($"Name - {resource.Key}, Amount - {resource.Value}");
        }
    }
}
