using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : Resource
{
    public override string GetName()
    {
        return "Milk";
    }

    public override int GetAmount()
    {
        return 2;
    }

    public override int GetPrice()
    {
        return 300;
    }
}
