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
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            string outputFolder = ResUtility.GetPlatformPath;

            string source =
                Path.Combine(Path.Combine(System.Environment.CurrentDirectory, ResUtility.AssetBundlesOutputPath),
                    outputFolder);

            if (!Directory.Exists(source))
            {
                Debug.LogError("No assetBundle output folder, try to build the assetBundles first.");
                return;
            }

            string destination = Path.Combine(outputPath, outputFolder);
#if UNITY_EDITOR
            if (Directory.Exists(destination))
                FileUtil.DeleteFileOrDirectory(destination);

            FileUtil.CopyFileOrDirectory(source, destination);
#endif
        }

    }
}

