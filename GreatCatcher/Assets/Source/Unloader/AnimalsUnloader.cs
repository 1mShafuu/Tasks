using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsUnloader : MonoBehaviour
{
    [SerializeField] private GameObject[] _yards;

    private Bag _bag;
    
    public event Action AnimalUnloaded;
    
    private void Awake()
    {
        _bag = GetComponent<Bag>();
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
                var activeYard = yard.transform;
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
