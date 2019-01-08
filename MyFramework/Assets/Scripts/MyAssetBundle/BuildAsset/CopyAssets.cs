using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace MyAssetBundleEditor
{
    public class CopyAssets
    {
        public CopyAssets(string outputPath)
        {
            if (Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.Delete(Application.streamingAssetsPath,true);
            }

            Directory.CreateDirectory(outputPath);

            string source =
                Path.Combine(ResUtility.AssetBundlesOutputPath, ResUtility.GetPlatformPath).Replace('\\', '/');

            if (!Directory.Exists(source))
            {
                Debug.LogError("No assetBundle output folder, try to build the assetBundles first.");
                return;
            }

            string destination = Path.Combine(outputPath, ResUtility.GetPlatformPath).Replace('\\', '/');
            string[] sources = {source};
            CopyFiles.Copy(destination, sources);
        }
    }
}

