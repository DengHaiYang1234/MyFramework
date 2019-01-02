using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MyAssetBundleEditor
{
    public class BuildingAssetBundles
    {
        public static void BuildAssetBundles(List<AssetBundleBuild> builds,BuildTarget target)
        {
            string output = BuildDefaultPath.GetAssetBundleDirectory;

            if (Directory.Exists(output))
            {
                Directory.Delete(output, true);
            }

            Directory.CreateDirectory(output);

            BuildAssetBundleOptions options = BuildAssetBundleOptions.None;

            if (builds == null || builds.Count == 0)
            {
                Debug.LogError("BuildAssetBundles is Called. But builds == null || builds.Count == 0 !   检查错误!!!");
            }
            else
            {
                BuildPipeline.BuildAssetBundles(output, builds.ToArray(), options,target);
                AfterBuild ab = new AfterBuild();
            }
        }
    }
}

