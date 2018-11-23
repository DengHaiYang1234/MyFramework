using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFramework;
using System.IO;
using Res;

namespace MyFramework
{
    public class ResourceManager : BaseClass
    {
        //资源结构
        class BundleInfo
        {
            public string path;
            public EResType type;
            public int referencedCount = 1; //资源引用数量
            public AssetBundle bundle = null;
            public List<Action<int>> callbacks;
            public bool isDone = false;

            public BundleInfo(string bPath,EResType type,AssetBundle bBundle = null)
            {
                path = bPath;
                bundle = bBundle;
                callbacks = new List<Action<int>>();
                referencedCount = 1;
                this.type = type;
            }
        }


        //资源AssetBundle缓存列表
        private Dictionary<string, BundleInfo> hashTable = null;

        private Dictionary<string, AssetBundle> cacheAtlasBundle = null;

        private Dictionary<string, AssetBundle> cacheUIPrefabBundle = null;

        private Dictionary<string, Sprite> cacheAllSprite = null;


        private EResType currentType;
        
        public void Initialize(Action func = null)
        {
            hashTable = new Dictionary<string, BundleInfo>();
            cacheAtlasBundle = new Dictionary<string, AssetBundle>();
            cacheUIPrefabBundle = new Dictionary<string, AssetBundle>();
            cacheAllSprite = new Dictionary<string, Sprite>();
            currentType = EResType.isNull;
            if (func != null)
                func();
        }

        int GetCacheAllNum()
        {
            SDDebug.LogError("GetCacheAllNum:" + (cacheAtlasBundle.Count + cacheUIPrefabBundle.Count));
            return cacheAtlasBundle.Count + cacheUIPrefabBundle.Count;
        }

        ///// <summary>
        ///// 缓存assetBundle
        ///// </summary>
        ///// <param name="path"> assetPath </param>
        ///// <param name="callBack"> 回调 </param>
        //public void CacheBundle(string path, Action<AssetBundle> callBack = null)
        //{
        //    LoadAsset(path, callBack);
        //}


        /// <summary>
        /// 缓存assetBundle
        /// </summary>
        /// <param name="path"> assetPath </param>
        /// <param name="callBack"> 回调 </param>
        public void CacheBundle(string path,EResType type,Action<int> callBack = null)
        {
            currentType = type;
            LoadAsset(path,type,callBack);
        }



        public void LoadAsset(string path,EResType type, Action<int> callBack = null)
        {
            BundleInfo bInfo = null;
            if (hashTable.TryGetValue(path, out bInfo))
            {
                bInfo.referencedCount++;
                if (bInfo.isDone)
                    callBack(0);
            }
            else
            {
                bInfo = new BundleInfo(path, type);
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
                    CacheAssetBundle(path, bInfo.bundle, bInfo.type);


                    for (int i = 0; i < bInfo.callbacks.Count; i++)
                    {
                        bInfo.callbacks[i](GetCacheAllNum());
                    }

                    bInfo.callbacks.Clear();

                }
            }
            yield return 0;
        }

        public void CacheAssetBundle(string path,AssetBundle bundle, EResType type)
        {
            switch (type)
            {
                case EResType.Atlas:
                    if (!cacheAtlasBundle.ContainsKey(path))
                    {
                        cacheAtlasBundle.Add(path, bundle);
                    }
                    else
                        Console.WriteLine("Atlas存在重复AssetBundle:" + path);
                    break;
                case EResType.UIPrefab:
                    if (!cacheUIPrefabBundle.ContainsKey(path))
                    {
                        cacheUIPrefabBundle.Add(path, bundle);
                    }
                    else
                        Console.WriteLine("Prefab存在重复AssetBundle:" + path);
                    break;
            }
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
                //BundleInfo bInfo = new BundleInfo(path,bundle);
                //bInfo.isDone = true;
                //hashTable.Add(path, bInfo);
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

        public void AdvanceLoadAssetBundleByType(EResType type)
        {
            switch (type)
            {
                case EResType.Atlas:
                    LoadAtlasAssetByBunlde();
                    break;
            }
        }

        /// <summary>
        /// 提前加载atlas目录内容
        /// </summary>
        public void LoadAtlasAssetByBunlde()
        {
            foreach (var cacheUIPrefab in cacheUIPrefabBundle)
            {
                StartCoroutine(AsynLoadAtlasAssetBundle(cacheUIPrefab.Value, cacheUIPrefab.Key));
            }
        }
        
        IEnumerator AsynLoadAtlasAssetBundle(AssetBundle bundle,string name)
        {
            AssetBundleRequest request = bundle.LoadAssetAsync(name);
            yield return request;
            if (request.isDone)
            {
                UGUIAtlas atlas = request.asset as UGUIAtlas;

                foreach (var sprite in atlas.CachedSprites)
                {
                    if (cacheAllSprite.ContainsKey(sprite.name))
                    {
                        SDDebug.LogErrorFormat("存在相同的图片：{0},所在图集{1}", sprite.name, atlas.name);
                    }
                    else
                        cacheAllSprite.Add(sprite.name, sprite);
                }
            }
            yield return 0;
        }

        public Sprite GetSpriteByName(string name)
        {
            Sprite sprite = null;
            if (!cacheAllSprite.TryGetValue(name, out sprite))
            {
                SDDebug.LogFormat("未找到该图片。请检查");
            }
            return sprite;
        }

        public GameObject LoadUIPrefabAssetByPath(string path,bool isAsyn = false)
        {
            AssetBundle bundle = null;
            if (!cacheUIPrefabBundle.TryGetValue(path, out bundle))
            {
                SDDebug.LogFormat("未找到该prefab。请检查");
            }

            GameObject prefab = null;

            if (isAsyn)
            {
                AssetBundleRequest request = bundle.LoadAssetAsync(path);
                if (request.isDone)
                {
                    prefab = request.asset as GameObject;
                }
            }
            else
                prefab = bundle.LoadAsset(name, typeof(GameObject)) as GameObject;

            return prefab;
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

