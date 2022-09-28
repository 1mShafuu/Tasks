using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

namespace KindredSDK.Editor
{
#if UNITY_EDITOR

    public class KindredWizardWindow : EditorWindow
    {
        private PropertiesData _propertiesData;
        private WizardAction _actionAndroid;
        private WizardAction _actioniOS;
        private Texture2D _appIcon;
        private Vector2 _scrollMainPos;
        private string _pluginsAndroidPath;
        private bool _isUnderstandChecked;
        private Texture2D _darkGrayTexture;
        private Texture2D _alphaTexture;

        private bool _isClientIDValid;
        private bool _isClientSecretValid;
        private bool _isSharedKeyValid;

        private UnityWebRequest _www;
        private string _registerName;
        private string _registerEmail;
        private string _registerPassword;
        private string _loginEmail;
        private string _loginPassword;
        private string _wwwError;

        private event Action<string> _responseTextChanged;

        private const float LEFT_PANEL_WIDTH = 140;
        private const int KEY_LENGTH = 44;
        private const int MINIMUM_PASSWORD_LENGTH = 8;
        private const string KINDRED_DOCS_URL = "https://kindredsdkdocs.readme.io/docs/getting-started-1";


        readonly Color DARK_GRAY = new Color(38 / 255f, 38 / 255f, 38 / 255f);
        readonly string[] _extOptions = new string[] { "Plugin" };

        enum CredentialState
        {
            Welcome,
            Login,
            Register,
            EnterCredentials
        }

        CredentialState _credentialSectionState = CredentialState.Welcome;

        [MenuItem("Kindred/Configuration")]
        public static void CreateWindow()
        {
            GetWindow<KindredWizardWindow>("Kindred Setup Wizard");
        }

        // Add Example1 into a new menu list
        [MenuItem("Kindred/Setup Scene")]
        [MenuItem("GameObject/Kindred/Setup Scene")]
        public static void SetupScene()
        {
            //Give the path of the object to load and cache it in a variable
            var prefabPath = Path.Combine("Assets", "KindredSdk", "Prefabs", "InitKindred.prefab");
            var playerPrefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));

            //Instantiate prefab if it exists
            if (playerPrefab != null)
            {
                PrefabUtility.InstantiatePrefab(playerPrefab);
            }
        }

        private void OnEnable()
        {
            _propertiesData = new PropertiesData();

            _actionAndroid = new ConfigureAndroidAction(_propertiesData);
            _actioniOS = new ConfigureiOSAction(_propertiesData);

            _appIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(_propertiesData.APP_ICON);

            _pluginsAndroidPath = Path.Combine(Application.dataPath, "Plugins", "Android");

            PrepareTextures();
        }

        private void OnDisable()
        {
            RemoveTextures();
        }

        private void OnGUI()
        {
            DrawPropertyFields();
        }

        private void PrepareTextures()
        {
            _darkGrayTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            _darkGrayTexture.SetPixel(0, 0, DARK_GRAY);
            _darkGrayTexture.Apply();

            _alphaTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            _alphaTexture.SetPixel(0, 0, Color.clear);
            _alphaTexture.Apply();
        }

        private void RemoveTextures()
        {
            if (_darkGrayTexture != null)
            {
                DestroyImmediate(_darkGrayTexture);
                _darkGrayTexture = null;
            }

            if (_alphaTexture != null)
            {
                DestroyImmediate(_alphaTexture);
                _alphaTexture = null;
            }
        }

        private void DrawPropertyFields()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Space(2f);

            var headerLabel = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 13,
            };
            headerLabel.normal.textColor = Color.white;

            _scrollMainPos = EditorGUILayout.BeginScrollView(_scrollMainPos);
            EditorGUILayout.Space(15f);

            ShowMainTab(headerLabel);

            EditorGUILayout.EndScrollView();

            if (EditorGUI.EndChangeCheck())
            {
                _propertiesData.SaveToPrefs();
            }
        }

        private void ShowMainTab(GUIStyle headerLabel)
        {
            ShowCredentialsSection(headerLabel);

            // ShowAdvancedTab();

            EditorGUILayout.Space(15f);

            EditorGUILayout.LabelField("Settings", headerLabel);
            EditorGUILayout.Space(5f);

            // APP ICON
            var isAppIconValid = true;
            _appIcon = CustomTextureField("App Icon", _appIcon, "https://kindredsdkdocs.readme.io/docs/getting-started-1#app-icon");
            if (_appIcon != null)
            {
                _propertiesData.APP_ICON = AssetDatabase.GetAssetPath(_appIcon);
            }
            else
            {
                isAppIconValid = false;
                EditorGUILayout.HelpBox("App logo is not valid", MessageType.Error);
            }

            // ANDROID
            EditorGUILayout.LabelField(_actionAndroid.Name, headerLabel);
            EditorGUILayout.Space(5f);

            var warningLabel = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 12,

            };
            warningLabel.normal.textColor = Color.yellow;

