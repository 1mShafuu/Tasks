#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using UnityEngine;

public static class KindredSdkBridge
{
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SetKindredUserId(string userId);
    [DllImport("__Internal")]
    private static extern void SetKindredUserCountry(string userCountry);
    [DllImport("__Internal")]
    private static extern void SetKindredAppScheme(string appScheme);
    [DllImport("__Internal")]
    private static extern void ShowKindredSettings();
#endif

    /// <summary>
    /// Set your user ID.
    /// </summary>
    /// <param name="userId">Your user ID</param>
    public static void SetUserId(string userId)
    {
#if !UNITY_EDITOR
#if UNITY_ANDROID
        using (AndroidJavaClass keyboardService = new AndroidJavaClass("com.unity3d.player.KindredSdkBridge"))
        {
            AndroidJavaClass playerClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivityObject = playerClass.GetStatic<AndroidJavaObject> ("currentActivity");
            keyboardService.CallStatic("setUserId", userId, currentActivityObject);
        }
#elif UNITY_IOS
        SetKindredUserId(userId);
#endif
#endif
    }

    /// <summary>
    /// Set your country.
    /// </summary>
    /// <param name="userCountry">Your country</param>
    public static void SetUserCountry(string userCountry)
    {
#if !UNITY_EDITOR
#if UNITY_ANDROID
        using (AndroidJavaClass keyboardService = new AndroidJavaClass("com.unity3d.player.KindredSdkBridge"))
        {
            AndroidJavaClass playerClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivityObject = playerClass.GetStatic<AndroidJavaObject> ("currentActivity");
            keyboardService.CallStatic("setUserCountry", userCountry, currentActivityObject);
        }
#elif UNITY_IOS
        SetKindredUserCountry(userCountry);
#endif
#endif
    }

    /// <summary>
    /// Set application url scheme.
    /// </summary>
    /// <param name="urlScheme">Url scheme</param>
    public static void SetAppUrlScheme(string appScheme)
    {
#if !UNITY_EDITOR
#if UNITY_IOS
        SetKindredAppScheme(appScheme);
#endif
#endif
    }

    /// <summary>
    /// Show Kindred settings.
    /// </summary>
    public static void ShowAppSettings()
    {
#if !UNITY_EDITOR
#if UNITY_ANDROID
        using (AndroidJavaClass kindredService = new AndroidJavaClass("com.unity3d.player.KindredSdkBridge"))
        {
            AndroidJavaClass playerClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivityObject = playerClass.GetStatic<AndroidJavaObject> ("currentActivity");
            kindredService.CallStatic("showKindredSettings", currentActivityObject);
        }
#elif UNITY_IOS
        ShowKindredSettings();
#endif
#endif
    }

    /// <summary>
    /// Show Accessibility settings.
    /// </summary>
    public static void ShowAccessibilitySettings()
    {
#if !UNITY_EDITOR
#if UNITY_ANDROID
        using (AndroidJavaClass kindredService = new AndroidJavaClass("com.unity3d.player.KindredSdkBridge"))
        {
            AndroidJavaClass playerClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivityObject = playerClass.GetStatic<AndroidJavaObject> ("currentActivity");
            kindredService.CallStatic("showAccessibilitySettings", currentActivityObject);
        }
#endif
#endif
    }
}
