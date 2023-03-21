using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCraft : MonoBehaviour
{
    [SerializeField] private Crafting _crafting;

    private void OnTriggerEnter(Collider other)
    {
        _crafting.CraftWoolSweater();
    }
}
