using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace MyAssetBundleEditor
{
    public class BuildAssetsWithDirectroyName : BaseBuild
    {
        public BuildAssetsWithDirectroyName()
        {
            
        }

        public BuildAssetsWithDirectroyName(string path, string pattern, SearchOption option) : base(path, pattern, option)
        {
            
        }


        /// <summary>
        /// 将搜索到的所有资源按资源所在的路径进行打包，同一个路径下的所有资源会被打到一个包。(将searchPath路径下的按各自目录结构打成各自的包)
        /// 粒度适中
        /// </summary>
        public override void Build()
        {
            var files = GetFilesWithoutPacked(searchPath, searchPattern, option);
            Dictionary<string, List<string>> bundles = new Dictionary<string, List<string>>();
            for (int i = 0; i < files.Count; i++)
            {
                var item = files[i];
                if (UnityEditor.EditorUtility.DisplayCancelableProgressBar(string.Format("Collecting... [{0}/{1}]", i, files.Count), item, i * 1f / files.Count))
                {
                    break;
                }

                var path = Path.GetDirectoryName(item);
                if (!bundles.ContainsKey(path))   //按目录添加
                {
                    bundles[path] = new List<string>();
                }

                bundles[path].Add(item);
                bundles[path].AddRange(GetDependencies(item));
            }
            int count = 0;
            foreach (var item in bundles)
            {
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = BuildDefaultPath.BuildAssetBunldNameWithAssetPath(item.Key);
                build.assetNames = item.Value.ToArray();
                packedAssets.AddRange(build.assetNames);
                builds.Add(build);
                if (UnityEditor.EditorUtility.DisplayCancelableProgressBar(string.Format("Packing... [{0}/{1}]", count, bundles.Count), build.assetBundleName, count * 1f / bundles.Count))
                {
                    break;
                }

                count++;
            }
        }



        public override string GetAssetBundleName(string assetPath)
        {
            return BuildDefaultPath.BuildAssetBunldNameWithAssetPath(Path.GetDirectoryName(assetPath));
        }

    }
}

