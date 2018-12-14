using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace MyAssetBundleEditor
{
    public class BuildDefaultPath
    {
        public const string assetPath = "Assets";
        public const string dataPath = "data";
        public const string AssetBundleOutputPath = "AssetBundles";

        public const string assetsManifestFloder = "Manifest";
        public const string assetsBuildMethodFloder = "BuildPattern";
        public const string assetsAtlasFloder = "[UIAtlas]";
        public const string assetsPrefabFloder = "[UIPrefab]";

        public const string assetSuffix = ".asset";
        public const string bundleName = ".assetbundle";

        public const string assetBuildMethodName = "buildPattern";
        public const string assetManifestName = "manifest";

        public const string BuildAssetsWithAssetBundleName = "BuildAssetsWithAssetBundleName"; //粒度大。适用于无依赖的且体积小的资源(适用于需动态加载的资源)
        public const string BuildAssetsWithDirectroyName = "BuildAssetsWithDirectroyName";//最大粒度打包.直接按文件夹打包
        public const string BuildAssetsWithFilename = "BuildAssetsWithFilename";//小粒度.适用于prefab打包。  将prefab与其依赖的资源共同打包

        public static string GetAssetDataPath()
        {
            return string.Format("{0}/{1}/",assetPath,dataPath);
        }

        public static string GetBuildPattrenAssetPath()
        {
            return string.Format("{0}/{1}/{2}/{3}{4}", assetPath, dataPath, assetsBuildMethodFloder, assetBuildMethodName, assetSuffix);
        }

        public static string GetManifestAssetPath()
        {
            return string.Format("{0}/{1}/{2}/{3}{4}", assetPath, dataPath, assetsManifestFloder, assetManifestName, assetSuffix);
        }

        public static string BuildAssetBunldNameWithAssetPath(string assetPath)
        {
            return
                Path.Combine(Path.GetDirectoryName(assetPath), Path.GetFileNameWithoutExtension(assetPath))
                    .Replace('\\', '/')
                    .ToLower();
        }

        public static string CreateAssetBundleDirectory()
        {
            string outputPath = Path.Combine(AssetBundleOutputPath, ResUtility.GetPlatformName());

            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);

            return outputPath;
        }
    }
}

