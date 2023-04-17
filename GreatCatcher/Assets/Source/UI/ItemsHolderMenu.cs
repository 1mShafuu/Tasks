using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemsHolderMenu : MonoBehaviour
{
    [SerializeField] protected ResourceView Template;
    [SerializeField] protected GameObject ItemContainer;
    [SerializeField] protected ResourceUI[] PossibleToCraftResources;

    private void Start()
    {
        for (int index = 0; index < PossibleToCraftResources.Length; index++)
        {
            AddItem(PossibleToCraftResources[index]);
        }
    }

    protected abstract void AddItem(ResourceUI resource);
}
