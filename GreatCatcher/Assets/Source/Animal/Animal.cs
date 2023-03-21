using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    public int Level { get; protected set; }
    public int SellCost { get; protected set; }
    public abstract Resource GetResource();
}

