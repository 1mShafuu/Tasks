using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Animal : MonoBehaviour, ICatchable
{
    public int Level { get; protected set; }
    public int SellCost { get; protected set; }
    public bool IsCaught { get; private set; } = false;
    
    public abstract Resource GetResource();

    public void Catch()
    {
        IsCaught = true;
    }

    public void ResetCatchStatus()
    {
        IsCaught = false;
    }
}

