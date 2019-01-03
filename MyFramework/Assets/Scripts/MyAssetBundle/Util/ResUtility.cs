
using UnityEngine;
using System.IO;
using System;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResUtility
{
    public const string AssetBundlesOutputPath = "AssetBundles";

    //下载地址
    public static string WebUrl = "http://192.168.93.230:8081/StreamingAssets/";

    public static string GetPlatformPath
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

    public static string GetDataPathByPlatform
    {
        get
        {
            if (Application.isMobilePlatform)
            {
                return string.Format("{0}/{1}/{2}/", Application.persistentDataPath, AssetBundlesOutputPath, GetPlatformPath);
            }
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return string.Format("{0}/{1}/{2}/", Application.streamingAssetsPath, AssetBundlesOutputPath, GetPlatformPath);
            }
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                int i = Application.dataPath.LastIndexOf('/');
                return string.Format("{0}{1}/{2}/", Application.dataPath.Substring(0, i + 1),
                    Application.streamingAssetsPath, GetPlatformPath);
            }

            return Path.Combine(Path.Combine(System.Environment.CurrentDirectory, AssetBundlesOutputPath),
                (GetPlatformPath + "/")).Replace('\\', '/');
        }

    }

    static string GetPlatformForAssetBundles(RuntimePlatform platform)
    {
        if (platform == RuntimePlatform.Android)
        {
            return "Android";
        }

        if (platform == RuntimePlatform.IPhonePlayer)
        {
            return "iOS";
        }

        if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor)
        {
            return "Windows";
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

    public static string ReadFile(string path)
    {
        try
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string content = string.Empty;
            string outString = "";
            while ((content = sr.ReadLine()) != null)
            {
                outString += content + "\n";
            }
            sr.Close();
            return outString;
        }
        catch(Exception e)
        {
            MyDebug.LogFormat("ReadFile is Call.But Path is Null. Path:{0},err:{1},",path,e);
            return "";
        }
    }

    public static string GetPlatformResourcesPath
    {
        get
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return Application.dataPath + "!assets/";
                case RuntimePlatform.IPhonePlayer:
                    return Application.dataPath + "/Raw/";
                default:
                    return Application.dataPath + "/Raw/";
            }
        }
    }
}

