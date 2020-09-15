using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using System;

namespace CustomBuildSettings
{
    public class CustomBuildWindow : EditorWindow
    {
        public enum SEParty_AppType
        {
            GameClient,
            AdminClient
        }

        private const string buildConfigFilepath = "Assets/Editor/CustomBuildWindow/buildconfig.json";

        /*
         * Build Settings Variables - should save in EditorPrefs
         */
        private SEParty_AppType appType;
        private BuildConfigs buildConfigs;
        private const string appType_Key = "AppType";

        /*
         * Variables duplicated for each App Type
         */
        private string appString = "SE Party";
        private const string appString_Key = "AppString";
        private string companyString = "SuperSad";
        //private const string companyString_Key = "CompanyString";
        private string packageName = "com.supersad.SEParty";
        //private const string packageName_Key = "PackageName";
        //private string appIconFilePath;
        //private const string appIconPath_Key = "AppIconFilePath";
        private int numScenes;
        //private const string numScenes_Key = "NumberScenes";
        //private const string sceneAsset_KeyPrefix = "Scene";

        private const string saveFolderPath_Key = "SaveFolder";

        // Instantiated locally
        private Object[] sceneAssets;
        private Texture2D appIcon;

        //private bool buildGameClient = true;
        //private const string buildGameClient_Key = "BuildGameClient";

        // Add menu item named "My Window" to the Window menu
        [MenuItem("SE Party/Build Settings Window")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow window = EditorWindow.GetWindow(typeof(CustomBuildWindow));
            window.titleContent = new GUIContent("Custom Build");
        }

        void Awake()
        {
            InitConfig();

            // Initialize variables
            appType = (SEParty_AppType)EditorPrefs.GetInt(appType_Key, (int)SEParty_AppType.GameClient);
            //ReadEditorPrefs(appType);
            ReadBuildConfig(appType);
        }

        private void InitConfig()
        {
            TextAsset buildConfigFile = AssetDatabase.LoadAssetAtPath(buildConfigFilepath, typeof(TextAsset)) as TextAsset;

            Debug.Log(buildConfigFile);
            buildConfigs = JsonUtility.FromJson<BuildConfigs>(buildConfigFile.text);
        }

        private void ReadBuildConfig(SEParty_AppType app)
        {
            string appType = app.ToString();
            if (buildConfigs == null)
            {
                Debug.Log("buildConfigs is null");
                return;
            }
            foreach (BuildConfig config in buildConfigs.Configs)
            {
                if (config.AppType == appType)
                {
                    ConfigData data = config.ConfigData;
                    appString = data.AppName;
                    companyString = data.Company;
                    packageName = data.PackageName;

                    if (!string.IsNullOrEmpty(data.AppIconFilePath))
                        appIcon = AssetDatabase.LoadAssetAtPath(data.AppIconFilePath, typeof(Texture2D)) as Texture2D;

                    numScenes = data.NumScenes;
                    sceneAssets = new Object[numScenes];
                    for (int i = 0; i < numScenes; i++)
                    {
                        if (!string.IsNullOrEmpty(data.ScenePaths[i]))
                            sceneAssets[i] = AssetDatabase.LoadAssetAtPath(data.ScenePaths[i], typeof(Object)) as Object;
                    }
                    return;
                }
            }
        }

        void OnGUI()
        {
            GUILayout.Label("SE Party Build Settings", EditorStyles.boldLabel);

            SEParty_AppType newAppType = (SEParty_AppType)EditorGUILayout.EnumPopup("Application", appType);
            if (newAppType != appType)
            {
                // Write for the current app type
                //WriteEditorPrefs(appType);
                UpdateBuildConfig(appType);

                // Update to new app type
                appType = newAppType;
                EditorPrefs.SetInt(appType_Key, (int)newAppType);

                // Read new app type
                //ReadEditorPrefs(newAppType);
                ReadBuildConfig(appType);
            }

            switch (appType)
            {
                case SEParty_AppType.GameClient:
                    GUIGameClient();
                    break;

                case SEParty_AppType.AdminClient:
                    GUIAdminClient();
                    break;
            }
        }

