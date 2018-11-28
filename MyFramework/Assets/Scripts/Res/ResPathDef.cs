using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Res
{
    public class ResPathDef
    {
        public const string RootPath = "Assets";
        public const string ResRootPath = "data";
        public const string AssetSufix = ".asset";
        public const string stream = "/StreamingAssets/";

        public const string ExportMainfestAssetName = "ResMainfest";
        public const string AssetMainfestFolder = "mainfest";

        public const string AssetBundleSufix = ".assetbundle";
        public const string PrefabSuffix = ".prefab";



        public const string ResUGUIAtlasPackTag = "[UIAtlas]";
        public const string ResUGUIPrefabsPackTag = "[UIPrefab]";
        public const string ResMainfestPackTag = "[Mainfest]";



        public static string GetRootResAssetPath()
        {
            return string.Format("{0}/{1}",RootPath,ResRootPath);
        }

        public static string GetMainfestAssetPath()
        {
            return string.Format("{0}/{1}/{2}/{3}{4}", RootPath, ResRootPath, ResMainfestPackTag,ExportMainfestAssetName, AssetSufix);
        }
    
        public static string GetAssetBunldeSuffix(EResType type)
        {
            switch (type)
            {
                case EResType.UIPrefab:
                    return PrefabSuffix;
            }

            return AssetSufix;
        }

        public static string GetLoadAssetBundleName(string path,EResType type)
        {
            if (path == null)
            {
                SDDebug.LogErrorFormat("GetLoadAssetBundleName is called . but path:{0} is null!",path);
                return null;
            }

            string name = path.Substring(path.LastIndexOf('/') + 1);
            name = name.Replace(AssetBundleSufix,GetAssetBunldeSuffix(type));
            return name.ToLower();
        }

    }

}
