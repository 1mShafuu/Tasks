using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
#endif
using UnityEngine;

namespace KindredSDK.Editor
{
    public class ConfigureiOSAction : WizardAction
    {
        private string PathToFileTemplates => Path.Combine(Application.dataPath,
            "KindredSdk", "Editor", "Templates", "iOS");
        private string PathToKindredPlugin => Path.Combine(PathToFileTemplates, "KindredPlugin");

        private readonly PropertiesData _propertiesData;
        public ConfigureiOSAction(PropertiesData propertiesData) : base("Configure iOS")
        {
            _propertiesData = propertiesData;
        }

        protected override bool CheckIfActionIsConfigured()
        {
            return false;
        }

        protected override void DoConfiguration()
        {
            UpdateSafariPluginInfoPlist();
            PreparePluginLogos();
        }

        private void PreparePluginLogos()
        {
            CreatePluginActiveIcons();
            CreatePluginInactiveIcons();

#if UNITY_EDITOR_OSX
            AssetDatabase.Refresh();
#endif
        }

        private void CreatePluginInactiveIcons()
        {
            var inactiveDirectoryPath = Path.Combine(PathToKindredPlugin, "icons", "grey");

            if (Directory.Exists(inactiveDirectoryPath))
            {
                var appIconFrom = _propertiesData.APP_ICON;
                if (!string.IsNullOrEmpty(appIconFrom) && File.Exists(appIconFrom))
                {
                    var fromTexture = LoadTextureFromAssetDatabase(appIconFrom);
                    // inactive icon 16
                    var to16 = Path.Combine(inactiveDirectoryPath, "16.png");
                    CopyAndSaveGrayscaleAsPNG(fromTexture, to16, 16, 16);
                    // inactive icon 24
                    var to24 = Path.Combine(inactiveDirectoryPath, "24.png");
                    CopyAndSaveGrayscaleAsPNG(fromTexture, to24, 24, 24);
                    // inactive icon 32
                    var to32 = Path.Combine(inactiveDirectoryPath, "32.png");
                    CopyAndSaveGrayscaleAsPNG(fromTexture, to32, 32, 32);
                    // inactive icon 32
                    var to48 = Path.Combine(inactiveDirectoryPath, "48.png");
                    CopyAndSaveGrayscaleAsPNG(fromTexture, to48, 48, 48);
                    // inactive icon 32
                    var to128 = Path.Combine(inactiveDirectoryPath, "128.png");
                    CopyAndSaveGrayscaleAsPNG(fromTexture, to128, 128, 128);
                }
            }
        }

        private void CreatePluginActiveIcons()
        {
            var activeDirectoryPath = Path.Combine(PathToKindredPlugin, "icons", "color");

            if (Directory.Exists(activeDirectoryPath))
            {
                var appIconFrom = _propertiesData.APP_ICON;
                if (!string.IsNullOrEmpty(appIconFrom) && File.Exists(appIconFrom))
                {
                    var fromTexture = LoadTextureFromAssetDatabase(appIconFrom);
                    // active icon 16
                    var to16 = Path.Combine(activeDirectoryPath, "16.png");
                    CopyAndSaveTextureAsPNG(fromTexture, to16, 16, 16);
                    // active icon 24
                    var to24 = Path.Combine(activeDirectoryPath, "24.png");
                    CopyAndSaveTextureAsPNG(fromTexture, to24, 24, 24);
                    // active icon 32
                    var to32 = Path.Combine(activeDirectoryPath, "32.png");
                    CopyAndSaveTextureAsPNG(fromTexture, to32, 32, 32);
                    // active icon 48
                    var to48 = Path.Combine(activeDirectoryPath, "48.png");
                    CopyAndSaveTextureAsPNG(fromTexture, to48, 48, 48);
                    // active icon 48
                    var to128 = Path.Combine(activeDirectoryPath, "128.png");
                    CopyAndSaveTextureAsPNG(fromTexture, to128, 128, 128);
                    // active icon 48
                    var toLogo = Path.Combine(activeDirectoryPath, "logo.png");
                    CopyAndSaveTextureAsPNG(fromTexture, toLogo, 128, 128);
                }
            }
        }

        private void UpdateSafariPluginInfoPlist()
        {
#if UNITY_EDITOR && UNITY_IOS
            // Get plist
            var plistPath = Path.Combine(PathToKindredPlugin, "Info.plist");
            var plist = LoadPlist(plistPath);

            // Get root
            PlistElementDict rootDict = plist.root;

            var dict = rootDict.CreateDict("Kindred");
            dict.values.Add("CLIENT_ID", new PlistElementString(_propertiesData.AUTH_CLIENT_ID));
            dict.values.Add("CLIENT_SECRET", new PlistElementString(_propertiesData.AUTH_CLIENT_SECRET));
            dict.values.Add("SHARED_KEY", new PlistElementString(_propertiesData.AUTH_SHARED_KEY));

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
#endif
        }

#if UNITY_EDITOR && UNITY_IOS
        private PlistDocument LoadPlist(string plistPath)
        {
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            return plist;
        }
#endif

        private void CopyAndSaveGrayscaleAsPNG(Texture2D fromTexture, string destPath, int width, int height)
        {
            var toTexture = ScaleAndMakeGrayscale(fromTexture, width, height);
            SaveTextureAsPNG(toTexture, destPath);
        }

        private void CopyAndSaveTextureAsPNG(Texture2D fromTexture, string destPath, int width, int height)
        {
            var toTexture = ScaleTexture(fromTexture, width, height);
            SaveTextureAsPNG(toTexture, destPath);
        }
    }
}