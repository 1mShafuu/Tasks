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
    private float _rotationSpeed = 1000f;
    
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
         var rotaionGoal = Quaternion.LookRotation(transform.parent.position - Camera.main.transform.position);
         transform.rotation =Quaternion.Lerp(transform.rotation, rotaionGoal,_rotationSpeed * Time.deltaTime);
    }
    
    private void OnAnimalsAmountChanged(int value)
    {
        _text.text = value.ToString();
    }
}
