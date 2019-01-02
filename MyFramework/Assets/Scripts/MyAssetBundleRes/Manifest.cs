using System.Collections;
using System.Collections.Generic;
using MyAssetBundleEditor;
using UnityEngine;

namespace Res
{
    public class Manifest
    {
        private static Dictionary<string, string> assetsMaps = new Dictionary<string, string>();
        private static Dictionary<string, List<string>> bundleMaps = new Dictionary<string, List<string>>();
        private static Dictionary<string, string> bundleNameMaps = new Dictionary<string, string>();
        private static Dictionary<string, string> assetBundleName = new Dictionary<string, string>();

        private bool isInit = false;

        public bool IsInit
        {
            get { return isInit; }
            set { isInit = value; }

        }
        
        void Init()
        {
            assetsMaps.Clear();
            bundleMaps.Clear();
            bundleNameMaps.Clear();
            assetBundleName.Clear();

            isInit = true;
        }
        
        public void Load(PackageManifest manifestAsset)
        {
            if (manifestAsset == null)
            {
                MyDebug.LogError("Manifest初始化失败！manifestAsset == null");
                return;
            }

            Init();
            assetsMaps = manifestAsset.GetManifestAssetDic();
            bundleMaps = manifestAsset.GetManifestDic();
            bundleNameMaps = manifestAsset.GetManifestNameDic();
            assetBundleName = manifestAsset.GetManifestAssetBundleDic();
        }

        /// <summary>
        /// 获取BundleName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetBundleName(string name)
        {
            string bundleName = "";
            if (bundleNameMaps.TryGetValue(name, out bundleName))
            {
                return bundleName;
            }
            else
                return "";
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <returns></returns>
        public string GetAssetByName(string name)
        {
            string assetPath = "";
            if (assetsMaps.TryGetValue(name, out assetPath))
            {
                return assetPath;
            }
            else
                return "";
        }

        /// <summary>
        /// 根据资源名获取所在bunlde
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetBundleByAssetName(string name)
        {
            string bundle = "";
            if (assetBundleName.TryGetValue(name, out bundle))
            {
                return bundle;
            }
            else
                return "";
        }
    }
}

