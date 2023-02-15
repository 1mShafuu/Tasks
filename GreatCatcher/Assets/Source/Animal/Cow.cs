using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : Animal
{
    private void Awake()
    {
        Level = 2;
        SellCost = 1000;
    }
}
