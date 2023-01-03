using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Animal
{
    
    
    private void Awake()
    {
        _level = 1;
        _isAbleToCatch = true;
    }
}
