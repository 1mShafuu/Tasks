using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(BoxCollider))]
public class LosingZone : MonoBehaviour
{
    private BoxCollider _boxCollider;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        _rigidbody.isKinematic = true;
    }
}
