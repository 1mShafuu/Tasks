using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BagUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private Bag _bag;
    private Canvas _canvas;
    
    private void Awake()
    {
        _bag = GetComponentInParent<Bag>();
        _canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        _bag.AnimalsAmountChanged += OnAnimalsAmountChanged;
    }

    private void OnDisable()
    {
        _bag.AnimalsAmountChanged -= OnAnimalsAmountChanged;
    }

    private void Update()
    {
        var rotaionGoal = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        transform.rotation =Quaternion.Slerp(transform.rotation, rotaionGoal,15f);
    }
    
    private void OnAnimalsAmountChanged(int value)
    {
        _text.text = value.ToString();
    }
}
