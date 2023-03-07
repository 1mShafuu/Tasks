using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Animal
{
    private void Awake()
    {
        Level = 1;
        SellCost = 50000;
    }
}
