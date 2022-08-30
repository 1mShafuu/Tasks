using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using DG.Tweening;

public class ColorChange : MonoBehaviour
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

    public void SetColorForPlayButton()
    {
        _image.color = Color.blue;
    }

    public void ChangeColorForAuthorsButton()
    {
        _image.DOColor(_to, 2f).SetLoops(-1, LoopType.Yoyo);
    }

    public void RotateForExitButton()
    {
        _image.DOFade(0f, 2f);
    }

}
