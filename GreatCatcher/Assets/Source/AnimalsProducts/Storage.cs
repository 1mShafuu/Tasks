using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private Game _game;
    
    private Dictionary<string, List<int>> _resources;

    public IReadOnlyDictionary<string, List<int>> Resources => _resources;

    public event Action<string, int> ResourcesChanged;

    private void Awake()
    {
        SetResourcesAtStart();
    }

    private void OnEnable()
    {
        _game.GameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        _game.GameEnded -= OnGameEnded;
    }

    private void Start()
    {
        foreach (var resource in _resources)
        {
            ResourcesChanged?.Invoke(resource.Key, GetTotalAmount(resource.Key));
        }
        
        ShowResources();
    }

    public void Store(Resource resource, int amount)
    {
        string resourceName = resource.GetName();

        if (_resources.ContainsKey(resourceName))
        {
            _resources[resourceName].Add(amount);
        }
        else
        {
            _resources.Add(resourceName, new List<int>() {amount});
        }

        ResourcesChanged?.Invoke(resourceName, GetTotalAmount(resourceName));
    }

    public bool TryTake(string resourceName, int amount)
    {
        if (_resources.ContainsKey(resourceName) && _resources[resourceName].Count >= amount)
        {
            return true;
        }

        return false;
    }

    public bool Contains(string resourceName, int amount)
    {
        return GetTotalAmount(resourceName) >= amount;
    }
    
    public void Take(string resourceName, int amount)
    {
        if (_resources.ContainsKey(resourceName))
        {
            _resources[resourceName].RemoveRange(0, amount);
            ResourcesChanged?.Invoke(resourceName, GetTotalAmount(resourceName));
            ShowResources();
        }
    }

    public void ShowResources()
    {
        foreach (var resource in _resources)
        {
           // Debug.Log($"Name - {resource.Key}, Amount - {GetTotalAmount(resource.Key)}");
        }
    }
    
    private int GetTotalAmount(string resourceName)
    {
        if (_resources.ContainsKey(resourceName))
        {
            int totalAmount = 0;

            foreach (int amount in _resources[resourceName])
            {
                totalAmount += amount;
            }

            return totalAmount;
        }

        return 0;
    }

    private void SetResourcesAtStart()
    {
        _resources = new Dictionary<string, List<int>>()
        {
            { "Milk", new List<int>() },
            { "Wool", new List<int>() },
            { "Meat", new List<int>() },
            { "Egg", new List<int>() }
        };
    }

    private void OnGameEnded()
    {
        SetResourcesAtStart();
        
        foreach (var resource in _resources)
        {
            ResourcesChanged?.Invoke(resource.Key, GetTotalAmount(resource.Key));
        }
    }
}
