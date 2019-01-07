using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MyAssetBundleEditor
{
    public class MyAssetBundleMenu
    {
        [MenuItem("Assets/CheckBuildPattern/ShowOrUpdateBuildPattern", false, 6)]
        public static void GetOrUpdatePattern()
        {
            if (EditorApplication.isCompiling)
            {
                Debug.LogError("代码正在编译中，请重试.....");
                return;
            }

            BuildPackPattern method = new BuildPackPattern();
        }


        [MenuItem("Assets/CheckBuildPattern/ClearBuildPattern", false,7)]
        public static void ClearPackageMethod()
        {
            if (EditorApplication.isCompiling)
            {
                Debug.LogError("代码正在编译中，请重试.....");
                return;
            }
            BuildPackPattern method = new BuildPackPattern(true);
        }


        [MenuItem("Assets/BuildManifest", false, 10)]
        public static void BuildManifest()
        {
            if (EditorApplication.isCompiling)
            {
                Debug.LogError("代码正在编译中，请重试.....");
                return;
            }

            List<AssetBundleBuild> builds = BaseBuild.GetBuilds();
            if (builds == null)
            {
                Debug.LogError("manifest打包失败。请检查错误!");
            }
            else
            {
                BuildMainfest manifest = new BuildMainfest(builds);
            }
        }


        [MenuItem("MyAssetsBundle/BuildAll", false, 105)]
        public static void BuildAssetBundles()
        {
            if (EditorApplication.isCompiling)
            {
                Debug.LogError("代码正在编译中，请重试.....");
                return;
            }
            //添加资源
            List<AssetBundleBuild> builds = BaseBuild.GetBuilds();

            BuildMainfest manifest = new BuildMainfest(builds);
            if (manifest.BuildManifestIsSuccess && builds != null)
            {
                BuildingAssetBundles.BuildAssetBundles(builds);
            }
            else
            {
                Debug.LogError("manifest打包失败。请检查错误!");
            }
        }

        //[MenuItem("MyAssetsBundle/BuildAllAssetBundles/Windows", false, 105)]
        //public static void BuildWindowsAssetBundles()
        //{
        //    if (EditorApplication.isCompiling)
        //    {
        //        Debug.LogError("代码正在编译中，请重试.....");
        //        return;
        //    }
        //    //添加资源
        //    List<AssetBundleBuild> builds = BaseBuild.GetBuilds();

        //    BuildMainfest manifest = new BuildMainfest(builds);
        //    if (manifest.BuildManifestIsSuccess && builds != null)
        //    {
        //        BuildingAssetBundles.BuildAssetBundles(builds,BuildTarget.StandaloneWindows);
        //    }
        //    else
        //    {
        //        Debug.LogError("manifest打包失败。请检查错误!");
        //    }
        //}

        //[MenuItem("MyAssetsBundle/BuildAllAssetBundles/Android", false, 105)]
        //public static void BuildAndroidAssetBundles()
        //{
        //    if (EditorApplication.isCompiling)
        //    {
        //        Debug.LogError("代码正在编译中，请重试.....");
        //        return;
        //    }
        //    //添加资源
        //    List<AssetBundleBuild> builds = BaseBuild.GetBuilds();

        //    BuildMainfest manifest = new BuildMainfest(builds);
        //    if (manifest.BuildManifestIsSuccess && builds != null)
        //    {
        //        BuildingAssetBundles.BuildAssetBundles(builds, BuildTarget.Android);
        //    }
        //    else
        //    {
        //        Debug.LogError("manifest打包失败。请检查错误!");
        //    }
        //}

        //[MenuItem("MyAssetsBundle/BuildAllAssetBundles/IOS", false, 105)]
        //public static void BuildIosAssetBundles()
        //{
        //    if (EditorApplication.isCompiling)
        //    {
        //        Debug.LogError("代码正在编译中，请重试.....");
        //        return;
        //    }

        //    //添加资源
        //    List<AssetBundleBuild> builds = BaseBuild.GetBuilds();

        //    BuildMainfest manifest = new BuildMainfest(builds);
        //    if (manifest.BuildManifestIsSuccess && builds != null)
        //    {
        //        BuildingAssetBundles.BuildAssetBundles(builds, BuildTarget.iOS);
        //    }
        //    else
        //    {
        //        Debug.LogError("manifest打包失败。请检查错误!");
        //    }
        //}

        [MenuItem("MyAssetsBundle/CopyAssetsToStreamingAssets", false, 105)]
        public static void CopyAssetsToStreamingAssets()
        {
            if (EditorApplication.isCompiling)
            {
                Debug.LogError("代码正在编译中，请重试.....");
                return;
            }

            CopyAssets ca = new CopyAssets(Path.Combine(Application.streamingAssetsPath,ResUtility.AssetBundlesOutputPath));
        }
    }
}



