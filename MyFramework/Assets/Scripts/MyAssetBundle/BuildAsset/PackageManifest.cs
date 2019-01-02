using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyAssetBundleEditor
{
    public class PackageManifest : ScriptableObject
    {
        [SerializeField] public List<AssetManifestInfo> assetInfos = new List<AssetManifestInfo>();
        [NonSerialized] //key:bundle   value:assets
        private static Dictionary<string, List<string>> _cachedBundleInfos = new Dictionary<string, List<string>>();
        [NonSerialized] //key:bundleName   value:bundle
        private static Dictionary<string, string> _cachedBundleName = new Dictionary<string, string>();
        [NonSerialized] //key:assetName   value:assetsPath
        private static Dictionary<string, string> _caheAssetInfos = new Dictionary<string, string>();
        [NonSerialized] //key:assetName   value:bundle
        private static Dictionary<string, string> _caheAssetBundleInfos = new Dictionary<string, string>();

        public void MapingAssetData()
        {
            _cachedBundleInfos.Clear();
            _cachedBundleName.Clear();
            _caheAssetInfos.Clear();
            _caheAssetBundleInfos.Clear();

            foreach (var asset in assetInfos)
            {
                if (!_cachedBundleInfos.ContainsKey(asset.bundle))
                {
                    _cachedBundleInfos[asset.bundle] = asset.assets;
                }
                else
                {
                    Debug.LogError(string.Format("PackageManifest【Bundle】存在重复！！！ {0}", asset.bundle));
                }

                if (!_cachedBundleName.ContainsKey(asset.name))
                {
                    _cachedBundleName[asset.name] = asset.bundle;
                }
                else
                {
                    Debug.LogError(string.Format("PackageManifest【name】存在重复！！！ {0}", asset.name));
                }

                foreach (var assetPath in asset.assets)
                {
                    string str = assetPath.Substring(0, assetPath.LastIndexOf('.'));
                    string itemName = str.Substring(str.LastIndexOf('/') + 1).ToLower();
                    if(itemName.IndexOf('.') != -1)
                        itemName = itemName.Substring(0, itemName.LastIndexOf('.'));

                    _caheAssetInfos[itemName] = assetPath;
                    _caheAssetBundleInfos[itemName] = asset.bundle;
                }
            }
        }

        public  Dictionary<string, List<string>> GetManifestDic()
        {
            if (_cachedBundleInfos.Count == 0)
            {
                Debug.LogError("PackageManifest为空！！ 请检查！");
                return null;
            }
            return _cachedBundleInfos;
        }

        public  Dictionary<string, string> GetManifestNameDic()
        {
            if (_cachedBundleName.Count == 0)
            {
                Debug.LogError("PackageManifest为空！！ 请检查！");
                return null;
            }
            return _cachedBundleName;
        }

        public  Dictionary<string, string> GetManifestAssetDic()
        {
            if (_caheAssetInfos.Count == 0)
            {
                Debug.LogError("PackageManifest为空！！ 请检查！");
                return null;
            }
            return _caheAssetInfos;
        }

        public  Dictionary<string, string> GetManifestAssetBundleDic()
        {
            if (_caheAssetBundleInfos.Count == 0)
            {
                Debug.LogError("PackageManifest为空！！ 请检查！");
                return null;
            }
            return _caheAssetBundleInfos;
        }
    }

    [Serializable]
    public class AssetManifestInfo
    {
        public string name;
        public string bundle;
        public List<string> assets;
    }
}

