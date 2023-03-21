using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bull : Animal
{
    private void Awake()
    {
        Level = 3;
        SellCost = 5000;
    }

    public override Resource GetResource()
    {
        return new Meat();
    }
}
