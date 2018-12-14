using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MyAssetBundleEditor
{
    public class BuildingAssetBundles
    {
        public static void BuildAssetBundles(List<AssetBundleBuild> builds)
        {
            string output = BuildDefaultPath.CreateAssetBundleDirectory();


       
            if (!Directory.Exists(output))
                Directory.CreateDirectory(output);

            BuildAssetBundleOptions options = BuildAssetBundleOptions.None;

            if (builds == null || builds.Count == 0)
            {
                Debug.LogError("BuildAssetBundles is Called. But builds == null || builds.Count == 0 !   Check!!!");
            }
            else
            {
                BuildPipeline.BuildAssetBundles(output, builds.ToArray(), options,
                    EditorUserBuildSettings.activeBuildTarget);
            }
        }

    }
}

