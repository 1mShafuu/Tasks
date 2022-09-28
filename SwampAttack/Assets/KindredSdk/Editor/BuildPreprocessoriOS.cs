using System;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
#endif
using UnityEngine;

namespace KindredSDK.Editor
{
#if UNITY_EDITOR
    class BuildPreprocessoriOS : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.iOS)
            {
                IsReady(report.summary.outputPath);
                AddUrlSchemes();
            }
        }

        private void IsReady(string buildPath)
        {
            CheckPluginsKindrediOSFolder();
        } 

        private void CheckPluginsKindrediOSFolder()
        {
            var folderPath = Path.Combine(Application.dataPath, "Plugins", "Kindred", "iOS");

            if (!File.Exists(Path.Combine(folderPath, "KindredService.swift")) ||
                !File.Exists(Path.Combine(folderPath, "KindredSdkBridge.mm")) ||
                !Directory.Exists(Path.Combine(folderPath, "Settings.bundle")) ||
                !File.Exists(Path.Combine(folderPath, "UnityFramework.modulemap")))
            {
                Debug.LogError("The plugin structure is not correct. Try reimporting the plugin.");
                // Rethrow exceptions during build postprocessing as BuildFailedException, so we don't pretend the build was fine.
                throw new BuildFailedException("The plugin structure is not correct.");
            }
        }

        private void AddUrlSchemes()
        {
            var urlSchemes = PlayerSettings.iOS.iOSUrlSchemes;
            var productName = Application.productName.ToLower().Replace(" ", string.Empty);
            if (!urlSchemes.Contains(productName))
            {
                var urlSchemesList = urlSchemes.ToList();
                urlSchemesList.Add(productName);
                PlayerSettings.iOS.iOSUrlSchemes = urlSchemesList.ToArray();
            }
        }
    }
#endif
}