#if !UNITY_ANDROID
            EditorGUILayout.LabelField("Please switch your platform to Android to prepare the Android build.", warningLabel);
            EditorGUI.BeginDisabledGroup(true);
#endif

#if !UNITY_2020_3_OR_NEWER
            EditorGUILayout.LabelField("Building for Android is only supported in Unity 2020.3 or newer.", warningLabel);
            EditorGUI.BeginDisabledGroup(true);
#endif

            EditorGUILayout.Space(1f);

            var isFormValid = isAppIconValid && _isClientIDValid &&
                _isClientSecretValid && _isSharedKeyValid;
            if (!isFormValid)
            {
                EditorGUI.BeginDisabledGroup(true);
            }

            if (GUILayout.Button("Prepare Android Build", GUILayout.Height(30)))
            {
                if ((File.Exists(Path.Combine(_pluginsAndroidPath, "AndroidManifest.xml")) ||
                    File.Exists(Path.Combine(_pluginsAndroidPath, "baseProjectTemplate.gradle")) ||
                    File.Exists(Path.Combine(_pluginsAndroidPath, "gradleTemplate.properties")) ||
                    File.Exists(Path.Combine(_pluginsAndroidPath, "mainTemplate.gradle"))))
                {
                    if (EditorUtility.DisplayDialog("Overwrite existing Android and Gradle templates?",
                    "Are you sure you want to overwrite existing Android and Gradle templates?",
                    "Backup Existing Ones, Then Overwrite", "Do Not Overwrite"))
                    {
                        _actionAndroid.Configure();
                    }
                }
                else
                {
                    _actionAndroid.Configure();
                }
            }

            if (!isFormValid)
            {
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.HelpBox("Configuration is not valid", MessageType.Error);
            }
#if !UNITY_ANDROID
            EditorGUI.EndDisabledGroup();
#endif

#if !UNITY_2020_3_OR_NEWER
            EditorGUI.EndDisabledGroup();
#endif

            EditorGUILayout.Space(20f);

            // iOS
            EditorGUILayout.LabelField(_actioniOS.Name, headerLabel);

#if !UNITY_IOS
            EditorGUILayout.LabelField("Please switch your platform to iOS to prepare the iOS build.", warningLabel);
            EditorGUI.BeginDisabledGroup(true);
#endif
            EditorGUILayout.Space(5f);

            var isiOSConfigValid = true;

            _propertiesData.APP_GROUP_NAME = CustomTextField("App Group Name", _propertiesData.APP_GROUP_NAME, "https://kindredsdkdocs.readme.io/docs/getting-started-1#app-group");
            if (string.IsNullOrEmpty(_propertiesData.APP_GROUP_NAME))
            {
                EditorGUILayout.HelpBox("App Group Name is not valid", MessageType.Error);
                isiOSConfigValid = false;
            }

            EditorGUILayout.Space(15f);

            isFormValid = isFormValid && isiOSConfigValid;
            if (!isFormValid)
            {
                EditorGUI.BeginDisabledGroup(true);
            }

            if (GUILayout.Button("Prepare iOS Build", GUILayout.Height(30)))
            {
                _actioniOS.Configure();
            }

            if (!isFormValid)
            {
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.HelpBox("Configuration is not valid", MessageType.Error);
            }
