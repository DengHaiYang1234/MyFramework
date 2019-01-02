using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using MyFramework;

namespace MyAssetBundleEditor
{
    public abstract class BaseBuild
    {
        protected static List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
        protected static List<string> packedAssets = new List<string>(); //记录打包的资源
        private static List<BaseBuild> patterns = new List<BaseBuild>();
        private static Dictionary<string, List<string>> allDependencies = new Dictionary<string, List<string>>(); //所有文件依赖信息

        public string searchPath;
        public string searchPattern;
        public SearchOption option;
        public string bundleName;
        public string assetName;

        static BaseBuild()
        {
            
        }

        protected BaseBuild(string path, string pattern, SearchOption option)
        {
            searchPath = path;
            searchPattern = pattern;
            this.option = option;
        }

        protected BaseBuild()
        {
            
        }

        public abstract void Build();

        public abstract string GetAssetBundleName(string assetPath);

        public static List<AssetBundleBuild> GetBuilds()
        {
            packedAssets.Clear();
            builds.Clear();
            //allDependencies.Clear();
            builds.Add(BuildManifest());

            string packagePatternPath = BuildDefaultPath.GetBuildPattrenAssetPath();

            if(!File.Exists(packagePatternPath))
                new BuildPackPattern();

            if (!LoadEachPatterns())
                return null;

            foreach (var item in patterns)
            {
                if (item.searchPath == "")
                {
                    Debug.LogErrorFormat("assetName:{0}   searchPath is null ! Check !!!", item.assetName);
                    continue;
                }

                if (item.searchPattern == "")
                {
                    Debug.LogErrorFormat("assetName:{0}   searchPattern is null ! Check !!!", item.assetName);
                    continue;
                }

                CollectDependencies(ResFileInfo.GetFilesWithoutDirectores(item.searchPath, item.searchPattern,
                    item.option));
            }


            foreach (var item in patterns)
            {
                item.Build();
            }

            BuildAtlas();

            UnityEditor.EditorUtility.ClearProgressBar();

            return builds;
        }
        
        private static bool LoadEachPatterns()
        {
            bool isError = true;
            //在程序编译阶段，编译器会自动将using语句生成为try-finally语句，并在finally块中调用对象的Dispose方法，来清理资源。所以，using语句等效于try-finally语句
            patterns.Clear();
            var pkgPattern = AssetDatabase.LoadAssetAtPath<PackagePattern>(BuildDefaultPath.GetBuildPattrenAssetPath());
            if (pkgPattern != null)
            {
                pkgPattern.MappingPackageData();
                var data = pkgPattern.GetCacheAssetDataInfos();
                var e = data.GetEnumerator();
                while (e.MoveNext())
                {
                    if (e.Current.Value.assetName != null)
                    {
                        if (e.Current.Value.BuildType == BuildType.None)
                        {
                            MyDebug.LogErrorFormat("LoadEachPatterns is Called,But  BuildType == None! 【assetName】:{0}", e.Current.Value.assetName);
                            isError = false;
                            continue;
                        }

                        var type =
                            typeof (BaseBuild).Assembly.GetType("MyAssetBundleEditor." + Enum.GetName(typeof(BuildType), e.Current.Value.BuildType)); //反射得到对应对象

                        if (type != null)
                        {
                            //实例化
                            var pattern = Activator.CreateInstance(type) as BaseBuild;
                            pattern.searchPath = e.Current.Value.searchPath;
                            pattern.searchPattern = e.Current.Value.searchPattern;
                            pattern.bundleName = e.Current.Value.bundleName;
                            pattern.option = e.Current.Value.searchOption;
                            pattern.assetName = e.Current.Value.assetName;
                            patterns.Add(pattern);
                        }
                        else
                        {
                            Debug.LogError(string.Format("BuildType is Have.But MyAssetBundleEditor.{0} is null!!!!", e.Current.Value.BuildType));
                        }
                    }
                    else
                    {
                        Debug.LogErrorFormat("LoadEachPatterns is Called.But assetName == null  {0}",e.Current.Key);
                    }
                }
            }

            return isError;
        }
        /// <summary>
        /// 搜集依赖
        /// </summary>
        /// <param name="files"> 资源 </param>
        protected static void CollectDependencies(List<string> files)
        {
            for (int i = 0; i < files.Count; i++)
            {
                var item = files[i];
                var dependencies = AssetDatabase.GetDependencies(item);

                if (
                    UnityEditor.EditorUtility.DisplayCancelableProgressBar(
                        string.Format("CollectDependencies....[{0}/{1}]", i, files.Count), item, i*1f/files.Count))
                    break;

                foreach (var assetPath in dependencies) 
                {
                    if (!allDependencies.ContainsKey(assetPath))
                        allDependencies[assetPath] = new List<string>();

                    if (!allDependencies[assetPath].Contains(item))  //收集资源依赖情况
                        allDependencies[assetPath].Add(item);
                }
            }
        }

