using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource
{
    public abstract string GetName();

    public abstract int GetAmount();

    public abstract int GetPrice();
}