        private void GUIGameClient()
        {
            // Set Scenes
            DrawGUILayouts();

            BuildTarget currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;

            // Create Build for Standalone button
            string standaloneButtonText = "Build Game Client (Windows)";
            if (currentBuildTarget != BuildTarget.StandaloneWindows && currentBuildTarget != BuildTarget.StandaloneWindows64)
                standaloneButtonText += " [Switch Platform Required]";
            if (GUILayout.Button(standaloneButtonText))
            {
                BuildSEParty(SEParty_AppType.GameClient, BuildTargetGroup.Standalone, true, "SEParty");
            }

            // Create Build for Android button
            string androidButtonText = "Build Game Client (Android)";
            if (currentBuildTarget != BuildTarget.Android)
                androidButtonText += " [Switch Platform Required]";
            if (GUILayout.Button(androidButtonText))
            {
                BuildSEParty(SEParty_AppType.GameClient, BuildTargetGroup.Android, false, "SEParty");
            }

            // Create Build for iOS button
            string iOSButtonText = "Build Game Client (iOS)";
            if (currentBuildTarget != BuildTarget.iOS)
                iOSButtonText += " [Switch Platform Required]";
            if (GUILayout.Button(iOSButtonText))
            {
                BuildSEParty(SEParty_AppType.GameClient, BuildTargetGroup.iOS, true, "SEParty_XCode");
            }
        }

        void OnDestroy()
        {
            //WriteEditorPrefs(appType);
            UpdateBuildConfig(appType);
        }

        private void UpdateBuildConfig(SEParty_AppType app)
        {
            string appType = app.ToString();
            bool configUpdated = false;
            if (buildConfigs == null)
            {
                Debug.Log("buildConfigs is null");
            }
            else
            {
                foreach (BuildConfig configIteration in buildConfigs.Configs)
                {
                    if (configIteration.AppType == appType)
                    {
                        // Set Details
                        configIteration.ConfigData = CreateConfigData();
                        configUpdated = true;
                        break;
                    }
                }
            }

            // No Config yet
            if (!configUpdated)
            {
                BuildConfig config = new BuildConfig();
                config.AppType = appType;
                config.ConfigData = CreateConfigData();

                BuildConfig[] temp = (BuildConfig[])buildConfigs.Configs.Clone();
                buildConfigs.Configs = new BuildConfig[temp.Length + 1];
                Array.Copy(temp, buildConfigs.Configs, temp.Length);
                buildConfigs.Configs[temp.Length] = config;
            }

            // Write to config file
            string configString = JsonUtility.ToJson(buildConfigs);
            string path = buildConfigFilepath;
            System.IO.File.WriteAllText(path, configString);
            // Refresh Assets
            AssetDatabase.Refresh();
        }

        private ConfigData CreateConfigData()
        {
            ConfigData data = new ConfigData();
            data.AppName = appString;
            data.Company = companyString;
            data.PackageName = packageName;

            // Set App Icon
            string appIconPath = AssetDatabase.GetAssetPath(appIcon);
            data.AppIconFilePath = appIconPath;

            // Set Scenes
            data.NumScenes = numScenes;
            data.ScenePaths = new string[numScenes];
            for (int i = 0; i < numScenes; i++)
            {
                string scenePath;

                // Get relative directory of Scene
                if (sceneAssets[i] == null)
                    scenePath = "";
                else
                    scenePath = AssetDatabase.GetAssetPath(sceneAssets[i]);

                // Save the directory
                data.ScenePaths[i] = scenePath;
            }

            return data;
        }

        private void GUIAdminClient()
        {
            /*
             * Windows Standalone
             */
            // Set Application details & Scenes
            DrawGUILayouts();

            // Create Build button
            string buttonText = "Build Admin Client (Windows Standalone)";
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows)
                buttonText += " [Switch Platform Required]";
            if (GUILayout.Button(buttonText))
            {
                BuildSEParty(SEParty_AppType.AdminClient, BuildTargetGroup.Standalone, true, "SEParty_Admin");
            }
        }

