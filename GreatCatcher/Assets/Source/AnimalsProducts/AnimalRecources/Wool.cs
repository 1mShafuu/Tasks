using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wool : Resource
{
    public override string GetName()
    {
        return "Wool";
    }

    public override int GetAmount()
    {
        return 3;
    }
}
