using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceGainer : MonoBehaviour
{
    [SerializeField] private SellArea _sellArea;
    [SerializeField] private Storage _storage;

    private List<Animal> _ownedAnimals = new List<Animal>();

    private void Start()
    {
        StartCoroutine(ResourceGenerator());
    }

    private IEnumerator ResourceGenerator()
    {
        int secondsInOneMinute = 60;
        var waitForSeconds = new WaitForSeconds(secondsInOneMinute);
        
        while (true)
        {
            yield return waitForSeconds;
            
            _ownedAnimals.Clear();
            
            foreach (var sellAreaAnimal in _sellArea.Animals)
            {
                var ownedAnimal = sellAreaAnimal.gameObject.GetComponent<Animal>();
                _ownedAnimals.Add(ownedAnimal);
            }
            
            foreach (var ownedAnimal in _ownedAnimals)
            {
                //Debug.Log("ResourceGainer");
                _storage.Store(ownedAnimal.GetResource(), ownedAnimal.GetResource().GetAmount());
            }
        
            _storage.ShowResources();
        }
    }
}

