using System;
using System.Collections;
using UnityEngine;
using Agava.YandexGames;
using Agava.WebUtility;
using UnityEngine.UI;

public class Ads : MonoBehaviour
{
    [SerializeField] private Image _authorizationStatusImage;
    [SerializeField] private Image _personalProfileDataPermissionStatusImage;
    [SerializeField] private WatchAdNotification _adNotification;

    public event Action<string> LanguageReceived;
    private Action _videoClosed;

    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private void OnEnable()
    {
        StartCoroutine(InitSDK());
        WebApplication.InBackgroundChangeEvent += OnOnInBackgroundChanged;
        _adNotification.WatchAdButtonClicked += OnWatchAdButtonClicked;
        _videoClosed += OnVideoClosed;
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnOnInBackgroundChanged;
        _adNotification.WatchAdButtonClicked -= OnWatchAdButtonClicked;
        _videoClosed -= OnVideoClosed;
    }

    private IEnumerator InitSDK()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        yield return YandexGamesSdk.Initialize();

        LanguageReceived?.Invoke(YandexGamesSdk.Environment.i18n.lang);
        
        while (true)
        {
            _authorizationStatusImage.color = PlayerAccount.IsAuthorized ? Color.green : Color.red;

            if (PlayerAccount.IsAuthorized)
            {
                _personalProfileDataPermissionStatusImage.color =
                    PlayerAccount.HasPersonalProfileDataPermission ? Color.green : Color.red;
            }
            else
            {
                _personalProfileDataPermissionStatusImage.color= Color.red;
            }

            yield return new WaitForSecondsRealtime(0.25f);
        }
#else
        yield return null;
#endif
    }
    
    private void OnWatchAdButtonClicked()
    {
        Time.timeScale = 0;
#if UNITY_WEBGL && !UNITY_EDITOR
        VideoAd.Show(null, null , _videoClosed);
#endif
    }

    private void OnVideoClosed()
    {
        _adNotification.Close();
    }

    private void OnOnInBackgroundChanged(bool inBackground)
    {
        //AudioListener.pause = inBackground;
        //AudioListener.volume = inBackground ? 0f : 1f;
        Time.timeScale =  inBackground ? 0 : 1;
    }
}