        private void BuildSEParty(SEParty_AppType appType, BuildTargetGroup buildTargetGroup, bool saveToFolder, string defaultBuildName)
        {
            // Get previous saved folder path
            string appTypePrefix = appType.ToString() + "_" + buildTargetGroup.ToString();

            string path;
            if (saveToFolder)   // for Standalone and iOS (XCode)
            {
                string folderPath = EditorPrefs.GetString(appTypePrefix + saveFolderPath_Key, "");
                path = EditorUtility.SaveFolderPanel("Choose Location for Built Game", folderPath, defaultBuildName);
            }
            else    // for Android (APK)
            {
                string filePath = EditorPrefs.GetString(appTypePrefix + saveFolderPath_Key, "");
                path = EditorUtility.SaveFilePanel("Choose Location for Built Game", filePath, defaultBuildName, "apk");
            }

            if (buildTargetGroup == BuildTargetGroup.Standalone)    // build an exe inside the selected folder for Standalone
            {
                string[] directories = path.Split('/');
                path += "/" + directories[directories.Length-1] + ".exe";
            }

            if (path.Length == 0)   // dialog was cancelled
                return;
            EditorPrefs.SetString(appTypePrefix + saveFolderPath_Key, path);

            List<string> levels = new List<string>();
            foreach (Object sceneObj in sceneAssets)
            {
                if (sceneObj != null)
                    levels.Add(AssetDatabase.GetAssetPath(sceneObj));
            }

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = levels.ToArray();
            buildPlayerOptions.locationPathName = path;
            buildPlayerOptions.target = BuildTargetGroupToBuildTarget(buildTargetGroup);
            buildPlayerOptions.options = BuildOptions.None;

            // NOTE:
            // A custom Application Identifier (package namespace) needs to be set for different builds
            PlayerSettings.SetApplicationIdentifier(buildTargetGroup, packageName);

            // Set default app icon via Unknown build target
            Texture2D[] iconsDefault = { appIcon };
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, iconsDefault);

            // Refresh Assets
            AssetDatabase.Refresh();

            // Build player
            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }

        private void DrawGUILayouts()
        {
            // Set Application Name
            companyString = EditorGUILayout.TextField("Company Name", companyString);
            appString = EditorGUILayout.TextField("Product Name", appString);
            // Set package name
            packageName = EditorGUILayout.TextField("Package Name", packageName);

            // Set App Icon (Texture2D)
            //GUILayout.Label("Default Icon", EditorStyles.label);
            appIcon = (Texture2D)EditorGUILayout.ObjectField("Default Icon", appIcon, typeof(Texture2D), false);

            // Get number of Scenes
            int newNumScenes = EditorGUILayout.IntField("Scene Size", numScenes);
            // Set Scenes
            if (newNumScenes != numScenes && newNumScenes >= 0)
            {
                Object[] temp;
                if (sceneAssets != null)
                    temp = (Object[])sceneAssets.Clone();
                else
                    temp = new Object[newNumScenes];

                sceneAssets = new Object[newNumScenes];

                for (int i = 0; i < newNumScenes; i++)
                {
                    if (i < numScenes)
                    {
                        sceneAssets[i] = EditorGUILayout.ObjectField("Scene " + i, temp[i], typeof(Object), false);
                    }
                    else
                    {
                        sceneAssets[i] = EditorGUILayout.ObjectField("Scene " + i, sceneAssets[i], typeof(Object), false);
                    }
                }

                numScenes = newNumScenes;
            }
            else
            {
                for (int i = 0; i < newNumScenes; i++)
                {
                    sceneAssets[i] = EditorGUILayout.ObjectField("Scene " + i, sceneAssets[i], typeof(Object), false);
                }
            }

        }

        private BuildTarget BuildTargetGroupToBuildTarget(BuildTargetGroup buildTargetGroup)
        {
            switch (buildTargetGroup)
            {
                case BuildTargetGroup.Android:
                    return BuildTarget.Android;
                case BuildTargetGroup.iOS:
                    return BuildTarget.iOS;
                case BuildTargetGroup.Standalone:
                    return BuildTarget.StandaloneWindows;

                default:
                    return EditorUserBuildSettings.activeBuildTarget;
            }
        }
    }

}