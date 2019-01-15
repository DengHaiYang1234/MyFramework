using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Res
{
    public class MyBundles
    {
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
            dataPath = path;

            //加载AssetBundle
            var request = LoadInternal(ResUtility.GetPlatformPath, true, false);

            if (request == null || request.error != null)
                return false;

            //加载AssetBundle的所有依赖资源
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
                    if (!isLoadingAssetBundleManifest) //Bundle下载完成之后 加载对应依赖
                        LoadDependencies(bundle, assetBundleName, asyncRequest);
                }
            }
            bundle.Retain();
            return bundle;
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
                {
                    bundle.dependencies.Add(LoadInternal(item, false, asyncRequest));
                }
            }
        }

        static void UnLoadDependencies(MyBundle bundle)
        {
            foreach (var item in bundle.dependencies)
            {
                item.Release();
            }

            bundle.dependencies.Clear();
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

        public static void Update()
        {
            List<MyBundle> bundleToDestroy = new List<MyBundle>(); //需要卸载的Bundle
            foreach (var item in bundles)
            {
                if (item.Value.isDone && item.Value.references <= 0)
                {
                    bundleToDestroy.Add(item.Value);
                }
            }

            for (int i = 0; i < bundleToDestroy.Count; i++)
            {
                var bundle = bundleToDestroy[i];
                bundles.Remove(bundle.name);
                bundle.UnLoad();
                UnLoadDependencies(bundle);
                bundle = null;
            }

            bundleToDestroy.Clear();
        }
    }
}

