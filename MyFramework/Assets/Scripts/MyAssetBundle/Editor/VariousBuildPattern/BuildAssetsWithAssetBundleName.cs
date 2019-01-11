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
        /// 将搜索到的所有资源按指定的 AssetBundleName 进行打包。(将searchPath路径下的都打成一个包)
        /// 粒度最大
        /// </summary>
        public override void Build()
        {
            var files = GetFilesWithoutPacked(searchPath, searchPattern, option);
            List<string> list = new List<string>();
            foreach (var item in files)
                list.AddRange(GetDependencies(item));  //获取每个Iiem对应的依赖资源

            files.AddRange(list); //自身 + 对应依赖
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = GetAssetBundleName(assetFolderName);
            build.assetNames = files.ToArray();
            builds.Add(build);
            packedAssets.AddRange(files);
        }

        public override string GetAssetBundleName(string assetName)
        {
            string path = searchPath.Substring(0, searchPath.LastIndexOf('/') + 1).ToLower();
            return path + assetName.Substring(assetName.IndexOf('[') + 1, assetName.LastIndexOf(']') - 1);
        }
    }
}

