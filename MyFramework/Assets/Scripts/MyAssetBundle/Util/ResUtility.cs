﻿
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResUtility
{
    public const string AssetBundlesOutputPath = "AssetBundles";

    public static string GetPlatformName
    {
        get
        { 
#if UNITY_EDITOR
        
        return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
            return GetPlatformForAssetBundles(Application.platform);
#endif
        }
    }

    static string GetPlatformForAssetBundles(RuntimePlatform platform)
    {
        if (platform == RuntimePlatform.Android)
        {
            return Application.persistentDataPath + "/Android";
        }

        if (platform == RuntimePlatform.IPhonePlayer)
        {
            return Application.persistentDataPath + "/iOS";
        }

        if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor)
        {
            return Application.streamingAssetsPath + "/Windows";
        }

        return null;
    }

#if UNITY_EDITOR
    static string GetPlatformForAssetBundles(BuildTarget target)
    {
        if (target == BuildTarget.Android)
        {
            return "Android";
        }

        if (target == BuildTarget.iOS)
        {
            return "iOS";
        }

        if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
        {
            return "Windows";
        }

        return null;
    }
#endif
}

