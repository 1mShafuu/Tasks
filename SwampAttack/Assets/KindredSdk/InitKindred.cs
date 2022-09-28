using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitKindred : MonoBehaviour
{
    void Start()
    {
        InitializeKindred();
    }

    private void InitializeKindred()
    {
        var userId = SystemInfo.deviceUniqueIdentifier;
        KindredSdkBridge.SetUserId(userId);
        KindredSdkBridge.SetUserCountry("US");
        var urlScheme = Application.productName.ToLower().Replace(" ", string.Empty);
        KindredSdkBridge.SetAppUrlScheme(urlScheme);
    }
}
