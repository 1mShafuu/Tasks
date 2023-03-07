using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalSeller : MonoBehaviour
{
    [SerializeField] private GameObject _sellAreaGameObject;
    [SerializeField] private Player _player;
    [SerializeField] private UpgradeYardButton _yardUpgrader;
    
    private Wallet _wallet;
    private SellArea _sellArea;
    private bool _isAbleToSell = false;
    private float _animalSelloutPriceModifier = 2;
    
    private void Awake()
    {
        _wallet = _player.GetComponent<Wallet>();
        _sellArea = _sellAreaGameObject.GetComponent<SellArea>();
    }

    private void OnEnable()
    {
        _sellArea.AnimalsLimitReached += OnAnimalsLimitReached;
        _yardUpgrader.YardUpgraded += OnYardUpgraded;
    }

    private void OnDisable()
    {
        _sellArea.AnimalsLimitReached -= OnAnimalsLimitReached;
        _yardUpgrader.YardUpgraded -= OnYardUpgraded;
    }

    private void OnTriggerEnter(Collider other)
    {
        var animalsToSell = _sellArea.Animals.ToList();
        
        if (other.TryGetComponent(out Player player) && animalsToSell.Count > 0 && _isAbleToSell)
        {
            SellAnimals(animalsToSell);
        }
        
        _sellArea.ClearDeletedAnimals();
        _isAbleToSell = false;
    }

    private void SellAnimals(List<GameObject> animals)
    {
        int saleAmount = 0;
        
        foreach (var animalGameObject in animals)
        {
            animalGameObject.TryGetComponent(out Animal animal);
            saleAmount += Convert.ToInt32(animal.SellCost * _animalSelloutPriceModifier);
            Destroy(animalGameObject);
        }
        
        _wallet.ChangeMoney(saleAmount);
    }

    private void OnAnimalsLimitReached()
    {
        _isAbleToSell = true;
    }

    private void OnYardUpgraded()
    {
        _animalSelloutPriceModifier *= 1.8f;
    }
}
