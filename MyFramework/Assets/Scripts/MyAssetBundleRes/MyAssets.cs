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
            InitManifest();
#if UNITY_EDITOR
            if (!ResUtility.useEditorPrefab)
            {
                return InitializeBundle();
            }
            return true;
#else
            return InitializeBundle();
#endif
        }

        static bool InitializeBundle()
        {
            string relativePath = Path.Combine(ResUtility.AssetBundlesOutputPath, ResUtility.GetPlatformName());
            var url = 
#if UNITY_EDITOR
                relativePath + "/";
#else
                Path.Combine(Application.streamingAssetsPath, relativePath) + "/";
#endif
            if (MyBundles.Initialize(url))
            {
                var bundle = MyBundles.Load("manifest");
                if (bundle != null)
                {
                    bundle.Release();
                    MyDebug.Log("manifest Load Is Complete!");
                }
                return true;
            }
            MyDebug.LogErrorFormat("bundle manifest not exist.!");

            return false;
        }

        private static void InitManifest()
        {

#if UNITY_EDITOR
            string path = BuildDefaultPath.GetManifestAssetPath();
            var manifestAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<PackageManifest>(path);
            manifestAsset.MapingAssetData();
#endif
            manifest.Load(manifestAsset);
        }

        static MyAsset CreatAssetRuntime(string name,System.Type type,bool asyncMode)
        {
            if (asyncMode)
            {
                return null;
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
                asset.Load();
            }
            asset.Retain();
            return asset;
        }
        
    }
}

