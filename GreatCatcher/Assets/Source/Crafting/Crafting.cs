using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    [SerializeField] private Storage _storage;
    

    public void CraftMilkshake()
    {
        MilkShake milkshake = new MilkShake();
        
        if (milkshake.CanCraft(_storage))
        {
            milkshake.Craft(_storage);
        }
    }

    public void CraftOmelette()
    {
        Omelette omelette = new Omelette();
        
        if (omelette.CanCraft(_storage))
        {
            omelette.Craft(_storage);
        }
    }
    
    public void CraftWoolSweater()
    {
        WoolenSweater woolenSweater = new WoolenSweater();
        
        if (woolenSweater.CanCraft(_storage))
        {
            woolenSweater.Craft(_storage);
            _storage.ShowResources();
        }
    }
}
