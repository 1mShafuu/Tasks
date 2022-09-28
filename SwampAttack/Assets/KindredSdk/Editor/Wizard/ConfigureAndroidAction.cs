using System;
using System.IO;
using System.IO.Compression;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace KindredSDK.Editor
{
    public class ConfigureAndroidAction : WizardAction
    {
        private string PathToFileTemplates => Path.Combine(Application.dataPath, "KindredSdk", "Editor", "Templates", "Android");
        private const string NAMESPACE_URI = "http://schemas.android.com/apk/res/android";

        private readonly PropertiesData _propertiesData;
        public ConfigureAndroidAction(PropertiesData propertiesData) : base("Configure Android")
        {
            _propertiesData = propertiesData;
        }

        protected override bool CheckIfActionIsConfigured()
        {
            return false;
        }

        protected override void DoConfiguration()
        {
            EnsurePluginsDirectory();

            SetAndroidMinVersion();
            BackupPreviousCustomTemplates();
            EnableCustomTemplates();
            PopulateGradleProperties();
            PopulateAndroidManifest();
            PopulateGradleTemplate();
            //CopyAppIcon();
            CopyResFiles();
        }

        private void CopyAppIcon()
        {
            var appIconPath = _propertiesData.APP_ICON;
            if (string.IsNullOrEmpty(appIconPath))
                return;

            var from = appIconPath;
            if (!File.Exists(from))
                return;

            string sourceDir = Path.Combine(Application.dataPath, "Plugins", "Kindred", "Android");
            string sourceFile = Path.Combine(sourceDir, "kindred-sdk-unity-release.aar");
            var tempDir = Path.Combine(sourceDir, "temp");

            // decompress from .aar
            ZipFile.ExtractToDirectory(sourceFile, tempDir);

            // copy logo to res/drawable
            var directoryPath = Path.Combine(sourceDir, "temp", "res", "drawable");
            var to = Path.Combine(directoryPath, "kindredlogo.png");
            // rescale texture
            var fromTexture = LoadTextureFromAssetDatabase(from);
            var toTexture = ScaleTexture(fromTexture, 72, 72);
            SaveTextureAsPNG(toTexture, to);

            // remove source .aar
            File.Delete(sourceFile);

            // remove all meta files before compressing
            var files = Directory.GetFiles(tempDir, "*.meta", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                File.Delete(file);
            }

            // compress to .aar
            ZipFile.CreateFromDirectory(tempDir, sourceFile);

#if UNITY_EDITOR
            // remove temp folder and its meta file
            FileUtil.DeleteFileOrDirectory(tempDir);
            FileUtil.DeleteFileOrDirectory(Path.Combine(sourceDir, "temp.meta"));

            AssetDatabase.Refresh();
#endif
        }

        private void CopyResFiles()
        {
            string sourceDir = Path.Combine(Application.dataPath, "Plugins", "Kindred", "Android");
            string sourceFile = Path.Combine(sourceDir, "browser-sdk-1.2.0.aar");
            var tempDir = Path.Combine(sourceDir, "temp");

            // decompress from .aar
            ZipFile.ExtractToDirectory(sourceFile, tempDir);

            // copy accessibilityservice to res/xml
            var directoryPath = Path.Combine(sourceDir, "temp", "res", "xml");
            var to = Path.Combine(directoryPath, "accessibilityservice.xml");
            if (!File.Exists(to))
            {
                var from = Path.Combine(PathToFileTemplates, "accessibilityservice.txt");
                Directory.CreateDirectory(directoryPath);
                File.Copy(from, to);
            }

            // copy strings to res/values
            var directoryPathValues = Path.Combine(sourceDir, "temp", "res", "values");
            var toStrings = Path.Combine(directoryPathValues, "strings.xml");
            var fromStrings = Path.Combine(PathToFileTemplates, "strings.txt");
            var stringsText = File.ReadAllText(fromStrings);
            stringsText = stringsText.Replace("APP_NAME", Application.productName);
            File.WriteAllText(toStrings, stringsText);

            // remove source .aar
            File.Delete(sourceFile);

            // remove all meta files before compressing
            var files = Directory.GetFiles(tempDir, "*.meta", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                File.Delete(file);
            }

            // compress to .aar
            ZipFile.CreateFromDirectory(tempDir, sourceFile);

#if UNITY_EDITOR
            // remove temp folder and its meta file
            FileUtil.DeleteFileOrDirectory(tempDir);
            FileUtil.DeleteFileOrDirectory(Path.Combine(sourceDir, "temp.meta"));

            AssetDatabase.Refresh();
#endif
        }

        private void EnsurePluginsDirectory()
        {
            if (!Directory.Exists(PathToAndroid))
            {
                Directory.CreateDirectory(PathToAndroid);
            }
        }

        private void SetAndroidMinVersion()
        {
#if UNITY_EDITOR
#if UNITY_2020_3_OR_NEWER
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
#else
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;
#endif
#endif
        }

        private void BackupPreviousCustomTemplates()
        {
            var folderPath = Path.Combine(Application.dataPath, "Plugins", "Android");

            var manifestPath = Path.Combine(folderPath, "AndroidManifest.xml");
            var baseProjectTemplatePath = Path.Combine(folderPath, "baseProjectTemplate.gradle");
            var gradleTemplatePath = Path.Combine(folderPath, "gradleTemplate.properties");
            var mainTemplatePath = Path.Combine(folderPath, "mainTemplate.gradle");

            string newBackupDirPath;
            if (File.Exists(manifestPath) ||
                File.Exists(baseProjectTemplatePath) ||
                File.Exists(gradleTemplatePath) ||
                File.Exists(mainTemplatePath))
            {
                var templateBackupDirPath = Path.Combine(Application.dataPath, "Plugins", "AndroidTemplatesBackups");
                if (!Directory.Exists(templateBackupDirPath))
                    Directory.CreateDirectory(templateBackupDirPath);

                var backupFolderName = DateTime.Now.ToString("yyyyMMddTHHmmssZ");
                newBackupDirPath = Path.Combine(templateBackupDirPath, backupFolderName);
                Directory.CreateDirectory(newBackupDirPath);
            }
            else
            {
                return;
            }

            if(File.Exists(manifestPath))
            {
                var destPath = Path.Combine(newBackupDirPath, "AndroidManifest.xml");
                File.Copy(manifestPath, destPath);
            }

            if(File.Exists(baseProjectTemplatePath))
            {
                var destPath = Path.Combine(newBackupDirPath, "baseProjectTemplate.gradle");
                File.Copy(baseProjectTemplatePath, destPath);
            }

            if (File.Exists(gradleTemplatePath))
            {
                var destPath = Path.Combine(newBackupDirPath, "gradleTemplate.properties");
                File.Copy(gradleTemplatePath, destPath);
            }

            if(File.Exists(mainTemplatePath))
            {
                var destPath = Path.Combine(newBackupDirPath, "mainTemplate.gradle");
                File.Copy(mainTemplatePath, destPath);
            }
        }

        private void EnableCustomTemplates()
        {
            CreateCustomTemplateFile("AndroidManifest.xml", ANDROID_MANIFEST_TEMPLATE);
            CreateCustomTemplateFile("mainTemplate.gradle", MAIN_GRADLE_TEMPLATE);
            CreateCustomTemplateFile("baseProjectTemplate.gradle", GRADLE_BASE_PROJECT_TEMPLATE); 
            CreateCustomTemplateFile("gradleTemplate.properties", GRADLE_PROPERTIES_TEMPLATE);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        private void CreateCustomTemplateFile(string name, string content)
        {
            var path = Path.Combine(PathToAndroid, name);
            File.WriteAllText(path, content);
        }

        private void PopulateGradleProperties()
        {
            var filePath = Path.Combine(PathToAndroid, "gradleTemplate.properties");
            var content = File.ReadAllText(filePath);

            var textToAppend = BuildKindredConfigText();

            var additionalPropertiesTag = "**ADDITIONAL_PROPERTIES**";
            content = content.Replace(additionalPropertiesTag, textToAppend);

            File.WriteAllText(filePath, content);
        }

        private string BuildKindredConfigText()
        {
            string text = String.Empty;
            text += "# Begin Kindred Properties" + '\n';
            text += "API_URL=https://api-partners.kindred.co\n";
            text += "ASSETS_CDN_URL=https://cdn.kindred.co\n";
            text += "AUTH_CLIENT_ID=" + _propertiesData.AUTH_CLIENT_ID + '\n';
            text += "AUTH_CLIENT_SECRET=" + _propertiesData.AUTH_CLIENT_SECRET + '\n';
            text += "AUTH_SHARED_KEY=" + _propertiesData.AUTH_SHARED_KEY + '\n';
            text += "kindred_sdk_maven_url=https://maven.pkg.github.com/kindred-app/KindredAndroidPackages\n";
            text += "android.useAndroidX=true" + '\n';
            text += "android.enableJetifier=true" + '\n';
            text += "# End Kindred Properties" + '\n';
            return text;
        }

        private void PopulateAndroidManifest()
        {
            var manifestPath = Path.Combine(PathToAndroid, "AndroidManifest.xml");
            var document = new XmlDocument();
            document.Load(manifestPath);

            var manifestNode = document["manifest"];

            var appNode = document["manifest"]["application"];

            // By default Unity application does not have theme, it's needed for settings activity.
            appNode.SetAttribute("theme", NAMESPACE_URI, "@style/Theme.AppCompat.Light");

            var settingsActivity = document.CreateElement("activity");
            settingsActivity.SetAttribute("name", NAMESPACE_URI, ".SettingsActivity");
            settingsActivity.SetAttribute("exported", NAMESPACE_URI, "false");
            settingsActivity.SetAttribute("label", NAMESPACE_URI, Application.productName);
            appNode.AppendChild(settingsActivity);

            var element = CreateServiceElement(document);
            appNode.AppendChild(element);

            XmlElement internetPermission = document.CreateElement("uses-permission");
            internetPermission.SetAttribute("name", NAMESPACE_URI, "android.permission.INTERNET");
            manifestNode.AppendChild(internetPermission);

            XmlElement systemAlertPermission = document.CreateElement("uses-permission");
            systemAlertPermission.SetAttribute("name", NAMESPACE_URI, "android.permission.SYSTEM_ALERT_WINDOW");
            manifestNode.AppendChild(systemAlertPermission);

            document.Save(manifestPath);
        }

        private XmlElement CreateServiceElement(XmlDocument document)
        {
            var serviceElement = document.CreateElement("service");
            serviceElement.SetAttribute("name", NAMESPACE_URI, ".DealsAccessibilityService");
            serviceElement.SetAttribute("canRetrieveWindowContent", NAMESPACE_URI, "true");
            serviceElement.SetAttribute("exported", NAMESPACE_URI, "true");
            serviceElement.SetAttribute("label", NAMESPACE_URI, Application.productName);
            serviceElement.SetAttribute("permission", NAMESPACE_URI, "android.permission.BIND_ACCESSIBILITY_SERVICE");

            // Intent Filter
            var filterElement = document.CreateElement("intent-filter");
            var actionElement = document.CreateElement("action");
            actionElement.SetAttribute("name", NAMESPACE_URI, "android.accessibilityservice.AccessibilityService");
            filterElement.AppendChild(actionElement);

            serviceElement.AppendChild(filterElement);

            // Meta-data
            var metaElement = document.CreateElement("meta-data");
            metaElement.SetAttribute("name", NAMESPACE_URI, "android.accessibilityservice");
            metaElement.SetAttribute("resource", NAMESPACE_URI, "@xml/accessibilityservice");

            serviceElement.AppendChild(metaElement);
            return serviceElement;
        }

        private void PopulateGradleTemplate()
        {
            ConfigureMainTemplate();
        }

        private void ConfigureMainTemplate()
        {
            string path = Path.Combine(PathToAndroid, "mainTemplate.gradle");
            var text = File.ReadAllText(path);

#if UNITY_2020_3_OR_NEWER
            text = text.Replace("**MINSDKVERSION**", "22");
#else
            text = text.Replace("**MINSDKVERSION**", "21");
#endif

            string buildConfigField =
                @"buildConfigField(""String"", ""API_URL"", ""\"""" + API_URL + ""\"""") 
                buildConfigField(""String"", ""ASSETS_CDN_URL"", ""\"""" + ASSETS_CDN_URL + ""\"""") 
                buildConfigField(""String"", ""AUTH_CLIENT_ID"", ""\"""" + AUTH_CLIENT_ID + ""\"""") 
                buildConfigField(""String"", ""AUTH_CLIENT_SECRET"", ""\"""" + AUTH_CLIENT_SECRET + ""\"""")
                buildConfigField(""String"", ""AUTH_SHARED_KEY"",  ""\"""" + AUTH_SHARED_KEY + ""\"""")";

            string proguardTag = "**USER_PROGUARD**";
            int index = text.IndexOf(proguardTag);
            text = text.Insert(index + proguardTag.Length, "\n" + buildConfigField);

            File.WriteAllText(path, text);
        }

        public static readonly string ANDROID_MANIFEST_TEMPLATE =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
            <!-- GENERATED BY UNITY. REMOVE THIS COMMENT TO PREVENT OVERWRITING WHEN EXPORTING AGAIN-->
            <manifest
                xmlns:android=""http://schemas.android.com/apk/res/android""
                package=""com.unity3d.player""
                xmlns:tools=""http://schemas.android.com/tools"">
                <application>
                    <activity android:name=""com.unity3d.player.UnityPlayerActivity""
                    android:theme=""@style/UnityThemeSelector"">
                        <intent-filter>
                        <action android:name=""android.intent.action.MAIN"" />
                        <category android:name=""android.intent.category.LAUNCHER"" />
                        </intent-filter>
                        <meta-data android:name=""unityplayer.UnityActivity"" android:value=""true"" />
                    </activity>
                </application>
            </manifest>
            ";
        

        public static readonly string MAIN_GRADLE_TEMPLATE =
            @"// GENERATED BY UNITY. REMOVE THIS COMMENT TO PREVENT OVERWRITING WHEN EXPORTING AGAIN

            apply plugin: 'com.android.library'
            apply plugin: 'kotlin-android'
            **APPLY_PLUGINS**

            dependencies {
                // Necessary for Kindred-SDK
                implementation fileTree(dir: 'libs', include: ['*.jar'])
                implementation 'com.squareup.okhttp3:okhttp:4.9.3'
                implementation 'com.google.code.gson:gson:2.9.1'
                implementation 'org.jetbrains.kotlinx:kotlinx-coroutines-core:1.4.3'
                implementation 'org.jetbrains.kotlinx:kotlinx-coroutines-android:1.4.3'
                implementation 'androidx.appcompat:appcompat:1.2.0'
                implementation 'androidx.preference:preference-ktx:1.2.0'
                implementation 'androidx.constraintlayout:constraintlayout:2.0.4'
                implementation 'androidx.navigation:navigation-fragment:2.3.5'
                implementation 'androidx.navigation:navigation-ui:2.3.5'
            **DEPS**}

            android {
                compileSdkVersion **APIVERSION**
                buildToolsVersion '**BUILDTOOLS**'

                compileOptions {
                    sourceCompatibility JavaVersion.VERSION_1_8
                    targetCompatibility JavaVersion.VERSION_1_8
                }

                kotlinOptions {
                    jvmTarget = '1.8'
                }

                defaultConfig {
                    minSdkVersion **MINSDKVERSION**
                    targetSdkVersion **TARGETSDKVERSION**
                    ndk {
                        abiFilters **ABIFILTERS**
                    }
                    versionCode **VERSIONCODE**
                    versionName '**VERSIONNAME**'
                    consumerProguardFiles 'proguard-unity.txt'**USER_PROGUARD**
                }

                lintOptions {
                    abortOnError false
                }

                aaptOptions {
                    noCompress = ['.json', '.wav', '.ress', '.resource', '.obb'] + unityStreamingAssets.tokenize(', ')
                    ignoreAssetsPattern = ""!.svn:!.git:!.ds_store:!*.scc:.*:!CVS:!thumbs.db:!picasa.ini:!*~""
            }**PACKAGING_OPTIONS**
            }**REPOSITORIES**
            **IL_CPP_BUILD_SETUP**
            **SOURCE_BUILD_SETUP**
            **EXTERNAL_SOURCES**
            ";

        public static readonly string GRADLE_PROPERTIES_TEMPLATE =
            @"org.gradle.jvmargs=-Xmx**JVM_HEAP_SIZE**M
            org.gradle.parallel=true" + '\n' +
#if UNITY_2020_3_OR_NEWER
            "android.enableR8=**MINIFY_WITH_R_EIGHT**" + '\n' +
#endif
            @"unityStreamingAssets=.unity3d**STREAMING_ASSETS**
            **ADDITIONAL_PROPERTIES**
            ";

        public static readonly string GRADLE_BASE_PROJECT_TEMPLATE =
            @"// GENERATED BY UNITY. REMOVE THIS COMMENT TO PREVENT OVERWRITING WHEN EXPORTING AGAIN

            allprojects {
                buildscript {
                    repositories {**ARTIFACTORYREPOSITORY**
                        google()
                        jcenter()
                    }

                    dependencies {
                        // If you are changing the Android Gradle Plugin version, make sure it is compatible with the Gradle version preinstalled with Unity
                        // See which Gradle version is preinstalled with Unity here https://docs.unity3d.com/Manual/android-gradle-overview.html
                        // See official Gradle and Android Gradle Plugin compatibility table here https://developer.android.com/studio/releases/gradle-plugin#updating-gradle
                        // To specify a custom Gradle version in Unity, go do ""Preferences > External Tools"", uncheck ""Gradle Installed with Unity (recommended)"" and specify a path to a custom Gradle version" + '\n' +
#if UNITY_2020_3_OR_NEWER
                    @"  classpath 'com.android.tools.build:gradle:4.0.1'
                        classpath 'org.jetbrains.kotlin:kotlin-gradle-plugin:1.5.31'" + '\n' +
#elif UNITY_2020_1_OR_NEWER
                    @"  classpath 'com.android.tools.build:gradle:3.6.0'
                        classpath 'org.jetbrains.kotlin:kotlin-gradle-plugin:1.4.20'" + '\n' +
#else
                    @"  classpath 'com.android.tools.build:gradle:3.4.3'
                        classpath 'org.jetbrains.kotlin:kotlin-gradle-plugin:1.3.70'" + '\n' +
#endif
            @"      **BUILD_SCRIPT_DEPS**
                }
            }

            repositories {**ARTIFACTORYREPOSITORY**
                google()
                jcenter()
                flatDir {
                    dirs ""${project(':unityLibrary').projectDir}/libs""
                }
            }
            }

            task clean(type: Delete) {
                delete rootProject.buildDir
            }
            ";
    }
}