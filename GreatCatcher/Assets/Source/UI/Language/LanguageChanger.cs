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
    [SerializeField] private Ads _ads;

    public event Action LanguageChanged;

    private void OnEnable()
    {
        _englishLanguageButton.onClick.AddListener(OnEnglishLanguageButtonClicked);
        _russianLanguageButton.onClick.AddListener(OnRussianLanguageButtonClicked);
        _turkishLanguageButton.onClick.AddListener(OnTurkishLanguageButtonClicked);
        _ads.LanguageReceived += OnLanguageReceived;
    }
    
    private void OnDisable()
    {
        _englishLanguageButton.onClick.RemoveListener(OnEnglishLanguageButtonClicked);
        _russianLanguageButton.onClick.RemoveListener(OnRussianLanguageButtonClicked);
        _turkishLanguageButton.onClick.RemoveListener(OnTurkishLanguageButtonClicked);
        _ads.LanguageReceived -= OnLanguageReceived;
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

    private void OnLanguageReceived(string language)
    {
        switch (language)
        {
            case "en":
                Lean.Localization.LeanLocalization.SetCurrentLanguageAll("English");
                break;
            case "ru":
                Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Russian");
                break;
            case "tr":
                Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Arabic");
                break;
            default:
                Lean.Localization.LeanLocalization.SetCurrentLanguageAll("English");
                break;
        }
        
        ResourcesTranslations.InitTranslations();
        LanguageChanged?.Invoke();
    }
}
