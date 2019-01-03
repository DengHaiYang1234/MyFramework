using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Res
{
    public class RuntimeResPath
    {
        public const string assetManifestName = "manifest";
        public const string assetPath = "Assets";
        public const string dataPath = "data";
        public const string assetSuffix = ".asset";
        public const string bundleName = ".assetbundle";
        public const string assetsManifestFloder = "ManifestDir";
        public const string assetsLuaFloder = "LuaScripts";
        public const string LuaTempDir = "LuaTemp";

        public static string GetManifestAssetPath
        {
            get
            { return string.Format("{0}/{1}/{2}/{3}{4}", assetPath, dataPath, assetsManifestFloder, assetManifestName, assetSuffix); }
        }

        public static string GetManifestAssetPathExceptSuffix
        {
            get
            {
                {
                    return string.Format("{0}/{1}/{2}/{3}", assetPath, dataPath, assetsManifestFloder, assetManifestName);
                }
            }
        }

        public static string GetLuaDataPath
        {
            get
            {
                return string.Format("{0}/{1}/", Application.dataPath, assetsLuaFloder);
            }
        }

        public static string GetToLuaDataPath
        {
            get
            {
                return string.Format("{0}/{1}/{2}/", Application.dataPath, "ToLua", "Lua");
            }
        }

        public static string GetAssetBundleDirectory
        {
            get
            {
                string outputPath = Path.Combine(ResUtility.AssetBundlesOutputPath, ResUtility.GetPlatformPath).Replace('\\','/');
                return outputPath;
            }
        }

        /// <summary>
        /// 获取本地文件路径
        /// </summary>
        public static string GetLocalDataPath
        {
            get
            {
                return string.Format("{0}", ResUtility.GetDataPathByPlatform);
            }
        }

        public static string GetLuaTempDataPath
        {
            get
            {
                return string.Format("{0}{1}/", GetLocalDataPath, LuaTempDir);
            }
        }
    }
}

