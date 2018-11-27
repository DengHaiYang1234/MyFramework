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

    }

}
