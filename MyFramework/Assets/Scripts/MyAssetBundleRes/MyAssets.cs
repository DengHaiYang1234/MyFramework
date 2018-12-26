using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MyAssetBundleEditor;

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
            if (!ResUtility.useEditorPrefab)
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

        static bool InitializeBundle()
        {
            //资源目录
            string relativePath = Path.Combine(ResUtility.AssetBundlesOutputPath, ResUtility.GetPlatformName());
            var url = 
#if UNITY_EDITOR
                relativePath + "/";
#else
                Path.Combine(Application.streamingAssetsPath, relativePath) + "/";
#endif
            if (MyBundles.Initialize(url)) //初始化Bundles信息
            {
                var bundle = MyBundles.Load(RuntimeResPath.GetManifestAssetPathExceptSuffix);
                if (bundle != null)
                {
                    InitManifest();
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
        private static void InitManifest()
        {
#if UNITY_EDITOR
            //直接读取项目资源
            if (ResUtility.useEditorPrefab)
            {
                string path = BuildDefaultPath.GetManifestAssetPath();
                var manifestAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<PackageManifest>(path);
                manifestAsset.MapingAssetData();
                manifest.Load(manifestAsset);
            }
            else //从Bundle中读取
            {
                var manifestAsset = Load<PackageManifest>("manifest");
                PackageManifest assets = manifestAsset.asset as PackageManifest;
                assets.MapingAssetData();
                manifest.Load(assets);
            }
#else
                var manifestAsset = Load<PackageManifest>("manifest");
                PackageManifest assets = manifestAsset.asset as PackageManifest;
                assets.MapingAssetData();
                manifest.Load(assets);
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

        public static MyAsset Load<T>(string name) where T : Object
        {
            name = name.ToLower();
            return Load(name, typeof (T));
        }

        public static MyAsset Load(string name, System.Type type)
        {
            return LoadInternam(name,type,false);
        }

        private static MyAsset LoadInternam(string name, System.Type type, bool asyncMode)
        {
            MyAsset asset = assets.Find(obj => { return obj.assetName == name; });
            if (asset == null)
            {
#if UNITY_EDITOR
                if (!ResUtility.useEditorPrefab)
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
            return asset;
        }
        
    }
}

