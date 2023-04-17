using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using TMPro;
using DeviceType = Agava.YandexGames.DeviceType;

public class Ads : MonoBehaviour
{
    [SerializeField] private TMP_Text _authorizationStatusText;
    [SerializeField] private TMP_Text _personalProfileDataPermissionStatusText;
    [SerializeField] private UltimateJoystick _joystick;
    [SerializeField] private WatchAdNotification _adNotification;

    private void OnEnable()
    {
        _adNotification.WatchAdButtonClicked += OnWatchAdButtonClicked;
    }

    private void OnDisable()
    {
        _adNotification.WatchAdButtonClicked -= OnWatchAdButtonClicked;
    }

    private IEnumerator Start()
    {
        yield return YandexGamesSdk.Initialize();

        while (true)
        {
            _authorizationStatusText.color = PlayerAccount.IsAuthorized ? Color.green : Color.red;

            if (PlayerAccount.IsAuthorized)
                _personalProfileDataPermissionStatusText.color = PlayerAccount.HasPersonalProfileDataPermission ? Color.green : Color.red;
            else
                _personalProfileDataPermissionStatusText.color = Color.red;

            yield return new WaitForSecondsRealtime(0.25f);
        }
    }

    private void OnWatchAdButtonClicked()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
// Код только для WebGL билда
        VideoAd.Show();
#endif
        Time.timeScale = 0;
        //VideoAd.Show();
        Time.timeScale = 1;
        _adNotification.Close();
    }
}
