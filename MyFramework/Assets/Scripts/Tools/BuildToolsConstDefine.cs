using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Res;



public static class BuildToolsConstDefine
{
    public const string RootPath = "Assets";
    public const string ResRootPath = "data";

    public const string ExportDependsAtlasTextureAssetFolder = "_source";
    public const string AssetRootFolder = "Assets";
    public const string AssetSuffix = ".asset";
    public const string PrefabSuffix = ".prefab";
    public const string BundleName = "assetbundle";
    public const string DataAtlasPath = "Assets/data/[UIAtlas]/";

    public const string BuildDataFolder = "data/";
    public const string BuildAtlasFolder = "atlas";
    public const string BuildUIPrefabFolder = "uiprefab";
    public const string BuildMainfestFolder = "mainfest";


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
            case EResType.Mainfest:
                path = BuildMainfestFolder;
                break;
        }
        return string.Format("{0}{1}", BuildDataFolder, path);
    }


    public static string GetBuildingAssetPath(EResType type)
    {
        string resPath = GetBuildingFolderByResType(type);
        return string.Format("{0}/{1}", AssetRootFolder, resPath);
    }


    public static string GetBuildAssetDirPath(EResType type,string path)
    {
        string _path = "";
        switch (type)
        {
            case EResType.Atlas:
                _path = path.Replace(DataAtlasPath, "");
                _path = "/" + _path.Substring(0, _path.IndexOf('/'));
                break;
            case EResType.UIPrefab:
                _path = string.Empty;
                break;
        }

        return GetBuildingFolderByResType(type) + _path;


        switch (type)
        {
            case EResType.Atlas:
                path = BuildAtlasFolder;
                break;
            case EResType.UIPrefab:
                path = BuildUIPrefabFolder;
                break;
        }
    }




}
