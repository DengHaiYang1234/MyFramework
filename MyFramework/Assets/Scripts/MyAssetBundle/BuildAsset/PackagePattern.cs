using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace MyAssetBundleEditor
{
    /// <summary>
    /// 构造打包数据
    /// </summary>
    public class PackagePattern : ScriptableObject
    {
        [SerializeField]
        public List<BuildPackageInfo> packagInfos = new List<BuildPackageInfo>();

        [NonSerialized] private Dictionary<string, BuildPackageInfo> _cachedPackagInfos = new Dictionary<string, BuildPackageInfo>();

        public void MappingPackageData()
        {
            foreach (var info in packagInfos)
            {
                _cachedPackagInfos[info.assetFolderName] = info;
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
        public string assetFolderName;
        public BuildType BuildType;
        public string searchPath;
        public string searchPattern;
        public SearchOption searchOption;
        public string bundleName;
    }
}

