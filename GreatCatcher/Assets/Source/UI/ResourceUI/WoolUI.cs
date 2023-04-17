using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoolUI : ResourceUI
{
    private Wool _wool = new Wool();
    
    public override string GetName()
    {
        return _wool.GetName();
    }

    public override List<string> GetCraftedResources()
    {
        return null;
    }

    public override int GetPrice()
    {
        return _wool.GetPrice();
    }
}
