using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyAssetBundleEditor
{
    public class MyAssetBundleMenu
    {
        [MenuItem("Assets/MyAssetBundle/Build Manifest")]
        public static void BuildAssetManifest()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }
            List<AssetBundleBuild> bundles;
        }

        [MenuItem("Assets/CheckPackagingMethod/BuildAssetsWithFilename",false,4)]
        public static void SetManifestWithBuildAssetsWithFilename()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }
            BuildPackPattern method = new BuildPackPattern(Selection.activeObject,BuildDefaultPath.BuildAssetsWithFilename);
        }

        [MenuItem("Assets/CheckPackagingMethod/BuildAssetsWithDirectroyName",false,5)]
        public static void SetManifestWithBuildAssetsWithDirectroyName()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }
            BuildPackPattern method = new BuildPackPattern(Selection.activeObject, BuildDefaultPath.BuildAssetsWithDirectroyName);
        }

        [MenuItem("Assets/CheckPackagingMethod/BuildAssetsWithAssetBundleName",false,6)]
        public static void SetManifestWithBuildAssetsWithAssetBundleName()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }
            BuildPackPattern method = new BuildPackPattern(Selection.activeObject, BuildDefaultPath.BuildAssetsWithAssetBundleName);
        }

        [MenuItem("Assets/CheckPackagingMethod/ClearPackageMethod",false,7)]
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
            BuildMainfest manifest = new BuildMainfest(builds);
        }


        [MenuItem("AssetsBundle/StartBuildAssetBundles", false, 105)]
        public static void BuildAssetBundles()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }
            List<AssetBundleBuild> builds = BaseBuild.GetBuilds();
            BuildMainfest manifest = new BuildMainfest(builds);
            BuildingAssetBundles.BuildAssetBundles(builds);

        }
    }
}



