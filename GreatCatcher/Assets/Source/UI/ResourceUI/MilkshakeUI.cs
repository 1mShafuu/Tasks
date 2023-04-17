using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilkshakeUI : ResourceUI
{
    private MilkShake _milkShake = new MilkShake();
    
    public override string GetName()
    {
        return _milkShake.GetName();
    }

    public override List<string> GetCraftedResources()
    {
        return GetRequiredResourcesList(_milkShake);
    }

    public override int GetPrice()
    {
        return _milkShake.GetPrice();
    }
}
