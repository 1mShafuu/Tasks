using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ResourceView : MonoBehaviour
{
    [SerializeField] protected TMP_Text Label;
    [SerializeField] protected Image Icon;
    [SerializeField] protected Button Button;
    [SerializeField] protected TMP_Text RequiredResources;

    protected ResourceUI ResourceUIElement;

    protected virtual void OnEnable()
    {
        Button.onClick.AddListener(OnButtonClicked);
    }

    protected virtual void OnDisable()
    {
        Button.onClick.RemoveListener(OnButtonClicked);
    }

    public abstract void Render(ResourceUI resource, Crafting crafting = null);

    protected abstract void OnButtonClicked();
}
