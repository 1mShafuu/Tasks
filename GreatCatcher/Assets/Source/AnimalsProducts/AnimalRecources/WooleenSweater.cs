using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoolenSweater : CraftableItem
{
    public override string GetName()
    {
        return "Woolen Sweater";
    }

    public override int GetAmount()
    {
        return 1;
    }

    public override int GetPrice()
    {
        return 500;
    }

    public override Resource[] GetRequiredResources()
    {
        return new Resource[] { new Wool(), new Wool()};
    }
}