        /// <summary>
        /// 获取packedAssets之外的资源
        /// </summary>
        /// <param name="searchPath"></param>
        /// <param name="searchPattern"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        protected static List<string> GetFilesWithoutPacked(string searchPath,string searchPattern,SearchOption option)
        {
            var files = ResFileInfo.GetFilesWithoutDirectores(searchPath, searchPattern, option);
            var filesCount = files.Count;
            var removeAll = files.RemoveAll((string file) =>
            {
                //TODO
                return packedAssets.Contains(file);
            });

            Debug.LogError(string.Format("RemoveAll {0} size: {1}",removeAll,filesCount));

            return files;
        }

        /// <summary>
        /// 对应路径下的依赖文件
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        protected static List<string> GetDependencies(string pathName)
        {
            var assets = AssetDatabase.GetDependencies(pathName);
            List<string> assetNames = new List<string>();
            foreach (var assetPath in assets)
            {
                //1.剔除prefab  2.去除自身  3.去除已打包资源  4.去除.cs文件
                if (assetPath.Contains(".prefab") || assetPath.Equals(pathName) || packedAssets.Contains(assetPath) ||
                    assetPath.EndsWith(".cs", StringComparison.CurrentCulture))
                {
                    continue;
                }

                if (allDependencies[assetPath].Count == 1) //排除多依赖文件
                    assetNames.Add(assetPath);
            }

            return assetNames;
        }

        static void BuildAtlas()
        {
            foreach (var item in builds)
            {
                var assetsPath = item.assetNames;
                foreach (var assetPath in assetsPath)
                {
                    if (assetPath.IndexOf(BuildDefaultPath.assetsAtlasFloder) != -1)
                    {
                        var importer = AssetImporter.GetAtPath(assetPath);
                        var ti = importer as TextureImporter;
                        if (ti.textureType == TextureImporterType.Sprite)
                        {
                            var tex = AssetDatabase.LoadAssetAtPath<Texture>(assetPath);
                            if (tex.texelSize.x >= 1024 || tex.texelSize.y >= 1024)
                            {
                                continue;
                            }

                            var tag = item.assetBundleName.Replace("/", "_");
                            if (!tag.Equals(ti.spritePackingTag))
                            {
                                var settings = ti.GetPlatformTextureSettings(ResUtility.GetPlatformName);
                                settings.format = ti.GetAutomaticFormat(ResUtility.GetPlatformName);
                                settings.overridden = true;
                                ti.SetPlatformTextureSettings(settings);
                                ti.spritePackingTag = tag;
                                ti.SaveAndReimport();
                            }
                        }
                    }
                }
            }
        }
        
        private static AssetBundleBuild BuildManifest()
        {
            string manifestAssetsName = BuildDefaultPath.GetManifestAssetPath();
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = manifestAssetsName.Substring(0,manifestAssetsName.LastIndexOf('.')).ToLower();
            build.assetNames = new string[] { manifestAssetsName };
            return build;
        }
    }

}


