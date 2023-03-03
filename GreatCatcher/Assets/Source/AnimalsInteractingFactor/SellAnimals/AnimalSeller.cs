using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalSeller : MonoBehaviour
{
    [SerializeField] private GameObject _sellAreaGameObject;
    [SerializeField] private Player _player;

    private Wallet _wallet;
    private SellArea _sellArea;
    
    private void Start()
    {
        _wallet = _player.GetComponent<Wallet>();
        _sellArea = _sellAreaGameObject.GetComponent<SellArea>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var animalsToSell = _sellArea.Animals.ToList();
        
        if (other.TryGetComponent(out Player player) && animalsToSell.Count > 0)
        {
            SellAnimals(animalsToSell);
        }
        
        _sellArea.ClearDeletedAnimals();
    }

    private void SellAnimals(List<GameObject> animals)
    {
        int saleAmount = 0;
        
        foreach (var animalGameObject in animals)
        {
            animalGameObject.TryGetComponent(out Animal animal);
            saleAmount += animal.SellCost;
            Destroy(animalGameObject);
        }
        
        _wallet.ChangeMoney(saleAmount);
    }
    
}
