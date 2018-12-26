using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Res
{
    public class MyBundles
    {

        public static string[] activeVariants { get; private set; }

        public static string dataPath { get; private set; }

        public static AssetBundleManifest manifest { get; private set; }

        internal static readonly Dictionary<string, MyBundle> bundles = new Dictionary<string, MyBundle>();
        
        /// <summary>
        /// 初始化当前目录Bundle信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Initialize(string path)
        {
            activeVariants = new string[0];
            dataPath = path;

            //初始化当前Bundle信息
            var request = LoadInternal(ResUtility.GetPlatformName(), true, false);

            if (request == null || request.error != null)
                return false;

            //加载对应的Manifest文件
            manifest = request.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            if (manifest == null)
                return false;

            return true;
        }
        
        static MyBundle LoadInternal(string assetBundleName, bool isLoadingAssetBundleManifest, bool asyncRequest)
        {
            if (!isLoadingAssetBundleManifest)
            {
                if (manifest == null)
                {
                    MyDebug.LogErrorFormat("Please initialize AssetBundleManifest by calling Bundles.Initialize()");
                    return null;
                }
                assetBundleName = RemapVariantName(assetBundleName);
            }

            var url = GetDataPath() + assetBundleName;
            MyBundle bundle = null;
            if (!bundles.TryGetValue(assetBundleName, out bundle))
            {
                var hash = isLoadingAssetBundleManifest
                    ? new Hash128(1, 0, 0, 0)
                    : manifest.GetAssetBundleHash(assetBundleName);

                if (bundle == null)
                {
                    if (url.StartsWith("file://"))
                    {
                        bundle = new MyBundleWWW(url, hash); //启动WWW下载
                    }
                    else
                    {
                        if (asyncRequest) //是否开启异步
                            bundle = new MyBundleAsync(url, hash);
                        else
                            bundle = new MyBundle(url, hash); //同步下载
                    }

                    bundle.name = assetBundleName;
                    bundles.Add(assetBundleName, bundle);
                    bundle.Load(); //开始下载
                    if (!isLoadingAssetBundleManifest)
                        LoadDependencies(bundle, assetBundleName, asyncRequest);
                }
            }
            bundle.Retain();
            return bundle;
        }

        static string RemapVariantName(string assetBundleName)
        {
            string[] bundlesWithVariant = manifest.GetAllAssetBundlesWithVariant();

            string baseName = assetBundleName.Split('.')[0];

            int bestFit = int.MaxValue;
            int bestFitIndex = -1;

            for (int i = 0; i < bundlesWithVariant.Length; i++)
            {
                string[] curSplit = bundlesWithVariant[i].Split('.');
                string curBaseName = curSplit[0];
                string curVariant = curSplit[1];

                if (curBaseName != baseName)
                {
                    continue;
                }

                int found = System.Array.IndexOf(activeVariants, curVariant);

                if (found == -1)
                    found = int.MaxValue - 1;

                if (found < bestFit)
                {
                    bestFit = found;
                    bestFitIndex = i;
                }
            }

            if (bestFit == int.MaxValue - 1)
            {
                MyDebug.LogFormat("Ambigious asset bundle variant chosen because there was no matching active variant: " + bundlesWithVariant[bestFitIndex]);
            }

            if (bestFitIndex != -1)
            {
                return bundlesWithVariant[bestFitIndex];
            }

            return assetBundleName;
        }

        public static string GetDataPath()
        {
            return dataPath;
        }

        static void LoadDependencies(MyBundle bundle,string assetBundleName,bool asyncRequest)
        {
            var dependencies = manifest.GetAllDependencies(assetBundleName);
            if (dependencies.Length > 0)
            {
                foreach (var item in dependencies)
                    bundle.dependencies.Add(LoadInternal(item, false, asyncRequest));
            }
        }

        /// <summary>
        /// 加载Bundle
        /// </summary>
        /// <param name="assetBundleName"> assetBundle名称(注意是包里的名称) </param>
        /// <returns></returns>
        public static MyBundle Load(string assetBundleName)
        {
            return LoadInternal(assetBundleName, false, false);
        }

        public static MyBundle LoadSync(string assetBundleName)
        {
            return LoadInternal(assetBundleName, false, true);
        }
    }
}

