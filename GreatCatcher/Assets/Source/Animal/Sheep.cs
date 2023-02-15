using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Animal
{
    private void Awake()
    {
        Level = 1;
        SellCost = 100;
    }
}
