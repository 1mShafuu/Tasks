using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] protected int _level;
    [SerializeField] protected bool _isAbleToCatch;

    public int Level => _level;
    public bool IsAbleToCatch => _isAbleToCatch;
}
