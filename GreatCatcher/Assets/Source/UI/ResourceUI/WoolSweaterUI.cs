using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoolSweaterUI : ResourceUI
{
    private  WoolenSweater _woolenSweater = new WoolenSweater();
    
    public override string GetName()
    {
        return _woolenSweater.GetName();
    }

    public override List<string> GetCraftedResources()
    {
        return GetRequiredResourcesList(_woolenSweater);
    }

    public override int GetPrice()
    {
        return _woolenSweater.GetPrice();
    }
}
