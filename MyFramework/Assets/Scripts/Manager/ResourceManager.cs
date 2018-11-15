using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFramework;
using System.IO;

namespace MyFramework
{
    public class ResourceManager : BaseClass
    {
        //资源结构
        class BundleInfo
        {
            public string path;
            public int referencedCount = 1;
            public AssetBundle bundle = null;
            public List<Action<AssetBundle>> callbacks;
            public bool isDone = false;

            public BundleInfo(string bPath,AssetBundle bBundle = null)
            {
                path = bPath;
                bundle = bBundle;
                callbacks = new List<Action<AssetBundle>>();
                referencedCount = 1;
            }
        }

        //资源AssetBundle缓存列表
        private Dictionary<string, BundleInfo> hashTable = null;

        public void Initialize(Action func = null)
        {
            hashTable = new Dictionary<string, BundleInfo>();
            if (func != null)
                func();
        }

        /// <summary>
        /// 缓存assetBundle
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callBack"></param>
        public void CacheBundle(string path, Action<AssetBundle> callBack = null)
        {
            LoadAsset(path, callBack);
        }

        public void LoadAsset(string path, Action<AssetBundle> callBack = null)
        {
            BundleInfo bInfo = null;
            if (hashTable.TryGetValue(path, out bInfo))
            {
                bInfo.referencedCount++;
                if (bInfo.isDone)
                    callBack(bInfo.bundle);
            }
            else
            {
                bInfo = new BundleInfo(path);
                if (callBack != null)
                    bInfo.callbacks.Add(callBack);

                hashTable.Add(path, bInfo);
                LoadBundle(path, true);
            }
        }

        /// <summary>
        /// 载入素材
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isAsync"></param>
        /// <returns></returns>
        private AssetBundle LoadBundle(string path, bool isAsync = false)
        {
            string url = Util.DataPath + path.ToLower() + AppConst.BundleName;
            byte[] stream = File.ReadAllBytes(url);
            AssetBundle bundle = null;
            if (isAsync)
                StartCoroutine(IAsynLoadBundle(path, stream));
            else
                bundle = AssetBundle.LoadFromMemory(stream);

            return bundle;
        }

        IEnumerator IAsynLoadBundle(string path,byte[] stream)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(stream);
            yield return request;
            if (request.isDone)
            {
                AssetBundle bundle = request.assetBundle;
                BundleInfo bInfo = null;
                hashTable.TryGetValue(path, out bInfo);
                if (bInfo != null)
                {
                    bInfo.bundle = bundle;
                    bInfo.isDone = true;

                    for (int i = 0; i < bInfo.callbacks.Count; i++)
                    {
                        bInfo.callbacks[i](request.assetBundle);
                    }

                    bInfo.callbacks.Clear();
                }
            }
            yield return 0;
        }

        /// <summary>
        /// 获取prefabs
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public GameObject Getprefab(string path)
        {
            AssetBundle bundle = null;
            if (hashTable.ContainsKey(path))
            {
                BundleInfo bInfo = hashTable[path];
                bInfo.referencedCount++;
                bundle = bInfo.bundle;
            }
            else
            {
                bundle = LoadBundle(path);
                BundleInfo bInfo = new BundleInfo(path,bundle);
                bInfo.isDone = true;
                hashTable.Add(path, bInfo);
            }

            GameObject prefab = Util.LoadAsset(bundle, Util.TrimPath(path));
            return prefab;
        }

        public GameObject CreatGamePrefab(string path)
        {
            GameObject prefab = Getprefab(path);
            GameObject go = Instantiate(prefab) as GameObject;
            return go;
        }

        /// <summary>
        /// 获取已缓存的bundle
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public AssetBundle GetBundle(string path)
        {
            if (hashTable.ContainsKey(path))
                return hashTable[path].bundle;

            return null;
        }

        /// <summary>
        /// 卸载Bundle
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bClear"></param>
        public void UnLoadBundle(string path, bool bClear = false)
        {
            BundleInfo bInfo = null;
            hashTable.TryGetValue(path, out bInfo);
            if (bInfo == null)
                return;
            if (--bInfo.referencedCount == 0)
            {
                bInfo.bundle.Unload(bClear);
                bInfo.bundle = null;
                bInfo.callbacks.Clear();
                hashTable.Remove(bInfo.path);
            }
        }
    }
}

