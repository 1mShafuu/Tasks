using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : Resource
{
    public override string GetName()
    {
        return "Meat";
    }

    public override int GetAmount()
    {
        return 1;
    }

    public override int GetPrice()
    {
        return 800;
    }
}
