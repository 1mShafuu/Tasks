using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using DG.Tweening;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _from;
    [SerializeField] private Color _to;

    [ContextMenu("Load color")]
    private void LoadColor()
    {
        if (_image != null)
        {
            _from = _image.color;
        }
    }

    public void SetBlueColor()
    {
        _image.color = Color.blue;
    }

    public void SetGradientColor()
    {
        var duration = 2f;
        _image.DOColor(_to, duration).SetLoops(-1, LoopType.Yoyo);
    }
}
