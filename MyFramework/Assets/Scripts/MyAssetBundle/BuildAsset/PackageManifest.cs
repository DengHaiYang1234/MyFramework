using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyAssetBundleEditor
{
    public class PackageManifest : ScriptableObject
    {
        [SerializeField] public List<AssetManifestInfo> assetInfos = new List<AssetManifestInfo>();

    }

    [Serializable]
    public class AssetManifestInfo
    {
        public string AssetName;
        public List<string> Infos;
    }
}

