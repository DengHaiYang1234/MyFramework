using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyAssetBundleEditor
{
    public class BuildMainfest
    {
        private bool buildIsSuccess = true;

        public bool BuildManifestIsSuccess
        {
            get { return buildIsSuccess; }
        }

        public BuildMainfest(List<AssetBundleBuild> builds)
        {
            string path = BuildDefaultPath.GetManifestAssetPath();

            var asset =  AssetDatabase.LoadAssetAtPath<PackageManifest>(path);
            if(asset != null) AssetDatabase.DeleteAsset(path);

            CreatManifestAsset(builds);
        }

        private void CreatManifestAsset(List<AssetBundleBuild> builds)
        {
            List<string> checkIsRepeat = new List<string>();
            List<AssetManifestInfo> list = new List<AssetManifestInfo>();

            foreach (var build in builds)
            {
                List<string> infoList = new List<string>();

                string name = build.assetBundleName.Substring(build.assetBundleName.LastIndexOf('/') + 1);

                foreach (var assetPath in build.assetNames)
                {
                    infoList.Add(assetPath);
                    string itemName = assetPath.Substring(assetPath.LastIndexOf('/') + 1);
                    if (checkIsRepeat.Contains(itemName))
                    {
                        buildIsSuccess = false;
                        Debug.LogError(string.Format("图片命名重复 【Path】:{0},【SpriteName】:{1}", assetPath, itemName));
                        Debug.LogError(string.Format("已存在图片   【Path】:{0}", assetPath));
                        Debug.LogError("===========================================================");
                    }
                    else
                    {
                        checkIsRepeat.Add(itemName);
                    }
                }

                var manifest = new AssetManifestInfo()
                {
                    name = name,
                    bundle = build.assetBundleName,
                    assets = infoList,
                };

                list.Add(manifest);
            }

            var asset = ScriptableObject.CreateInstance<PackageManifest>();
            asset.assetInfos = list;
            asset.MapingAssetData();
            AssetDatabase.CreateAsset(asset, BuildDefaultPath.GetManifestAssetPath());
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

