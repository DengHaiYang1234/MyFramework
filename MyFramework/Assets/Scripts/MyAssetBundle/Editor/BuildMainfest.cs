using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyAssetBundleEditor
{
    public class BuildMainfest
    {
        public BuildMainfest(List<AssetBundleBuild> builds)
        {
            string path = BuildDefaultPath.GetManifestAssetPath();

            var asset =  AssetDatabase.LoadAssetAtPath<PackageManifest>(path);
            if(asset != null) AssetDatabase.DeleteAsset(path);

            CreatManifestAsset(builds);
        }

        private void CreatManifestAsset(List<AssetBundleBuild> builds)
        {
            List<AssetManifestInfo> list = new List<AssetManifestInfo>();
            foreach (var build in builds)
            {
                List<string> infoList = new List<string>();
                string name = build.assetBundleName.Substring(build.assetBundleName.LastIndexOf('/') + 1);

                foreach (var assetName in build.assetNames)
                {
                    infoList.Add(assetName);
                }

                var manifest = new AssetManifestInfo()
                {
                    AssetName = name,
                    Infos = infoList,
                };

                list.Add(manifest);
            }

            var asset = ScriptableObject.CreateInstance<PackageManifest>();
            asset.assetInfos = list;
            AssetDatabase.CreateAsset(asset, BuildDefaultPath.GetManifestAssetPath());
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


    }
}

