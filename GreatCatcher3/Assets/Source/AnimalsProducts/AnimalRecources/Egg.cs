using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : Resource
{
    public override string GetName()
    {
        return "Egg";
    }

    public override int GetAmount()
    {
        return 1;
    }

    public override int GetPrice()
    {
        return 20000;
    }
}
