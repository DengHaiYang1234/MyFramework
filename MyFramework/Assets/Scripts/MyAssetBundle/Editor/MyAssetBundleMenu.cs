using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyAssetBundleEditor
{
    public class MyAssetBundleMenu
    {
        //[MenuItem("Assets/CheckPackagingMethod/BuildAssetsWithFilename",false,4)]
        //public static void SetManifestWithBuildAssetsWithFilename()
        //{
        //    if (EditorApplication.isCompiling)
        //    {
        //        return;
        //    }
        //    BuildPackPattern method = new BuildPackPattern(Selection.activeObject,BuildType.BuildAssetsWithFilename);
        //}

        //[MenuItem("Assets/CheckPackagingMethod/BuildAssetsWithDirectroyName",false,5)]
        //public static void SetManifestWithBuildAssetsWithDirectroyName()
        //{
        //    if (EditorApplication.isCompiling)
        //    {
        //        return;
        //    }
        //    BuildPackPattern method = new BuildPackPattern(Selection.activeObject, BuildType.BuildAssetsWithDirectroyName);
        //}

        //[MenuItem("Assets/CheckPackagingMethod/BuildAssetsWithAssetBundleName",false,6)]
        //public static void SetManifestWithBuildAssetsWithAssetBundleName()
        //{
        //    if (EditorApplication.isCompiling)
        //    {
        //        return;
        //    }
        //    BuildPackPattern method = new BuildPackPattern(Selection.activeObject, BuildType.BuildAssetsWithAssetBundleName);
        //}

        [MenuItem("Assets/CheckBuildPattern/ShowOrUpdateBuildPattern", false, 6)]
        public static void GetOrUpdatePattern()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }

            BuildPackPattern method = new BuildPackPattern();
        }

        [MenuItem("Assets/CheckBuildPattern/ClearBuildPattern", false,7)]
        public static void ClearPackageMethod()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }
            BuildPackPattern method = new BuildPackPattern(true);
        }

        [MenuItem("Assets/BuildManifest", false, 10)]
        public static void BuildManifest()
        {
            if (EditorApplication.isCompiling)
            {
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


        [MenuItem("MyAssetsBundle/BuildAllAssetBundles/Windows", false, 105)]
        public static void BuildWindowsAssetBundles()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }
            //添加Lua
            //BuildLuaAssetBundles.BuildLuaResource(BuildTarget.StandaloneWindows);
            //添加资源
            List<AssetBundleBuild> builds = BaseBuild.GetBuilds();

            BuildMainfest manifest = new BuildMainfest(builds);
            if (manifest.BuildManifestIsSuccess && builds != null)
            {
                BuildingAssetBundles.BuildAssetBundles(builds,BuildTarget.StandaloneWindows);
            }
            else
            {
                Debug.LogError("manifest打包失败。请检查错误!");
            }
        }

        [MenuItem("MyAssetsBundle/BuildAllAssetBundles/Android", false, 105)]
        public static void BuildAndroidAssetBundles()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }
            //添加Lua
            BuildLuaAssetBundles.BuildLuaResource(BuildTarget.StandaloneWindows);
            //添加资源
            List<AssetBundleBuild> builds = BaseBuild.GetBuilds();

            BuildMainfest manifest = new BuildMainfest(builds);
            if (manifest.BuildManifestIsSuccess && builds != null)
            {
                BuildingAssetBundles.BuildAssetBundles(builds, BuildTarget.Android);
            }
            else
            {
                Debug.LogError("manifest打包失败。请检查错误!");
            }
        }

        [MenuItem("MyAssetsBundle/BuildAllAssetBundles/IOS", false, 105)]
        public static void BuildIosAssetBundles()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }

            //添加Lua
            BuildLuaAssetBundles.BuildLuaResource(BuildTarget.StandaloneWindows);
            //添加资源
            List<AssetBundleBuild> builds = BaseBuild.GetBuilds();

            BuildMainfest manifest = new BuildMainfest(builds);
            
            if (manifest.BuildManifestIsSuccess && builds != null)
            {
                BuildingAssetBundles.BuildAssetBundles(builds, BuildTarget.iOS);
            }
            else
            {
                Debug.LogError("manifest打包失败。请检查错误!");
            }
        }
    }
}



