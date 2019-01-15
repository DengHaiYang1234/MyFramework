using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MyAssetBundleEditor;
using Object = UnityEngine.Object;

namespace Res
{
    /// <summary>
    /// 应用于某个类时，sealed 修饰符可阻止其他类继承自该类
    /// </summary>
    public sealed class MyAssets : MonoBehaviour
    {
        private static MyAssets instance;

        private static Manifest manifest = new Manifest();

        private static List<MyAsset> assets = new List<MyAsset>();

        static void CheckInstace()
        {
            if (instance == null)
            {
                GameObject go = new GameObject("Assets");
                DontDestroyOnLoad(go);
                instance = go.AddComponent<MyAssets>();
            }
        }

        public static bool Initialize()
        {
            CheckInstace();
#if UNITY_EDITOR
            if (!FrameworkDefaultSetting.useEditorPrefab)
            {
                return InitializeBundle();
            }
            else
            {
                InitManifest();
            }
            return true;
#else
            return InitializeBundle();
#endif
        }

        /// <summary>
        /// 初始化必要AssetBulde 
        /// </summary>
        /// <returns></returns>
        static bool InitializeBundle()
        {
            //资源目录
            string relativePath = Path.Combine(ResUtility.AssetBundlesOutputPath, ResUtility.GetPlatformPath);
            var url =
#if UNITY_EDITOR
            relativePath + "/";
#else
            ResUtility.GetDataPathByPlatform;
#endif
            if (MyBundles.Initialize(url)) //初始化Bundles信息
            {
                var bundle = MyBundles.Load(RuntimeResPath.GetManifestAssetPathExceptSuffix); //卸载配置文件
                if (bundle != null)
                {
                    InitManifest(bundle);
                    bundle.Release();
                    MyDebug.Log("manifest Load Is Complete!");
                }
                return true;
            }
            MyDebug.LogErrorFormat("bundle manifest not exist.!");
            return false;
        }

        /// <summary>
        /// 初始化manifestAsset。方便直接读取
        /// </summary>
        private static void InitManifest(MyBundle bundle = null)
        {
#if UNITY_EDITOR
            //直接读取项目资源
            if (FrameworkDefaultSetting.useEditorPrefab)
            {
                string path = BuildDefaultPath.GetManifestAssetPath();
                var manifestAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<PackageManifest>(path);
                manifestAsset.MapingAssetData();
                manifest.Load(manifestAsset);
            }
            else //从Bundle中读取
            {
                if (bundle == null)
                {
                    Console.WriteLine("从Bundle中读取Manifest失败！！！！");
                    return;
                }
                var manifestAsset =  bundle.LoadAsset<PackageManifest>(RuntimeResPath.GetManifestAssetPath);
                manifestAsset.MapingAssetData();
                manifest.Load(manifestAsset);
            }
#else
                if (bundle == null)
                {
                    Console.WriteLine("从Bundle中读取Manifest失败！！！！");
                    return;
                }
                var manifestAsset =  bundle.LoadAsset<PackageManifest>(RuntimeResPath.GetManifestAssetPath);
                manifestAsset.MapingAssetData();
                manifest.Load(manifestAsset);
#endif

        }

        static MyAsset CreatAssetRuntime(string name,System.Type type,bool asyncMode)
        {
            if (asyncMode)
            {
                return new LoadBundleAssetSync(name, type);
            }
            else
                return new LoadBundleAsset(name, type);
        }

        /// <summary>
        /// 获取资源Bundle
        /// </summary>
        /// <param name="assetPath">  </param>
        /// <returns></returns>
        public static string GetBundleName(string name)
        {
            return manifest.GetBundleName(name);
        }

        /// <summary>
        /// 获取资源Bundle
        /// </summary>
        /// <param name="assetPath">  </param>
        /// <returns></returns>
        public static string GetBundleByAssetName(string name)
        {
            return manifest.GetBundleByAssetName(name);
        }

        public static string GetAssetPathByAssetName(string name)
        {
            return manifest.GetAssetByName(name);
        }

        public static MyAsset Load<T>(string name,Action<MyAsset> callback = null) where T : Object
        {
            name = name.ToLower();
            return Load(name, typeof (T), callback);
        }

        public static MyAsset Load(string name, System.Type type, Action<MyAsset> callback = null)
        {
            return LoadInternam(name, type, false, callback);
        }
        
        public static MyAsset LoadSync<T>(string name, Action<MyAsset> callback = null) where T : Object
        {
            name = name.ToLower();
            return LoadSync(name, typeof(T), callback);
        }

        public static MyAsset LoadSync(string name, System.Type type, Action<MyAsset> callback = null)
        {
            return LoadInternam(name, type, true, callback);
        }
        
        private static MyAsset LoadInternam(string name, System.Type type, bool asyncMode, Action<MyAsset> callback = null)
        {
            MyAsset asset = assets.Find(obj => { return obj.assetName == name; });
            if (asset == null)
            {
#if UNITY_EDITOR
                if (!FrameworkDefaultSetting.useEditorPrefab)
                {
                    asset = CreatAssetRuntime(name, type, asyncMode);
                }
                else
                    asset = new MyAsset(name, type);
#else
                asset = CreatAssetRuntime(name, type, asyncMode);
#endif
                assets.Add(asset);
                if (!manifest.IsInit)
                {
                    asset.OnLoadManifest();
                }
                else
                {
                    asset.Load();
                }
            }   
            asset.Retain(); //资源依赖数量

            if (callback != null)
            {
                asset.AddCompletedLisenter(callback);
            }
            return asset;
        }

        /// <summary>
        /// 卸载指定资源
        /// </summary>
        /// <param name="asset"></param>
        public static void UndLoad(MyAsset asset)
        {
            asset.Release();
        }
                
        private System.Collections.IEnumerator gc = null;

        System.Collections.IEnumerator GC()
        {
            yield return 0;
            yield return Resources.UnloadUnusedAssets(); //释放所有已经没有引用的Asset.
        }

        private void Update()
        {
            bool removed = false;
            for (int i = 0; i < assets.Count; i++)
            {
                var asset = assets[i];
                if (!asset.Update() && asset.references <= 0) //资源下载完毕并且资源没有被引用
                {
                    asset.UnLoad();
                    asset = null; //清除该资源的引用
                    assets.RemoveAt(i);
                    i--;
                    removed = true;
                }
            }

            if (removed)
            {
                if (gc != null)
                {
                    StopCoroutine(gc);
                }

                gc = GC();
                StartCoroutine(gc);
            }
            
            //bundle卸载
            MyBundles.Update();
        }
    }
}

