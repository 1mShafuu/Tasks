using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkUI : ResourceUI
{
    private Milk _milk = new Milk();
    
    public override string GetName()
    {
        return _milk.GetName();
    }

    public override List<string> GetCraftedResources()
    {
        return null;
    }

    public override int GetPrice()
    {
        return _milk.GetPrice();
    }
}
