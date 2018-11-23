using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Res;

public static class BuildToolsConstDefine
{
    public const string ExportDependsAtlasTextureAssetFolder = "_source";
    public const string AssetRootFolder = "Assets";
    public const string AssetSuffix = ".asset";
    public const string BundleName = "assetbundle";

    public const string BuildDataFolder = "data/";
    public const string BuildAtlasFolder = "atlas";
    public const string BuildUIPrefabFolder = "uiprefab";

    public static string GetBuildingFolderByResType(EResType type)
    {
        string path = "";
        switch (type)
        {
            case EResType.Atlas:
                path =  BuildAtlasFolder;
                break;
            case EResType.UIPrefab:
                path = BuildUIPrefabFolder;
                break;
        }
        return string.Format("{0}{1}", BuildDataFolder, path);
    }


    public static string GetBuildingAssetPath(EResType type)
    {
        string resPath = GetBuildingFolderByResType(type);
        return string.Format("{0}/{1}", AssetRootFolder, resPath);
    }
}
