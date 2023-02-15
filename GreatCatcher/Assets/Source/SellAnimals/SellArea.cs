using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellArea : MonoBehaviour
{
    [SerializeField] private int _level;
    
    private List<GameObject> _animals = new List<GameObject>();

    public IReadOnlyList<GameObject> Animals => _animals;
    public int Level => _level;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Animal animal))
        {
            _animals.Add(animal.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Animal animal))
        {
            _animals.Remove(animal.gameObject);
        }
    }
}
