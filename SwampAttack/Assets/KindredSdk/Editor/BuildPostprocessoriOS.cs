using UnityEngine;
using System.IO;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
#endif
#endif

namespace KindredSDK.Editor
{
    public class BuildPostprocessoriOS
    {
#if UNITY_EDITOR && UNITY_IOS
        private static string AppAssetsPath => Path.Combine(Application.dataPath, "KindredSdk", "Editor", "Templates", "iOS");
        private static string AppGroupName => EditorPrefs.GetString("APP_GROUP_NAME", "");

        [PostProcessBuild]
        private static void PostProcessBuildiOS(BuildTarget target, string buildPath)
        {
            if (target == BuildTarget.iOS)
            {
                HandleiOSPostBuild(buildPath);
            }
        }

        private static void HandleiOSPostBuild(string buildPath)
        {
            PBXProject project = new PBXProject();
            string projPath = PBXProject.GetPBXProjectPath(buildPath);
            project.ReadFromFile(projPath);

            var mainProjectGuid = project.GetUnityMainTargetGuid();
            string unityFrameworkGuid = project.GetUnityFrameworkTargetGuid();

            project.SetBuildProperty(mainProjectGuid, "ENABLE_BITCODE", "NO");
            project.SetBuildProperty(unityFrameworkGuid, "ENABLE_BITCODE", "NO");

            AddAppGroups(buildPath, project, projPath, mainProjectGuid);

            var propertiesData = new PropertiesData();
            AddSafariExtension(project, mainProjectGuid, projPath, buildPath);

            HandleBridge(project, mainProjectGuid, unityFrameworkGuid, buildPath);
            UpdatePlist(Path.Combine(buildPath, "Info.plist"));

            project.WriteToFile(projPath);
        }

        private static void AddAppGroups(string buildPath, PBXProject project, string projPath, string mainProjectGuid)
        {
            var entitlementsName = "kindred-unity.entitlements";
            var entitlementsProjPath = Path.Combine("Unity-iPhone", entitlementsName);
            var projCapability = new ProjectCapabilityManager(projPath, entitlementsProjPath, targetGuid: mainProjectGuid);
            projCapability.AddAppGroups(new string[] { AppGroupName });
            projCapability.WriteToFile();
            var entitlementsGuid = project.AddFile(Path.Combine(buildPath, entitlementsProjPath), entitlementsName);
            project.SetBuildProperty(mainProjectGuid, "CODE_SIGN_ENTITLEMENTS", entitlementsProjPath);
        }

        private static void UpdatePlist(string plistPath)
        {
            // Get plist
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            PlistElementDict rootDict = plist.root;

            // Add app group name for defaults
            rootDict.SetString("AppGroupName", AppGroupName);

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }

        private static void HandleBridge(PBXProject project, string mainProjectGuid, string unityFrameworkGuid, string buildPath)
        {
            // Modulemap
            project.AddBuildProperty(unityFrameworkGuid, "DEFINES_MODULE", "YES");

            var moduleFile = buildPath + "/UnityFramework/UnityFramework.modulemap";
            if (!File.Exists(moduleFile))
            {
                FileUtil.CopyFileOrDirectory("Assets/Plugins/Kindred/iOS/UnityFramework.modulemap", moduleFile);
                project.AddFile(moduleFile, "UnityFramework/UnityFramework.modulemap");
                project.AddBuildProperty(unityFrameworkGuid, "MODULEMAP_FILE", "$(SRCROOT)/UnityFramework/UnityFramework.modulemap");
            }

            // Headers
            string unityInterfaceGuid = project.FindFileGuidByProjectPath("Classes/Unity/UnityInterface.h");
            project.AddPublicHeaderToBuild(unityFrameworkGuid, unityInterfaceGuid);

            string unityForwardDeclsGuid = project.FindFileGuidByProjectPath("Classes/Unity/UnityForwardDecls.h");
            project.AddPublicHeaderToBuild(unityFrameworkGuid, unityForwardDeclsGuid);

            string unityRenderingGuid = project.FindFileGuidByProjectPath("Classes/Unity/UnityRendering.h");
            project.AddPublicHeaderToBuild(unityFrameworkGuid, unityRenderingGuid);

            string unitySharedDeclsGuid = project.FindFileGuidByProjectPath("Classes/Unity/UnitySharedDecls.h");
            project.AddPublicHeaderToBuild(unityFrameworkGuid, unitySharedDeclsGuid);
        }

