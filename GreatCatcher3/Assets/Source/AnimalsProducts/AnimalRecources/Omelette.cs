using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omelette : CraftableItem
{
    public override string GetName()
    {
        return "Omelette";
    }

    public override int GetAmount()
    {
        return 1;
    }

    public override int GetPrice()
    {
        return 30000;
    }

    public override Resource[] GetRequiredResources()
    {
        return new Resource[] { new Egg(), new Milk(), new Meat() };
    }
}
