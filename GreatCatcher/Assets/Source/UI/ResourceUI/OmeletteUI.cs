using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class OmeletteUI : ResourceUI
{
    private Omelette _omelette = new Omelette();

    public override string GetName()
    {
        return _omelette.GetName();
    }

    public override List<string> GetCraftedResources()
    {
         return GetRequiredResourcesList(_omelette);
    }

    public override int GetPrice()
    {
        return _omelette.GetPrice();
    }
}
