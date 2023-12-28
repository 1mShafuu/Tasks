using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookieUI : ResourceUI
{
    private Cookie _cookie = new Cookie();
    
    public override string GetName()
    {
        return _cookie.GetName();
    }

    public override List<string> GetCraftedResources()
    {
        return GetRequiredResourcesList(_cookie);
    }

    public override int GetPrice()
    {
        return _cookie.GetPrice();
    }
}
