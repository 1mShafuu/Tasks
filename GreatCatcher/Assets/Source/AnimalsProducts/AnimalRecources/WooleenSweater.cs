using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoolenSweater : CraftableItem
{
    public WoolenSweater() : base(1)
    {
    }

    public override string GetName()
    {
        return "WoolenSweater";
    }

    public override int GetAmount()
    {
        return 1;
    }

    public override Resource[] GetRequiredResources()
    {
        return new Resource[] { new Wool() };
    }
}