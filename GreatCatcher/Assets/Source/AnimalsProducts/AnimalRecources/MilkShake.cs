using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkShake : CraftableItem
{
    public override string GetName()
    {
        return "MilkShake";
    }

    public override int GetAmount()
    {
        return 1;
    }

    public override int GetPrice()
    {
        return 25000;
    }

    public override Resource[] GetRequiredResources()
    {
        return new Resource[] { new Milk(), new Egg() };
    }
}
