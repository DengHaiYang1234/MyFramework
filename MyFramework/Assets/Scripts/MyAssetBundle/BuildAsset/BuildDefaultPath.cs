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

        public const string assetsManifestFloder = "ManifestDir";
        public const string assetsBuildMethodFloder = "BuildPattern";
        public const string assetsAtlasFloder = "[UIAtlas]";
        public const string assetsPrefabFloder = "[UIPrefab]";
        public const string assetsLuaFloder = "LuaScripts";


        public const string assetSuffix = ".asset";
        public const string bundleName = ".assetbundle";

        public const string assetBuildMethodName = "buildPattern";
        public const string assetManifestName = "manifest";

        public const string LuaScriptsDir = "LuaScripts";

        public const string BuildAssetsWithAssetBundleName = "BuildAssetsWithAssetBundleName";
        //粒度大。适用于无依赖的且体积小的资源(适用于需动态加载的资源)

        public const string BuildAssetsWithDirectroyName = "BuildAssetsWithDirectroyName"; //最大粒度打包.直接按文件夹打包
        public const string BuildAssetsWithFilename = "BuildAssetsWithFilename"; //小粒度.适用于prefab打包。  将prefab与其依赖的资源共同打包
        public const string BuildLuaAssets = "BuildLua"; //Lua 打包

        public const string LuaTempDir = "LuaTemp";

        public static string GetManifestAsset
        {
            get { return string.Format("{0}{1}", assetManifestName, assetSuffix); }
        }

        public static string GetAssetDataPath()
        {
            return string.Format("{0}/{1}/", assetPath, dataPath);
        }

        public static string GetLuaDataPath
        {
            get
            {
                return string.Format("{0}/{1}/", assetPath, assetsLuaFloder);
            }

        }

        public static string GetLuaTempDataPath
        {
            get
            {
                return string.Format("{0}/{1}/", assetPath, LuaTempDir);
            }
        }

        public static string GetToLuaDataPath
        {
            get
            {
                return string.Format("{0}/{1}/{2}/", assetPath, "ToLua", "Lua");
            }
        }

        public static string GetBuildLuaPath
        {
            get
            {
                return string.Format("{0}/{1}/{2}/{3}/", ResUtility.AssetBundlesOutputPath, ResUtility.GetPlatformPath,
                    assetPath.ToLower(), LuaTempDir.ToLower());
            }
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

        public static string GetAssetBundleDirectory
        {
            get
            {
                string outputPath = Path.Combine(ResUtility.AssetBundlesOutputPath, ResUtility.GetPlatformPath);
                return outputPath;
            }
        }

        public static string GetAssetBundlePath
        {
            get
            {
                return string.Format("{0}/{1}/", ResUtility.AssetBundlesOutputPath, ResUtility.GetPlatformPath);
            }
        }

        public static string BuildLuaAssetBundlePath
        {
            get
            {
                string luaPath = ResUtility.GetPlatformPath + "/assets/luascripts";
                return Path.Combine(ResUtility.AssetBundlesOutputPath, luaPath);
            }

        }
    }
}

