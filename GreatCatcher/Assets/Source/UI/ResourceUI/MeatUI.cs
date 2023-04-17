using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatUI : ResourceUI
{
    private Meat _meat = new Meat();
    
    public override string GetName()
    {
        return _meat.GetName();
    }

    public override List<string> GetCraftedResources()
    {
        return null;
    }

    public override int GetPrice()
    {
        return _meat.GetPrice();
    }
}
