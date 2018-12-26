using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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


    }
}

