using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace KindredSDK.Editor
{
    public class KindredApi
    {
        public static UnityWebRequest PostRegister(string name, string email, string password)
        {
            var url = "https://sdk-portal.kindred.co/api/auth/register";
            var www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            www.SetRequestHeader("Content-Type", "application/json");
            var registerBody = new RegisterRequest
            {
                name = name,
                Email = email,
                password = password
            };
            var loginBodyText = JsonUtility.ToJson(registerBody);
            var jsonBytes = Encoding.UTF8.GetBytes(loginBodyText);
            www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            return www;
        }

        public static UnityWebRequest PostLogin(string email, string password)
        {
            var url = "https://sdk-portal.kindred.co/api/auth/login";
            var www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            www.SetRequestHeader("Content-Type", "application/json");
            var loginBody = new LoginRequest
            {
                Email = email,
                password = password
            };
            var loginBodyText = JsonUtility.ToJson(loginBody);
            var jsonBytes = Encoding.UTF8.GetBytes(loginBodyText);
            www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            return www;
        }

        public static UnityWebRequest GetCredentials(string token)
        {
            var www = new UnityWebRequest
            {
                url = "https://sdk-portal.kindred.co/api/client/credentials",
                method = UnityWebRequest.kHttpVerbGET,
                downloadHandler = new DownloadHandlerBuffer()
            };
            www.SetRequestHeader("Authorization", token);
            return www;
        }

        public static UnityWebRequest PostGenerateCredentials(string token)
        {
            var url = "https://sdk-portal.kindred.co/api/client/credentials";
            var www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            www.SetRequestHeader("Content-Type", "application/json");
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", token);
            return www;
        }

        public static UnityWebRequest PostResetCredentials(string token)
        {
            var url = "https://sdk-portal.kindred.co/api/client/credentials/reset";
            var www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            www.SetRequestHeader("Content-Type", "application/json");
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", token);
            return www;
        }
    }

    [Serializable]
    public class LoginRequest
    {
        public string Email;
        public string password;
    }

    [Serializable]
    public class RegisterRequest
    {
        public string name;
        public string Email;
        public string password;
    }

    [Serializable]
    public class GenerateCredentialsResponse
    {
        public string clientId;
        public string clientSecret;
        public string clientSecondarySecret;
        public string sharedKey;
        public int authClientType;
        public bool showSecret;
        public bool showSecondarySecret;
    }

    [Serializable]
    public class GetCredentialsResponse
    {
        public string clientId;
        public string clientSecret;
        public string clientSecondarySecret;
        public string sharedKey;
        public int authClientType;
        public bool showSecret;
        public bool showSecondarySecret;
    }
}