        private static void AddSafariExtension(PBXProject proj, string mainProjectGuid, string projPath, string buildPath)
        {
            var extensionName = "KindredPlugin";

            string sourcePath = Path.Combine(AppAssetsPath, extensionName);
            var destPath = Path.Combine(buildPath, extensionName);
            FileUtil.CopyFileOrDirectory(sourcePath, destPath);
            // remove all meta files
            var files = Directory.GetFiles(destPath, "*.meta", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                File.Delete(file);
            }

            var resourcesFolderPath = Path.Combine(buildPath, extensionName);

            // rename build folder js files
            var buildJsFiles = new string[] { "background", "browsercompat", "contentscript", "main" };
            var buildJsFolderPath = Path.Combine(buildPath, extensionName,  "static", "js");
            foreach (var jsFile in buildJsFiles)
            {
                var sourceJsFile = Path.Combine(buildJsFolderPath, jsFile + ".txt");
                var targetJsFile = Path.Combine(buildJsFolderPath, jsFile + ".js");
                File.Move(sourceJsFile, targetJsFile);
            }

            var identifier = Application.identifier;
            string extGuid = proj.AddAppExtension(mainProjectGuid, extensionName, identifier + "." + extensionName, extensionName + "/Info.plist");

            proj.SetBuildProperty(extGuid, "IPHONEOS_DEPLOYMENT_TARGET", "15.0");
            proj.SetBuildProperty(extGuid, "TARGETED_DEVICE_FAMILY", "1,2");
            proj.SetBuildProperty(extGuid, "SWIFT_VERSION", "5.0");
            proj.SetBuildProperty(extGuid, "ENABLE_BITCODE", "NO");
            var archs = PlayerSettings.iOS.sdkVersion == iOSSdkVersion.DeviceSDK ? "arm64" : "x86_64";
            proj.SetBuildProperty(extGuid, "ARCHS", archs);  // "arm64", "armv7 arm64", "$(ARCHS_STANDARD)", "x86_64"

            // add info.plist reference to the extension
            var plistPath = Path.Combine(buildPath, extensionName, "Info.plist");
            var infoGuid = proj.AddFile(plistPath, Path.Combine(extensionName, "Info.plist"));
            UpdatePlist(plistPath);

            var swiftFileNames = new string[] { "SafariWebExtensionHandler.swift", "KindredSWEHandler.swift", "KindredSettings.swift" };
            foreach (var fileName in swiftFileNames)
            {
                // add SafariWebExtensionHandler reference to the extension
                var fileGuid = proj.AddFile(Path.Combine(buildPath, extensionName, fileName),
                    Path.Combine(extensionName, fileName));
                proj.AddFileToBuild(extGuid, fileGuid); // to put it into "Compile Resources" in "Build Phase"
            }

            // handle "Copy Bundle Resources" in "Build Phase"
            var extBuildPath = Path.Combine(buildPath, extensionName);
            var fileNames = new string[] { "asset-manifest.json", "index.html", "manifest.json" };
            foreach (var fileName in fileNames)
            {
                var path = Path.Combine(extBuildPath, fileName);
                var fGuid = proj.AddFile(path, Path.Combine(extensionName, fileName));
                proj.AddFileToBuild(extGuid, fGuid);
            }

            var folderNames = new string[] { "_locales", "icons", "static" };
            foreach (var folderName in folderNames)
            {
                var folderPath = Path.Combine(extBuildPath, folderName);
                var folderGuid = proj.AddFolderReference(folderPath, Path.Combine(extensionName, folderName));
                proj.AddFileToBuild(extGuid, folderGuid);
            }

            // add plugin extension entitlements
            var entitlementsProjPath = Path.Combine(extensionName, "KindredPlugin.entitlements");
            var projCapability = new ProjectCapabilityManager(projPath, entitlementsProjPath, targetGuid: mainProjectGuid);
            projCapability.AddAppGroups(new string[] { AppGroupName });
            projCapability.WriteToFile();
            var entitlementsGuid = proj.AddFile(Path.Combine(buildPath, entitlementsProjPath), entitlementsProjPath);
            proj.SetBuildProperty(extGuid, "CODE_SIGN_ENTITLEMENTS", entitlementsProjPath);

            // change locales
            var manifestPath = Path.Combine(extBuildPath, "manifest.json");
            var manifestJsonText = File.ReadAllText(manifestPath);
            manifestJsonText = manifestJsonText.Replace("APP_NAME", Application.productName);
            File.WriteAllText(manifestPath, manifestJsonText);

            var buildLocalesPath = Path.Combine(extBuildPath, "_locales", "en", "messages.json");
            var buildLocalesJsonText = File.ReadAllText(buildLocalesPath);
            buildLocalesJsonText = buildLocalesJsonText.Replace("APP_NAME", Application.productName);
            File.WriteAllText(buildLocalesPath, buildLocalesJsonText);

            var resourcesLocalesPath = Path.Combine(resourcesFolderPath, "_locales", "en", "messages.json");
            var resourcesLocalesJsonText = File.ReadAllText(resourcesLocalesPath);
            resourcesLocalesJsonText = resourcesLocalesJsonText.Replace("APP_NAME", Application.productName);
            File.WriteAllText(resourcesLocalesPath, resourcesLocalesJsonText);
        }
#endif
    }
}