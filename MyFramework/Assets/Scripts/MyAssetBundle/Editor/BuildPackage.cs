using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyAssetBundleEditor
{
    public abstract class BuildPackage
    {
        protected static List<string> packedAssets = new List<string>();
        protected static List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
        
    }
}

