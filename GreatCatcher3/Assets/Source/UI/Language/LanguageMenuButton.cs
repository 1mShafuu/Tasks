using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageMenuButton : OpenMenuButton
{
    [SerializeField] private LanguageChangerMenu _languageChangerMenu;
    
    protected override void OnButtonClicked()
    {
        _languageChangerMenu.Open();
    }
}
