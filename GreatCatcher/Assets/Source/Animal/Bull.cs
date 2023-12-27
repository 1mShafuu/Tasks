using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bull : Animal, ICatchable
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
