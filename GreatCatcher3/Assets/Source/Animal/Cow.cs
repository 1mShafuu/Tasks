using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : Animal, ICatchable
{
    private void Awake()
    {
        Level = 2;
        SellCost = 1000;
    }

    public override Resource GetResource()
    {
        return new Milk();
    }
}
