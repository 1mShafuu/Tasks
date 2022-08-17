using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private UnityEvent<int> CoinAdded;
    
    private int _coinsAmount;

    public void AddCoin(int value)
    {
        if (value > 0)
        {
            _coinsAmount += value;
            CoinAdded.Invoke(_coinsAmount);
        }
    }

    public void AddListener(UnityAction<int> action)
    {
        CoinAdded.AddListener(action);
    }

    public void RemoveListener(UnityAction<int> action)
    {
        CoinAdded.RemoveListener(action);
    }
}

