using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsUnloader : MonoBehaviour
{
    [SerializeField] private GameObject[] _yards;
    [SerializeField] private SellArea _sellArea;
    
    private Bag _bag;
    private bool _isAbleToUnload = true;
    
    public event Action AnimalUnloaded;
    public event Action<int> YardChose;
    
    private void Awake()
    {
        _bag = GetComponent<Bag>();
    }

    private void OnEnable()
    {
        _sellArea.AnimalsLimitReached += OnAnimalsLimitReached;
        _sellArea.AnimalsLimitNotReached += OnAnimalsLimitNotReached;
    }

    private void OnDisable()
    {
        _sellArea.AnimalsLimitReached -= OnAnimalsLimitReached;
        _sellArea.AnimalsLimitNotReached -= OnAnimalsLimitNotReached;
    }

    public void Unload()
    {
        const int firstElement = 0;
        int amountAnimalsToUnload = _bag.AnimalsInBag;
        //Debug.Log(amountAnimalsToUnload);
        
        foreach (var yard in _yards)
        {
            if (yard.activeSelf)
            {
                if (_isAbleToUnload)
                {
                    var activeYard = yard.transform;
                    activeYard.TryGetComponent(out Yard currentYard);
                    YardChose?.Invoke(currentYard.Level);
                    Vector3 offset = new Vector3(-6, 3, 6);
                
                    for (int index = 0; index < amountAnimalsToUnload; index++)
                    { 
                        GameObject catched = Instantiate(_bag.CatchedAnimals[firstElement], activeYard.transform);
                        catched.transform.position = transform.position + offset;
                        catched.SetActive(true);
                        AnimalUnloaded?.Invoke();
                    }   
                }
            }
        }
    }

    private void OnAnimalsLimitReached()
    {
        _isAbleToUnload = false;
    }
    
    private void OnAnimalsLimitNotReached()
    {
        _isAbleToUnload = true;
    }
}
