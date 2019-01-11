using System;
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
                try
                {
                    FileUtil.DeleteFileOrDirectory(output);
                }
                catch(Exception e)
                {
                    Debug.LogError("请关闭打开的AssetBundle文件" + e);
                }
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

        public static void BuildAssetBundles(List<AssetBundleBuild> builds)
        {
            string output = BuildDefaultPath.GetAssetBundleDirectory;

            if (Directory.Exists(ResUtility.AssetBundlesOutputPath))
            {
                try
                {
                    Directory.Delete(ResUtility.AssetBundlesOutputPath, true);
                }
                catch (Exception e)
                {
                    Debug.LogError("请关闭打开的AssetBundle文件" + e);
                }
            }

            Directory.CreateDirectory(output);

            //BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets |
            //                              BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle;

            //BuildAssetBundleOptions options = BuildAssetBundleOptions.None;

            BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;

            if (builds == null || builds.Count == 0)
            {
                Debug.LogError("BuildAssetBundles is Called. But builds == null || builds.Count == 0 !   检查错误!!!");
            }
            else
            {
                Debug.LogError("EditorUserBuildSettings.activeBuildTarget:" + EditorUserBuildSettings.activeBuildTarget);
                BuildPipeline.BuildAssetBundles(output, builds.ToArray(), options, EditorUserBuildSettings.activeBuildTarget);
                AfterBuild ab = new AfterBuild();
            }
        }
    }
}

