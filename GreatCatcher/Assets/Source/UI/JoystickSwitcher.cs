using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class JoystickSwitcher : MonoBehaviour
{
    [SerializeField] private Toggle _switchInputToggle;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Color _backgroundActiveColor;
    [SerializeField] private Color _handleActiveColor;
    
    private Vector2 _handlePosition;
    private Color _backgroundDefaultColor;
    private Color _handleDefaultColor;
    private Image _backgroundImage;
    private Image _handleImage;
    
    public bool JoystickChosen { get; private set; } = false;

    public event Action ModeSwitched;
    
    private void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (Agava.WebUtility.Device.IsMobile)
        {
            _switchInputToggle.gameObject.SetActive(false);
        }
#endif
        _handlePosition = _rectTransform.anchoredPosition;
        _backgroundImage = _rectTransform.parent.GetComponent<Image>();
        _handleImage = _rectTransform.GetComponent<Image>();
        _backgroundDefaultColor = _backgroundImage.color;
        _handleDefaultColor = _handleImage.color;
        
        if (_switchInputToggle.isOn)
        {
            OnButtonClicked(true);
        }
    }

    private void OnEnable()
    {
        _switchInputToggle.onValueChanged.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _switchInputToggle.onValueChanged.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked(bool on)
    {
        _rectTransform.DOAnchorPos(on ? _handlePosition * -1 : _handlePosition, 0.2f)
            .SetUpdate(UpdateType.Normal, true)
            .SetEase((DG.Tweening.Ease.InOutQuad));
        _backgroundImage.DOColor(on ? _backgroundActiveColor : _backgroundDefaultColor, 0.6f)
            .SetUpdate(UpdateType.Normal, true);
        _handleImage.DOColor(on ? _handleActiveColor : _handleDefaultColor, 0.4f)
            .SetUpdate(UpdateType.Normal, true);
        JoystickChosen = on;
    }
}
