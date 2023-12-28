using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Animal, ICatchable
{
    private void Awake()
    {
        Level = 1;
        SellCost = 50000; 
    }

    public override Resource GetResource()
    {
        return new Egg();
    }
}
