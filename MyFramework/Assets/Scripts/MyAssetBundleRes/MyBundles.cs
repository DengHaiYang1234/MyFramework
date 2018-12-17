using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Res
{
    public class MyBundles
    {

        public static string[] activeVariants { get; private set; }

        public static string dataPath { get; private set; }

        public static AssetBundleManifest manifest { get; private set; }

        internal static readonly Dictionary<string, MyBundle> bundles = new Dictionary<string, MyBundle>();

        public static bool Initialize(string path)
        {
            activeVariants = new string[0];
            dataPath = path;

            var request = 
        }


        static MyBundle LoadInternal(string assetBundleName, bool isLoadingAssetBundleManifest, bool asyncRequest)
        {
            if (!isLoadingAssetBundleManifest)
            {
                if (manifest == null)
                {
                    MyDebug.LogErrorFormat("Please initialize AssetBundleManifest by calling Bundles.Initialize()");
                    return null;
                }
                assetBundleName = RemapVariantName(assetBundleName);
            }

            var url = GetDataPath(assetBundleName) + assetBundleName;
            MyBundle bundle;
            if (!bundles.TryGetValue(assetBundleName, out bundle))
            {
                var hash = isLoadingAssetBundleManifest
                    ? new Hash128(1, 0, 0, 0)
                    : manifest.GetAssetBundleHash(assetBundleName);

                if (bundle == null)
                {
                    if (url.StartsWith("file://")
                    {
                        
                    }
                }
            }
        }

        static string RemapVariantName(string assetBundleName)
        {
            string[] bundlesWithVariant = manifest.GetAllAssetBundlesWithVariant();

            string baseName = assetBundleName.Split('.')[0];

            int bestFit = int.MaxValue;
            int bestFitIndex = -1;

            for (int i = 0; i < bundlesWithVariant.Length; i++)
            {
                string[] curSplit = bundlesWithVariant[i].Split('.');
                string curBaseName = curSplit[0];
                string curVariant = curSplit[1];

                if (curBaseName != baseName)
                {
                    continue;
                }

                int found = System.Array.IndexOf(activeVariants, curVariant);

                if (found == -1)
                    found = int.MaxValue - 1;

                if (found < bestFit)
                {
                    bestFit = found;
                    bestFitIndex = i;
                }
            }

            if (bestFit == int.MaxValue - 1)
            {
                MyDebug.LogFormat("Ambigious asset bundle variant chosen because there was no matching active variant: " + bundlesWithVariant[bestFitIndex]);
            }

            if (bestFitIndex != -1)
            {
                return bundlesWithVariant[bestFitIndex];
            }

            return assetBundleName;
        }

        public static string GetDataPath(string bundleName)
        {
            return dataPath;
        }
    }
}

