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

        [MenuItem("Assets/CheckPackagingMethod/BuildAssetsWithFilename")]
        public static void SetManifestWithBuildAssetsWithFilename()
        {
            BuildPackMethod method = new BuildPackMethod(Selection.activeObject,BuildDefaultPath.BuildAssetsWithFilename);
        }

        [MenuItem("Assets/CheckPackagingMethod/BuildAssetsWithDirectroyName")]
        public static void SetManifestWithBuildAssetsWithDirectroyName()
        {
            BuildPackMethod method = new BuildPackMethod(Selection.activeObject, BuildDefaultPath.BuildAssetsWithDirectroyName);
        }

        [MenuItem("Assets/CheckPackagingMethod/BuildAssetsWithAssetBundleName")]
        public static void SetManifestWithBuildAssetsWithAssetBundleName()
        {
            BuildPackMethod method = new BuildPackMethod(Selection.activeObject, BuildDefaultPath.BuildAssetsWithAssetBundleName);
        }

        [MenuItem("Assets/CheckPackagingMethod/ClearPackageMethod")]
        public static void ClearPackageMethod()
        {
            BuildPackMethod method = new BuildPackMethod(true);
        }
    }
}



