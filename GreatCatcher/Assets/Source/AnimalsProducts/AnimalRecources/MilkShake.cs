using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkShake : CraftableItem
{
    public MilkShake() : base(2)
    {
    }

    public override string GetName()
    {
        return "MilkShake";
    }

    public override int GetAmount()
    {
        return 1;
    }

    public override Resource[] GetRequiredResources()
    {
        return new Resource[] { new Milk(), new Egg() };
    }
}
