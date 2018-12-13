using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace MyAssetBundleEditor
{
    public class PackagePattern : ScriptableObject
    {
        [SerializeField]
        public List<BuildPackageInfo> packagInfos = new List<BuildPackageInfo>();

        [NonSerialized] private Dictionary<string, BuildPackageInfo> _cachedPackagInfos = new Dictionary<string, BuildPackageInfo>();

        public void MappingPackageData()
        {
            foreach (var info in packagInfos)
            {
                _cachedPackagInfos[info.assetName] = info;
            }
        }


        public BuildPackageInfo GetPackagInfoByAssetName(string assetName)
        {
            BuildPackageInfo pkgInfo = null;
            _cachedPackagInfos.TryGetValue(assetName,out pkgInfo);
            return pkgInfo;
        }

        public void RemovePackagInfoByAssetName(string assetName)
        {
            if (GetPackagInfoByAssetName(assetName) != null)
            {
                packagInfos.Remove(GetPackagInfoByAssetName(assetName));
                _cachedPackagInfos.Remove(assetName);
            }
        }

        public Dictionary<string, BuildPackageInfo> GetCacheAssetDataInfos()
        {
            return _cachedPackagInfos;
        }

        public void Clear()
        {
            packagInfos.Clear();
            _cachedPackagInfos.Clear();

        }
    }

    [Serializable]
    public class BuildPackageInfo
    {
        public string assetName;
        public string BuildType;
        public string searchPath;
        public string searchPattern;
        public SearchOption option;
        public string bundleName;
    }
}

