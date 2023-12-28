using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemsHolderMenu : MonoBehaviour
{
    [SerializeField] protected ResourceView Template;
    [SerializeField] protected GameObject ItemContainer;
    [SerializeField] protected ResourceUI[] PossibleToCraftResources;
    [SerializeField] private LanguageChanger _languageChanger;
    
    private void OnEnable()
    {
        _languageChanger.LanguageChanged += OnLanguageChanged;
    }

    private void OnDisable()
    {
        _languageChanger.LanguageChanged -= OnLanguageChanged;
    }

    private void Start()
    {
        AddItems();
    }

    protected abstract void AddItem(ResourceUI resource);

    private void OnLanguageChanged()
    {
        AddItems();
    }

    private void AddItems()
    {
        ClearItemHolder();
        
        for (int index = 0; index < PossibleToCraftResources.Length; index++)
        {
            AddItem(PossibleToCraftResources[index]);
        }
    }
    
    private void ClearItemHolder()
    {
        if (ItemContainer.transform.childCount != 0)
        {
            for (int index = 0; index < ItemContainer.transform.childCount; index++)
            {
                Transform childTransform = ItemContainer.transform.GetChild(index);
                Destroy(childTransform.gameObject);
            }
        }
    }
}
