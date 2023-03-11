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
}
