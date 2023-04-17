using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftButton : OpenMenuButton
{
    [SerializeField] private CraftingMenu _menu;
    
    protected override void OnButtonClicked()
    {
        _menu.Open();
    }
}
