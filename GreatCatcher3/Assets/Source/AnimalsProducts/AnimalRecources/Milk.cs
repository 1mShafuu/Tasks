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
        return 1;
    }

    public override int GetPrice()
    {
        return 300;
    }
}
