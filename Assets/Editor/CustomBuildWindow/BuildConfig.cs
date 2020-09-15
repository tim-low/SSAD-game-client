using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomBuildSettings
{
    [System.Serializable]
    public class BuildConfigs
    {
        public BuildConfig[] Configs;
    }

    [System.Serializable]
    public class BuildConfig
    {
        public string AppType;
        public ConfigData ConfigData;
    }

    [System.Serializable]
    public class ConfigData
    {
        public string AppName = "SE Party";
        //private const string AppString_Key = "AppString";
        public string Company = "SuperSad";
        //private const string companyString_Key = "CompanyString";
        public string PackageName = "com.supersad.SEParty";
        //private const string packageName_Key = "PackageName";
        public string AppIconFilePath;
        //private const string appIconPath_Key = "AppIconFilePath";
        public int NumScenes;
        //private const string numScenes_Key = "NumberScenes";
        public string[] ScenePaths;
        //private const string sceneAsset_KeyPrefix = "Scene";

        //private const string saveFolderPath_Key = "SaveFolder";
    }
}