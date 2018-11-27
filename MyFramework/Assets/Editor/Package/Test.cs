using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Test : MonoBehaviour {

    [MenuItem("AssetsBundle/Build Test", false, 105)]
    public static void TestBuild()
    {
        string assetFolder = Application.dataPath.ToLower() + "/StreamingAssets/";
        if (!Directory.Exists(assetFolder))
            Directory.CreateDirectory(assetFolder);

        BuildAssetBundleOptions options = BuildAssetBundleOptions.CompleteAssets |
                                  BuildAssetBundleOptions.CollectDependencies |
                                  BuildAssetBundleOptions.DeterministicAssetBundle;

        BuildPipeline.BuildAssetBundles(assetFolder, options, BuildTarget.StandaloneWindows);
    }
}
