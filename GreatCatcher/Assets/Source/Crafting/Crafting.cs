using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    [SerializeField] private Storage _storage;

    private readonly CraftableItem[] _items = new CraftableItem[] {new Omelette(), new MilkShake(), new WoolenSweater()};

    public IReadOnlyList<CraftableItem> Items => _items;
    public Storage Storage => _storage;
    
    public void CraftResource(ResourceUI item)
    {
        var resourceToCraft = _items.FirstOrDefault(resource => resource.GetName() == item.GetName());

        if (resourceToCraft != null && resourceToCraft.CanCraft(_storage))
        {
            resourceToCraft.Craft(_storage);
            _storage.ShowResources();
        }
    }
}
