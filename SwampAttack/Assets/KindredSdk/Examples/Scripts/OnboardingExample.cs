using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnboardingExample : MonoBehaviour
{
    public GameObject iOSOnboardingPanel;
    public GameObject AndroidOnboardingPanel;
    public Button ActivateButton;
    public Button EnableButton;
    public Button AndroidCloseButton;
    public Button iOSCloseButton;
    public Button ShowPopupButton;
    public Button HomeButton;

    const string KINDRED_SCENE = "KindredExample";

    private void Start()
    {
        ActivateButton.onClick.AddListener(ActivateClicked);
        EnableButton.onClick.AddListener(EnableClicked);
        AndroidCloseButton.onClick.AddListener(HideOnboarding);
        iOSCloseButton.onClick.AddListener(HideOnboarding);
        ShowPopupButton.onClick.AddListener(ShowOnboarding);
        HomeButton.onClick.AddListener(SwitchToKindredExample);

        AndroidOnboardingPanel.SetActive(false);
        iOSOnboardingPanel.SetActive(false);
        ShowOnboarding(); 
    }

    private void SwitchToKindredExample()
    {
        SceneManager.LoadScene(KINDRED_SCENE, LoadSceneMode.Single);
    }

    private void EnableClicked()
    {
        KindredSdkBridge.ShowAccessibilitySettings();
    }

    private void ShowOnboarding()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidOnboardingPanel.SetActive(true);
        }
        else
        {
            iOSOnboardingPanel.SetActive(true);
        }
    }

    private void HideOnboarding()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidOnboardingPanel.SetActive(false);
        }
        else
        {
            iOSOnboardingPanel.SetActive(false);
        }
    }

    private void ActivateClicked()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Application.OpenURL("https://sdk.kindred.co/keyboard-activation");
        }
        else
        {
            var productName = Application.productName.ToLower().Replace(" ", string.Empty);
            Application.OpenURL($"https://sdk.kindred.co/plugin-activation?origin={productName}&utm_campaign={productName}");
        }
    }
}
