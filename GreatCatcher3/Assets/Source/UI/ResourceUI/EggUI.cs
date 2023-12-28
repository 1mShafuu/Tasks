using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggUI : ResourceUI
{
    private Egg _egg = new Egg();
    
    public override string GetName()
    {
        return _egg.GetName();
    }

    public override List<string> GetCraftedResources()
    {
        return null;
    }

    public override int GetPrice()
    {
        return _egg.GetPrice();
    }
}
