using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;

namespace MyAssetBundleEditor
{
    public class BuildAssetsWithAssetBundleName : BaseBuild
    {
        public BuildAssetsWithAssetBundleName()
        {
            
        }

        public BuildAssetsWithAssetBundleName(string path,string pattern,SearchOption option,string assetBundleName) : base(path,pattern,option)
        {
            bundleName = assetBundleName;
        }

        /// <summary>
        /// 将searchPath下的所有文件连同其各自对应的Dependencies都打包在一起
        /// </summary>
        public override void Build()
        {
            var files = GetFilesWithoutPacked(searchPath, searchPattern, option);
            List<string> list = new List<string>();
            foreach (var item in files)
                list.AddRange(GetDependencies(item));

            files.AddRange(list);
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = GetAssetBundleName(assetName);
            build.assetNames = files.ToArray();
            builds.Add(build);
            packedAssets.AddRange(files);
        }

        public override string GetAssetBundleName(string assetName)
        {
            return assetName.Substring(assetName.IndexOf('[') + 1, assetName.LastIndexOf(']') - 1);
        }
    }
}