#if !UNITY_IOS
            EditorGUI.EndDisabledGroup();
#endif
        }

        private void ShowCredentialsSection(GUIStyle headerLabel)
        {
            if (string.IsNullOrEmpty(_propertiesData.TOKEN))
            {
                if(_credentialSectionState == CredentialState.Welcome)
                {
                    WelcomeSection(headerLabel);
                }
                else if(_credentialSectionState == CredentialState.Login)
                {
                    LoginSection(headerLabel);
                }
                else if(_credentialSectionState == CredentialState.Register)
                {
                    RegisterSection(headerLabel);
                }
                else
                {
                    EnterClientCredentialsSection(headerLabel);
                }
            }
            else
            {
                LoggedInSection(headerLabel);
            }
        }

        private void WelcomeSection(GUIStyle headerLabel)
        {
            // Credentials
            EditorGUILayout.LabelField("Home", headerLabel);
            EditorGUILayout.Space(5f);

            CenteredLabel("<size=13><color=#00ce05>Welcome to Kindred's new and improved SDK!</color></size>", true);
            CenteredLabel("A new, easy way to earn more from your game that doesn’t impact your ad revenue.");

            EditorGUILayout.Space(10f);

            CenteredLabel("If you know your client credentials...");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Enter client credentials", GUILayout.MaxWidth(200), GUILayout.Height(30)))
            {
                _credentialSectionState = CredentialState.EnterCredentials;
                GUI.FocusControl("");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(5f);

            CenteredLabel("Or you can access your client credentials by logging in or registering now...");

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Login", GUILayout.MaxWidth(200), GUILayout.Height(30)))
            {
                _credentialSectionState = CredentialState.Login;
            }

            if (GUILayout.Button("Register", GUILayout.MaxWidth(200), GUILayout.Height(30)))
            {
                _credentialSectionState = CredentialState.Register;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(5f);

            CenteredLabel("For more information...");

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Documentations", GUILayout.MaxWidth(200), GUILayout.Height(30)))
            {
                Application.OpenURL(KINDRED_DOCS_URL);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void EnterClientCredentialsSection(GUIStyle headerLabel)
        {
            // Credentials
            EditorGUILayout.LabelField("Kindred Client Credentials", headerLabel);
            EditorGUILayout.Space(5f);

            _propertiesData.AUTH_CLIENT_ID = CenteredTextField("Client ID: ", _propertiesData.AUTH_CLIENT_ID, "https://kindredsdkdocs.readme.io/docs/getting-started-1#authenticationcredentials");
            EditorGUILayout.Space(3f);
            _isClientIDValid = Guid.TryParse(_propertiesData.AUTH_CLIENT_ID, out var guidOutput);
            if (!_isClientIDValid)
            {
                EditorGUILayout.HelpBox("Client ID is not valid", MessageType.Error);
            }
            
            _propertiesData.AUTH_CLIENT_SECRET = CenteredTextField("Client Secret: ", _propertiesData.AUTH_CLIENT_SECRET, "https://kindredsdkdocs.readme.io/docs/getting-started-1#authenticationcredentials");
            EditorGUILayout.Space(3f);
            _isClientSecretValid = _propertiesData.AUTH_CLIENT_SECRET.Length == KEY_LENGTH;
            if (!_isClientSecretValid)
            {
                EditorGUILayout.HelpBox("Client Secret is not valid", MessageType.Error);
            }

            _propertiesData.AUTH_SHARED_KEY = CenteredTextField("Shared key: ", _propertiesData.AUTH_SHARED_KEY, "https://kindredsdkdocs.readme.io/docs/getting-started-1#authenticationcredentials");
            EditorGUILayout.Space(3f);
            _isSharedKeyValid = _propertiesData.AUTH_SHARED_KEY.Length == KEY_LENGTH;
            if (!_isSharedKeyValid)
            {
                EditorGUILayout.HelpBox("Shared Key is not valid", MessageType.Error);
            }

            EditorGUILayout.Space(15f);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Back to Home", GUILayout.MaxWidth(200), GUILayout.Height(30)))
            {
                _credentialSectionState = CredentialState.Welcome;
                GUI.FocusControl("");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void RegisterSection(GUIStyle headerLabel)
        {
            // Credentials
            EditorGUILayout.LabelField("Kindred Register", headerLabel);
            EditorGUILayout.Space(5f);

            CenteredLabel("Register to activate your Kindred SDK and start earning");
            EditorGUILayout.Space(3f);
            _registerName = CenteredTextField("Company/App Name: ", _registerName);
            EditorGUILayout.Space(3f);
            _registerEmail = CenteredTextField("Email Address: ", _registerEmail);
            EditorGUILayout.Space(3f);
            _registerPassword = CenteredTextField("Password: ", _registerPassword, "", false, true);
            NotifyUnfulfilledPasswordRequirements(_registerPassword);
            EditorGUILayout.Space(5f);

            if (!string.IsNullOrEmpty(_wwwError))
            {
                CenteredLabel("<b><size=13><color=#FF0000>Register Failed!</color></size></b>", true);
                EditorGUILayout.Space(3f);
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Register", GUILayout.MaxWidth(200), GUILayout.Height(30)))
            {
                SendRegisterRequest();
            }
            GUI.backgroundColor = backgroundColor;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(10f);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Back to Home", GUILayout.MaxWidth(200), GUILayout.Height(30)))
            {
                _credentialSectionState = CredentialState.Welcome;
                GUI.FocusControl("");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void LoginSection(GUIStyle headerLabel)
        {
            // Credentials
            EditorGUILayout.LabelField("Kindred Login", headerLabel);
            EditorGUILayout.Space(5f);

            CenteredLabel("Login to activate your Kindred SDK and start earning");
            EditorGUILayout.Space(3f);
            _loginEmail = CenteredTextField("Email Address: ", _loginEmail);
            EditorGUILayout.Space(3f);
            _loginPassword = CenteredTextField("Password: ", _loginPassword, "", false, true);
            EditorGUILayout.Space(5f);

            if(!string.IsNullOrEmpty(_wwwError))
            {
                CenteredLabel("<b><size=13><color=#FF0000>Login Failed!</color></size></b>", true);
                EditorGUILayout.Space(3f);
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Login", GUILayout.MaxWidth(200), GUILayout.Height(30)))
            {
                SendLoginRequest();
            }
            GUI.backgroundColor = backgroundColor;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(10f);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Back to Home", GUILayout.MaxWidth(200), GUILayout.Height(30)))
            {
                _credentialSectionState = CredentialState.Welcome;
                GUI.FocusControl("");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void LoggedInSection(GUIStyle headerLabel)
        {
            // Credentials
            EditorGUILayout.LabelField("Kindred Credentials", headerLabel);
            EditorGUILayout.Space(5f);

            CenteredLabel("Register to activate your Kindred SDK and start earning");
            EditorGUILayout.Space(3f);
            CenteredTextField(
                "Client ID: ",
                _propertiesData.AUTH_CLIENT_ID,
                "https://kindredsdkdocs.readme.io/docs/getting-started-1#authenticationcredentials",
                false);
            _isClientIDValid = Guid.TryParse(_propertiesData.AUTH_CLIENT_ID, out var guidOutput);
            if (!_isClientIDValid)
            {
                EditorGUILayout.HelpBox("Client ID is not valid", MessageType.Error);
            }
            EditorGUILayout.Space(3f);
            CenteredTextField(
                "Client Secret: ",
                _propertiesData.AUTH_CLIENT_SECRET,
                "https://kindredsdkdocs.readme.io/docs/getting-started-1#authenticationcredentials",
                false);
            _isClientSecretValid = _propertiesData.AUTH_CLIENT_SECRET.Length == KEY_LENGTH;
            if (!_isClientSecretValid)
            {
                EditorGUILayout.HelpBox("Client Secret is not valid", MessageType.Error);
            }
            EditorGUILayout.Space(3f);
            CenteredTextField(
                "Shared key: ",
                _propertiesData.AUTH_SHARED_KEY,
                "https://kindredsdkdocs.readme.io/docs/getting-started-1#authenticationcredentials",
                false);
            _isSharedKeyValid = _propertiesData.AUTH_SHARED_KEY.Length == KEY_LENGTH;
            if (!_isSharedKeyValid)
            {
                EditorGUILayout.HelpBox("Shared Key is not valid", MessageType.Error);
            }
            EditorGUILayout.Space(15f);

            CenteredLabel("<b><size=13><color=#fff>Caution:</color></size></b> Regenerating client secrets will invalidate client secrets previously used, so this", true);
            CenteredLabel("may have an impact if you have active users using a previously activated client secret.", true);
            //EditorGUILayout.LabelField("<b><size=13><color=#fff>Caution:</color></size></b> Regenerating client secrets will invalidate client secrets previously used, so this", centerLabelStyle, GUILayout.ExpandWidth(true));
            //EditorGUILayout.LabelField("may have an impact if you have active users using a previously activated client secret.", centerLabelStyle, GUILayout.ExpandWidth(true));

            EditorGUILayout.Space(5f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            _isUnderstandChecked = EditorGUILayout.Toggle("I understand", _isUnderstandChecked);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(5f);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (!_isUnderstandChecked) EditorGUI.BeginDisabledGroup(true);
            var backgroundColor = GUI.backgroundColor;
            //GUI.backgroundColor = new Color(180/255f, 50/255f, 0/255f);
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Regenerate Client Secret", GUILayout.MaxWidth(200), GUILayout.Height(30)))
            {
                SendResetCredentialsRequest();
            }
            GUI.backgroundColor = backgroundColor;
            if (!_isUnderstandChecked) EditorGUI.EndDisabledGroup();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(10f);

            //EditorGUILayout.LabelField("<b><size=13><color=#fff>Note:</color></size></b> Your client secret can't be retrieved again after generation, so make a note of it in secure storage.", centerLabelStyle, GUILayout.ExpandWidth(true));
            CenteredLabel("<b><size=13><color=#fff>Note:</color></size></b> Your client secret can't be retrieved again after generation, so make a note of it in secure storage.", true);

            EditorGUILayout.Space(10f);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Documentations", GUILayout.MaxWidth(200), GUILayout.Height(30)))
            {
                Application.OpenURL(KINDRED_DOCS_URL);
            }
            if (GUILayout.Button("Log out", GUILayout.MaxWidth(200), GUILayout.Height(30)))
            {
                Logout();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private string CenteredTextField(string caption, string value, string url = "", bool isDisabled = false, bool isPassword = false)
        {
            var labelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleRight
            };

            var textFieldStyle = new GUIStyle(GUI.skin.textField)
            {
                margin = new RectOffset(0, 190, 0, 0),
            };

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(caption, labelStyle);
            if(!string.IsNullOrEmpty(url)) LinkButton("(?)", url);
            if(isDisabled) EditorGUI.BeginDisabledGroup(true);
            var tf = isPassword ?
                EditorGUILayout.PasswordField("", value, textFieldStyle, GUILayout.Width(350)) :
                EditorGUILayout.TextField("", value, textFieldStyle, GUILayout.Width(350));
            if(isDisabled) EditorGUI.EndDisabledGroup();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return tf;
        }

        private void CenteredLabel(string label, bool richText = false)
        {
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, richText = richText };
            EditorGUILayout.LabelField(label, style, GUILayout.ExpandWidth(true));
        }

        private static void SecondLabel(string caption)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(LEFT_PANEL_WIDTH + 9));
            EditorGUILayout.LabelField(caption);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(3f);
        }

        private void CustomLabel(string caption, string url = "https://www.google.com")
        {
            GUILayout.BeginHorizontal();
            var guiContent = new GUIContent(caption);
            var width = new GUIStyle(GUI.skin.label).CalcSize(guiContent).x;
            EditorGUILayout.LabelField(guiContent, GUILayout.Width(width));
            LinkButton("(?)", url);
            GUILayout.EndHorizontal();
        }

        private string CustomTextField(string caption, string textField, string url = "https://www.google.com")
        {
            GUILayout.BeginHorizontal();
            var guiContent = new GUIContent(caption);
            var width = new GUIStyle(GUI.skin.label).CalcSize(guiContent).x;
            EditorGUILayout.LabelField(guiContent, GUILayout.Width(width));
            LinkButton("(?)", url);
            EditorGUILayout.LabelField("", GUILayout.Width(LEFT_PANEL_WIDTH - width - 18));
            var tf = EditorGUILayout.TextField("", textField);
            GUILayout.EndHorizontal();
            //EditorGUILayout.Space(8f);
            return tf;
        }

        private bool CustomToggle(string caption, bool toggleValue, string url = "https://www.google.com")
        {
            GUILayout.BeginHorizontal();
            var guiContent = new GUIContent(caption);
            var width = new GUIStyle(GUI.skin.label).CalcSize(guiContent).x;
            EditorGUILayout.LabelField(guiContent, GUILayout.Width(width));
            LinkButton("(?)", url);
            EditorGUILayout.LabelField("", GUILayout.Width(LEFT_PANEL_WIDTH - width - 18));
            var tv = EditorGUILayout.Toggle("", toggleValue);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(8f);
            return tv;
        }

        private int CustomSelectionGrid(string caption, int selected, string[] options, string url = "https://www.google.com")
        {
            GUILayout.BeginHorizontal();
            var guiContent = new GUIContent(caption);
            var width = new GUIStyle(GUI.skin.label).CalcSize(guiContent).x;
            EditorGUILayout.LabelField(guiContent, GUILayout.Width(width));
            LinkButton("(?)", url);
            EditorGUILayout.LabelField("", GUILayout.Width(LEFT_PANEL_WIDTH - width - 18));
            var select = GUILayout.SelectionGrid(selected, options, 1, EditorStyles.radioButton);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(8f);
            return select;
        }

        private int CustomComboBox(string caption, int selected, string[] options, string url = "https://www.google.com")
        {
            GUILayout.BeginHorizontal();
            var guiContent = new GUIContent(caption);
            var width = new GUIStyle(GUI.skin.label).CalcSize(guiContent).x;
            EditorGUILayout.LabelField(guiContent, GUILayout.Width(width));
            LinkButton("(?)", url);
            EditorGUILayout.LabelField("", GUILayout.Width(LEFT_PANEL_WIDTH - width - 18));
            var select = EditorGUILayout.Popup("", selected, options);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(8f);
            return select;
        }

        private Texture2D CustomTextureField(string caption, Texture2D texValue, string url = "https://www.google.com")
        {
            GUILayout.BeginHorizontal(GUILayout.Width(LEFT_PANEL_WIDTH));
            var guiContent = new GUIContent(caption);
            var width = new GUIStyle(GUI.skin.label).CalcSize(guiContent).x;
            EditorGUILayout.LabelField(guiContent, GUILayout.Width(width));
            LinkButton("(?)", url);
            EditorGUILayout.LabelField("", GUILayout.Width(LEFT_PANEL_WIDTH - width - 18));
            var tv = (Texture2D)EditorGUILayout.ObjectField(texValue, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(8f);
            return tv;
        }

        private static Texture2D TextureField(string name, Texture2D texture, bool isHorizontal = true, int width = 0, int height = 0)
        {
            if (isHorizontal)
                GUILayout.BeginHorizontal();
            else
                GUILayout.BeginVertical();

            var style = new GUIStyle(GUI.skin.label)
            {
                fixedWidth = width,
                fixedHeight = height
            };
            GUILayout.Label(name, style);
            var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));

            if (isHorizontal)
                GUILayout.EndHorizontal();
            else
                GUILayout.EndVertical();

            return result;
        }

        private void LinkButton(string caption, string url = "https://www.google.com")
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                richText = true
            };
            caption = string.Format("<color=#A0B4FF>{0}</color>", caption);

            bool bClicked = GUILayout.Button(caption, style, GUILayout.Width(18));

            var rect = GUILayoutUtility.GetLastRect();
            rect.width = style.CalcSize(new GUIContent(caption)).x;
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

            if (bClicked)
                Application.OpenURL(url);
        }

        public static void DrawUILine(Color color, int thickness = 2, int padding = 10, int yOffset = 0)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += (padding / 2) + yOffset;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        private void SendRegisterRequest()
        {
            if (_www != null)
                return;

            _wwwError = "";
            _www = KindredApi.PostRegister(_registerName, _registerEmail, _registerPassword);
            _www.SendWebRequest();
            _responseTextChanged += HandleRegisterResponse;
            EditorApplication.update += EditorUpdate;
        }

        private void HandleRegisterResponse(string text)
        {
            if(!string.IsNullOrEmpty(text))
            {
                ResetRegisterSection();
                Logout();
                SaveToken(text);
                SendGetCredentialsRequest();
            }
            _responseTextChanged -= HandleRegisterResponse;
        }

        private void ResetRegisterSection()
        {
            _registerEmail = "";
            _registerName = "";
            _registerPassword = "";
        }

        private void SendLoginRequest()
        {
            if (_www != null)
                return;

            _wwwError = "";
            _www = KindredApi.PostLogin(_loginEmail, _loginPassword);
            _www.SendWebRequest();
            _responseTextChanged += HandleLoginResponse;
            EditorApplication.update += EditorUpdate;
        }

        private void HandleLoginResponse(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                ResetLoginSection();
                Logout();
                SaveToken(text);
                SendGetCredentialsRequest();
            }
            _responseTextChanged -= HandleLoginResponse;
        }

        private void ResetLoginSection()
        {
            _loginEmail = "";
            _loginPassword = "";
        }

        private void SendGetCredentialsRequest()
        {
            if (_www != null)
                return;

            _www = KindredApi.GetCredentials(_propertiesData.TOKEN);
            _www.SendWebRequest();
            _responseTextChanged += HandleGetCredentialsResponse;
            EditorApplication.update += EditorUpdate;
        }

        private void HandleGetCredentialsResponse(string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                SendGenerateCredentialsRequest();
            }
            else
            {
                var credentials = JsonUtility.FromJson<GetCredentialsResponse>(text);
                SaveClientId(credentials.clientId);
                SaveClientSecret(credentials.clientSecret);
                SaveSharedKey(credentials.sharedKey);
            }

            _responseTextChanged -= HandleGetCredentialsResponse;
        }

        private void SendGenerateCredentialsRequest()
        {
            if (_www != null)
                return;

            _www = KindredApi.PostGenerateCredentials(_propertiesData.TOKEN);
            _www.SendWebRequest();
            _responseTextChanged += HandleGenerateCredentialsResponse;
            EditorApplication.update += EditorUpdate;
        }

        private void HandleGenerateCredentialsResponse(string text)
        {
            var generatedCredentials = JsonUtility.FromJson<GenerateCredentialsResponse>(text);
            SaveClientId(generatedCredentials.clientId);
            SaveClientSecret(generatedCredentials.clientSecret);
            SaveSharedKey(generatedCredentials.sharedKey);
            _responseTextChanged -= HandleGenerateCredentialsResponse;
        }

        private void SendResetCredentialsRequest()
        {
            if (_www != null)
                return;

            _www = KindredApi.PostResetCredentials(_propertiesData.TOKEN);
            _www.SendWebRequest();
            _responseTextChanged += HandleResetCredentialsResponse;
            EditorApplication.update += EditorUpdate;
        }

        private void HandleResetCredentialsResponse(string text)
        {
            SaveClientSecret(text);
            _responseTextChanged -= HandleResetCredentialsResponse;
        }

        private void EditorUpdate()
        {
            if (!_www.isDone)
                return;

            EditorApplication.update -= EditorUpdate;

            if (WebRequestFailed(_www))
            {
                _wwwError = _www.downloadHandler.text;
                _www = null;
            }
            else
            {
                var downloadedText = _www.downloadHandler.text;
                // set null before invoke
                _www = null;
                _responseTextChanged.Invoke(downloadedText);
            }
        }

        private void Logout()
        {
            GUI.FocusControl("");
            _isUnderstandChecked = false;
            SaveToken("");
            SaveClientId("");
            SaveClientSecret("");
            SaveSharedKey("");
        }

        private void SaveClientId(string clientId)
        {
            _propertiesData.AUTH_CLIENT_ID = clientId;
            EditorPrefs.SetString("AUTH_CLIENT_ID", clientId);
        }

        private void SaveClientSecret(string clientSecret)
        {
            _propertiesData.AUTH_CLIENT_SECRET = clientSecret;
            EditorPrefs.SetString("AUTH_CLIENT_SECRET", clientSecret);
        }

        private void SaveSharedKey(string sharedKey)
        {
            _propertiesData.AUTH_SHARED_KEY = sharedKey;
            EditorPrefs.SetString("AUTH_SHARED_KEY", sharedKey);
        }

        private void SaveToken(string token)
        {
            _propertiesData.TOKEN = token;
            EditorPrefs.SetString("KINDRED_TOKEN", token);
        }

        private void NotifyUnfulfilledPasswordRequirements(string password)
        {
            if (string.IsNullOrEmpty(password))
                return;

            if (password.Length < MINIMUM_PASSWORD_LENGTH)
                CenteredLabel($"<color=#FF0000>* Password must be at least {MINIMUM_PASSWORD_LENGTH} characters long.</color>", true);

            if (!Regex.IsMatch(password, "^.*?[a-z]+.*$"))
                CenteredLabel($"<color=#FF0000>* Password must contain at least 1 lowercase letter.</color>", true);

            if (!Regex.IsMatch(password, "^.*?[A-Z]+.*$"))
                CenteredLabel($"<color=#FF0000>* Password must contain at least 1 uppercase letter.</color>", true);

            if (!Regex.IsMatch(password, "^.*?[0-9]+.*$"))
                CenteredLabel($"<color=#FF0000>* Password must contain at least 1 number.</color>", true);

            if (!Regex.IsMatch(password, "^.*?[^a-zA-Z0-9]+.*$"))
                CenteredLabel($"<color=#FF0000>* Password must contain at least 1 special character.</color>", true);

            if (Regex.IsMatch(password, "^.*?[^a-zA-Z0-9._@+-]+.*$"))
                CenteredLabel($"<color=#FF0000>* Password can only contain the following special characters: [ - . _ @ + ].</color>", true);
        }

        private bool WebRequestFailed(UnityWebRequest webRequest)
        {
#if UNITY_2020_1_OR_NEWER
            return webRequest.result != UnityWebRequest.Result.Success;
#else
            return webRequest.isNetworkError || webRequest.isHttpError;
#endif
        }
    }
#endif
}

