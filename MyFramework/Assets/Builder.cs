//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;

//public class Builder : MonoBehaviour
//{
//    //private readonly Object selectAssets = Selection.activeObject;
//    //private readonly string path = AssetDatabase.GetAssetPath(selectAssets);
//    public static string sourcePath;
//    const string AssetBundlesOutputPath = "StreamingAssets";

//    [MenuItem("Tools/AssetBundle/Build")]
//    public static void BuildAssetBundle()
//    {
//        Object selectAssets = Selection.activeObject;
//        string path = AssetDatabase.GetAssetPath(selectAssets);
//        path = path.Substring(path.IndexOf("/"));
//        sourcePath = Application.dataPath + path;
//        ClearAssetBundlesName();
//        Pack(sourcePath);
//        //Path.Combine:合并两个路径字符串
//        string outputPath = Path.Combine(AssetBundlesOutputPath, Platform.GetPlatformFolder(EditorUserBuildSettings.activeBuildTarget));
//        if (!Directory.Exists(outputPath))
//        {
//            Directory.CreateDirectory(outputPath);
//        }
//        //根据BuildSetting里面所激活的平台进行打包 
//        BuildPipeline.BuildAssetBundles(outputPath, 0, EditorUserBuildSettings.activeBuildTarget);
//        AssetDatabase.Refresh();
//    }
//    /// <summary> 
//    /// 清除之前设置过的AssetBundleName，避免产生不必要的资源也打包 
//    /// 之前说过，只要设置了AssetBundleName的，都会进行打包，不论在什么目录下 
//    /// </summary> 
//    static void ClearAssetBundlesName()
//    {
//        int length = AssetDatabase.GetAllAssetBundleNames().Length;
//        string[] oldAssetBundleNames = new string[length];
//        for (int i = 0; i < length; i++)
//        {
//            oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
//        }
//        for (int j = 0; j < oldAssetBundleNames.Length; j++)
//        {
//            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
//        }
//        length = AssetDatabase.GetAllAssetBundleNames().Length;
//    }



//    static void Pack(string source)
//    {
//        DirectoryInfo folder = new DirectoryInfo(source); //拿到文件夹目录
//        FileSystemInfo[] files = folder.GetFileSystemInfos(); //目录下的文件及文件夹
//        int length = files.Length; // 有几个文件夹或资源
//        for (int i = 0; i < length; i++)
//        {
//            if (files[i] is DirectoryInfo) //如果是文件夹目录就递归寻找该目录下的资源
//            {
//                Pack(files[i].FullName);
//            }
//            else
//            {
//                if (!files[i].Name.EndsWith(".meta")) //如果不是文件夹，是资源的话，开始命名
//                {
//                    file(files[i].FullName); //文件的完整名称
//                }
//            }
//        }
//    }
//    static void file(string source)
//    {
//        string _source = Replace(source);
//        string _assetPath = "Assets" + _source.Substring(Application.dataPath.Length);

//        string _assetPath2 = _source.Substring(Application.dataPath.Length + 1);
//        //在代码中给资源设置AssetBundleName 
//        AssetImporter assetImporter = AssetImporter.GetAtPath(_assetPath);
//        string assetName = _assetPath2.Substring(_assetPath2.IndexOf("/") + 1);
//        //Path.GetExtension 返回指定路径字符串的扩展名
//        assetName = assetName.Replace(Path.GetExtension(assetName), ".unity3d");
//        assetImporter.assetBundleName = assetName;
//    }



//    static string Replace(string s)
//    {
//        return s.Replace("\\", "/");  //将\替换为//
//    }
//}
//public class Platform
//{
//    public static string GetPlatformFolder(BuildTarget target)
//    {
//        switch (target)
//        {
//            case BuildTarget.Android:
//                return "Android";
//            case BuildTarget.iOS:
//                return "IOS";
//            case BuildTarget.StandaloneWindows:
//            case BuildTarget.StandaloneWindows64:
//                return "PC";
//            case BuildTarget.StandaloneOSXIntel:
//            case BuildTarget.StandaloneOSXIntel64:
//            case BuildTarget.StandaloneOSXUniversal:
//                return "OSX";
//            default:
//                return null;
//        }
//    }

//}
