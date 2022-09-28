using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KindredExample : MonoBehaviour
{
    public InputField UserIdField;
    public InputField UserCountryField;

    public Button SetUserIdBtn;
    public Button SetUserCountryBtn;
    public Button ShowSettingsBtn;
    public Button SwitchSceneBtn;

    public Text UserIdFeedback;
    public Text UserCountryFeedback;

    const string ONBOARDING_SCENE = "OnboardingExample";

    private void Start()
    {
        SetUserIdBtn.onClick.AddListener(SetUserId);
        SetUserCountryBtn.onClick.AddListener(SetUserCountry);
        ShowSettingsBtn.onClick.AddListener(ShowKeyboardSettings);
        SwitchSceneBtn.onClick.AddListener(SwitchScene);

        UserIdField.onValueChanged.AddListener(UserIdTextChanged);
        UserCountryField.onValueChanged.AddListener(UserCountryTextChanged);
    }

    private void SwitchScene()
    {
        SceneManager.LoadScene(ONBOARDING_SCENE, LoadSceneMode.Single);
    }

    private void UserCountryTextChanged(string arg0)
    {
        UserCountryFeedback.text = "";
    }

    private void UserIdTextChanged(string arg0)
    {
        UserIdFeedback.text = "";
    }

    public void SetUserId()
    {
        KindredSdkBridge.SetUserId(UserIdField.text);
        UserIdFeedback.text = "The user ID has been successfully set";
    }

    public void SetUserCountry()
    {
        KindredSdkBridge.SetUserCountry(UserCountryField.text);
        UserCountryFeedback.text = "The user country has been successfully set";
    }

    public void ShowKeyboardSettings()
    {
        KindredSdkBridge.ShowAppSettings();
    }
}
