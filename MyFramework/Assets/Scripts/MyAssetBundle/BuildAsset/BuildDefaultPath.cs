using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyAssetBundleEditor
{
    public class BuildDefaultPath
    {
        public const string assetPath = "Assets";
        public const string dataPath = "data";

        public const string assetsManifestFloder = "[Mainfest]";
        public const string assetsBuildMethodFloder = "[BuildMethod]";
        public const string assetsAtlasFloder = "[UIAtlas]";
        public const string assetsPrefabFloder = "[UIPrefab]";

        public const string assetSuffix = ".asset";
        public const string bundleName = ".assetbundle";

        public const string assetBuildMethodName = "BuildMethod";

        public const string BuildAssetsWithAssetBundleName = "BuildAssetsWithAssetBundleName"; //粒度大。适用于无依赖的且体积小的资源(适用于需动态加载的资源)
        public const string BuildAssetsWithDirectroyName = "BuildAssetsWithDirectroyName";//最大粒度打包.直接按文件夹打包
        public const string BuildAssetsWithFilename = "BuildAssetsWithFilename";//小粒度.适用于prefab打包。  将prefab与其依赖的资源共同打包


        public static string GetBuildMethodFloderPath()
        {
            return string.Format("{0}/{1}/{2}/{3}{4}", assetPath, dataPath, assetsBuildMethodFloder, assetBuildMethodName, assetSuffix);
        }
    }
}

