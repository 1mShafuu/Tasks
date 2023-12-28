using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Animal, ICatchable
{
    private void Awake()
    {
        Level = 1;
        SellCost = 100;
    }

    public override Resource GetResource()
    {
        return new Wool();
    }
    
}
