using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace MyAssetBundleEditor
{
    public class BuildAssetsWithFilename : BaseBuild
    {
        public BuildAssetsWithFilename()
        {
            
        }

        public BuildAssetsWithFilename(string path, string pattern, SearchOption option) : base(path, pattern, option)
        {
            
        }

        /// <summary>
        /// 细化打包  一个file只打其自身加依赖文件
        /// </summary>
        public override void Build()
        {
            var files = GetFilesWithoutPacked(searchPath, searchPattern, option);

            for (int i = 0; i < files.Count; i++)
            {
                var item = files[i];
                if (EditorUtility.DisplayCancelableProgressBar(string.Format("Packing... [{0}/{1}]", i, files.Count),
                    item, i*1f/files.Count))
                {
                    break;
                }

                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = BuildDefaultPath.BuildAssetBunldNameWithAssetPath(item);
                var assetNames = GetDependencies(item);
                assetNames.Add(item);
                build.assetNames = assetNames.ToArray();
                packedAssets.AddRange(assetNames);
                builds.Add(build);
            }
        }

        public override string GetAssetBundleName(string assetPath)
        {
            throw new NotImplementedException();
        }

    }
}

