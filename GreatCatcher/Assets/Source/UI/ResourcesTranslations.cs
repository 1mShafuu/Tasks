using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lean.Localization;
using UnityEngine;

public static class ResourcesTranslations
{
    private static Dictionary<string, string> _resourcesTranslations =
        new Dictionary<string, string>();

    public static IReadOnlyDictionary<string, string> ResourcesTranslationsDictionary => _resourcesTranslations;
    
    public static void InitTranslations()
    {
        var previousLanguage = LeanLocalization.GetFirstCurrentLanguage();
        var resourcesPreviousLanguage = LeanLocalization.GetTranslationText("TranslationResources").Split(",").ToList();
        
        LeanLocalization.SetCurrentLanguageAll("English");
        var resourcesEnglish = LeanLocalization.GetTranslationText("TranslationResources").Split(",").ToList();
        
        for (int index = 0; index < resourcesPreviousLanguage.Count; index++)
        {
            _resourcesTranslations.Add(resourcesEnglish[index],resourcesPreviousLanguage[index]);
        }
        
        LeanLocalization.SetCurrentLanguageAll(previousLanguage);
    }
}