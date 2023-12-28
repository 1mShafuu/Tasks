using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingResourcesInfo : MonoBehaviour
{
    [SerializeField] private Storage _storage;
    [SerializeField] private TMP_Text _wool;
    [SerializeField] private TMP_Text _meat;
    [SerializeField] private TMP_Text _milk;
    [SerializeField] private TMP_Text _egg;

    private int _resourcesSummary = 0;

    public Storage Storage => _storage;
    
    private void OnEnable()
    {
        _storage.ResourcesChanged += OnResourcesChanged;
    }

    private void OnDisable()
    {
        _storage.ResourcesChanged -= OnResourcesChanged;
    }

    private void OnResourcesChanged(string resource, int value)
    {
        //Debug.Log(value.ToString());
        switch (resource)
        {
            case "Meat":
                _meat.text = AddResources(_meat, value);
                break;
            case "Wool":
                _wool.text = AddResources(_wool, value);
                break;
            case "Egg":
                _egg.text = AddResources(_egg, value);
                break;
            case "Milk":
                _milk.text = AddResources(_milk, value);
                break;
        }
    }

    private string AddResources(TMP_Text text, int value)
    {
        int previousValue = Convert.ToInt32(text.text);
        var newValue = value;
       // Debug.Log($"{previousValue}      {value}");
        return newValue.ToString();
    }
}
