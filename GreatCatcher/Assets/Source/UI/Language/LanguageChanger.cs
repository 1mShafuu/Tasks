using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageChanger : MonoBehaviour
{
    [SerializeField] private Button _englishLanguageButton;
    [SerializeField] private Button _russianLanguageButton;
    [SerializeField] private Button _turkishLanguageButton;

    public event Action LanguageChanged;

    private void OnEnable()
    {
        _englishLanguageButton.onClick.AddListener(OnEnglishLanguageButtonClicked);
        _russianLanguageButton.onClick.AddListener(OnRussianLanguageButtonClicked);
        _turkishLanguageButton.onClick.AddListener(OnTurkishLanguageButtonClicked);
    }
    
    private void OnDisable()
    {
        _englishLanguageButton.onClick.RemoveListener(OnEnglishLanguageButtonClicked);
        _russianLanguageButton.onClick.RemoveListener(OnRussianLanguageButtonClicked);
        _turkishLanguageButton.onClick.RemoveListener(OnTurkishLanguageButtonClicked);
    }

    private void Start()
    {
        
    }

    private void OnEnglishLanguageButtonClicked()
    {
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll("English");
        ResourcesTranslations.InitTranslations();
        LanguageChanged?.Invoke();
    }

    private void OnRussianLanguageButtonClicked()
    {
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Russian");
        ResourcesTranslations.InitTranslations();
        LanguageChanged?.Invoke();
    }

    private void OnTurkishLanguageButtonClicked()
    {
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Arabic");
        ResourcesTranslations.InitTranslations();
        LanguageChanged?.Invoke();
    }
}
