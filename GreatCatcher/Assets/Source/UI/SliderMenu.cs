using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderMenu : MonoBehaviour
{
    private Animator _animator;
    private bool _isOpen;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void ShowHideMenu()
    {
        _isOpen = _animator.GetBool("show");
        _animator.SetBool("show",!_isOpen);
    }
}
