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
        //    //资源结构
        //    class BundleInfo
        //    {
        //        public string path;
        //        public EResType type;
        //        public int referencedCount = 1; //资源引用数量
        //        public AssetBundle bundle = null;
        //        public List<Action<int>> callbacks;
        //        public bool isDone = false;

        //        public BundleInfo(string bPath,EResType type,AssetBundle bBundle = null)
        //        {
        //            path = bPath;
        //            bundle = bBundle;
        //            callbacks = new List<Action<int>>();
        //            referencedCount = 1;
        //            this.type = type;
        //        }
        //    }


        //    //资源AssetBundle缓存列表
        //    private Dictionary<string, BundleInfo> hashTable = null;
        //    //缓存图集资源
        //    private Dictionary<string, AssetBundle> cacheAtlasBundle = null;
        //    //缓存prefab资源
        //    private Dictionary<string, AssetBundle> cacheUIPrefabBundle = null;
        //    //缓存图片资源
        //    private Dictionary<string, Sprite> cacheAllSprite = null;
        //    //通过名称检索资源对应路径
        //    private Dictionary<string, string> cacheAllBundleMap = null;

        //    private EResType currentType;



        //    public void Initialize(Action func = null)
        //    {

        //        MyDebug.LogError("ResourceManager  Initialize !!!ResourceManager  Initialize !!! ");
        //        hashTable = new Dictionary<string, BundleInfo>();
        //        cacheAtlasBundle = new Dictionary<string, AssetBundle>();
        //        cacheUIPrefabBundle = new Dictionary<string, AssetBundle>();
        //        cacheAllSprite = new Dictionary<string, Sprite>();
        //        cacheAllBundleMap = new Dictionary<string, string>();
        //        currentType = EResType.isNull;
        //        if (func != null)
        //            func();
        //    }

        //    int GetCacheAllNum()
        //    {
        //        return cacheAtlasBundle.Count + cacheUIPrefabBundle.Count;
        //    }

        //    /// <summary>
        //    /// 缓存assetBundle
        //    /// </summary>
        //    /// <param name="path"> assetPath </param>
        //    /// <param name="callBack"> 回调 </param>
        //    public void CacheBundle(string path,EResType type,Action<int> callBack = null)
        //    {
        //        MyDebug.LogError("缓存assetBundle");
        //        currentType = type;
        //        LoadAsset(path,type,callBack);
        //    }

        //    public void LoadAsset(string path,EResType type, Action<int> callBack = null)
        //    {
        //        BundleInfo bInfo = null;
        //        if (hashTable.TryGetValue(path, out bInfo))
        //        {
        //            bInfo.referencedCount++;
        //            if (bInfo.isDone)
        //                callBack(0);
        //        }
        //        else
        //        {
        //            bInfo = new BundleInfo(path, type);
        //            if (callBack != null)
        //                bInfo.callbacks.Add(callBack);

        //            hashTable.Add(path, bInfo);
        //            LoadBundle(path, true);
        //        }
        //    }

        //    /// <summary>
        //    /// 载入素材
        //    /// </summary>
        //    /// <param name="path"></param>
        //    /// <param name="isAsync"></param>
        //    /// <returns></returns>
        //    private AssetBundle LoadBundle(string path, bool isAsync = false)
        //    {
        //        string url = Util.DataPath + path.ToLower() + AppConst.BundleName;
        //        byte[] stream = File.ReadAllBytes(url);
        //        AssetBundle bundle = null;
        //        if (isAsync)
        //            StartCoroutine(IAsynLoadBundle(path, stream));
        //        else
        //            bundle = AssetBundle.LoadFromMemory(stream);

        //        return bundle;
        //    }

        //    IEnumerator IAsynLoadBundle(string path,byte[] stream)
        //    {
        //        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(stream);
        //        yield return request;
        //        if (request.isDone)
        //        {
        //            AssetBundle bundle = request.assetBundle;
        //            BundleInfo bInfo = null;
        //            hashTable.TryGetValue(path, out bInfo);
        //            if (bInfo != null)
        //            {
        //                bInfo.bundle = bundle;
        //                bInfo.isDone = true;
        //                CacheAssetBundle(path, bInfo.bundle, bInfo.type);
        //                AddBundleMap(path);
        //                for (int i = 0; i < bInfo.callbacks.Count; i++)
        //                {
        //                    bInfo.callbacks[i](GetCacheAllNum());
        //                }

        //                bInfo.callbacks.Clear();
        //            }
        //        }
        //        yield return 0;
        //    }



        //    public void AddBundleMap(string path)
        //    {
        //        string name = path.Substring(path.LastIndexOf('/') + 1);
        //        if(!cacheAllBundleMap.ContainsKey(name))
        //            cacheAllBundleMap.Add(name, path);

        //    }

        //    public void CacheAssetBundle(string path,AssetBundle bundle, EResType type)
        //    {
        //        switch (type)
        //        {
        //            case EResType.Atlas:
        //                if (!cacheAtlasBundle.ContainsKey(path))
        //                {
        //                    cacheAtlasBundle.Add(path, bundle);
        //                }
        //                else
        //                    Console.WriteLine("Atlas  Sprite存在重复AssetBundle:" + path);
        //                break;
        //            case EResType.UIPrefab:
        //                if (!cacheUIPrefabBundle.ContainsKey(path))
        //                {
        //                    cacheUIPrefabBundle.Add(path, bundle);
        //                }
        //                else
        //                    Console.WriteLine("Prefab存在重复AssetBundle:" + path);
        //                break;
        //        }
        //    }

        //    /// <summary>
        //    /// 缓存图片资源
        //    /// </summary>
        //    /// <param name="type"></param>
        //    /// <param name="callback"></param>
        //    public void AdvanceLoadAssetBundleByType(EResType type,Action<int> callback = null)
        //    {
        //        switch (type)
        //        {
        //            case EResType.Atlas:
        //                LoadAtlasAssetByBunlde(callback);
        //                break;
        //        }
        //    }

        //    /// <summary>
        //    /// 提前加载atlas目录内容
        //    /// </summary>
        //    public void LoadAtlasAssetByBunlde(Action<int> callback = null)
        //    {
        //        foreach (var cacheSprite in cacheAtlasBundle)
        //        {
        //            StartCoroutine(AsynLoadAtlasAssetBundle(cacheSprite.Value, cacheSprite.Key, callback));
        //        }
        //    }

        //    /// <summary>
        //    /// 异步加载图片资源
        //    /// </summary>
        //    /// <param name="bundle"></param>
        //    /// <param name="name"></param>
        //    /// <param name="callback"></param>
        //    /// <returns></returns>
        //    IEnumerator AsynLoadAtlasAssetBundle(AssetBundle bundle,string name, Action<int> callback = null)
        //    {
        //        name = name.Substring(name.LastIndexOf('/') + 1) + BuildToolsConstDefine.AssetSuffix;
        //        AssetBundleRequest request = bundle.LoadAssetAsync(name);
        //        yield return request;
        //        if (request.isDone)
        //        {
        //            UGUIAtlas atlas = request.asset as UGUIAtlas;

        //            foreach (var sprite in atlas.CachedSprites)
        //            {
        //                if (cacheAllSprite.ContainsKey(sprite.name))
        //                {
        //                    MyDebug.LogErrorFormat("存在相同的图片：{0},所在图集{1}", sprite.name, atlas.name);
        //                }
        //                else
        //                {
        //                    cacheAllSprite.Add(sprite.name, sprite);
        //                    MyDebug.LogErrorFormat("sprite.name{0}:",sprite.name);
        //                    EventDispatchCenter.Instance.Dispatch("C2C_ASYN_LOAD_ATLAL_SPRITE", sprite.name);
        //                }
        //            }
        //        }
        //        yield return 0;
        //    }

        //    public Dictionary<string, AssetBundle> GetCacheAssetBundle(EResType type)
        //    {
        //        switch (type)
        //        {
        //            case EResType.Atlas:
        //                return cacheAtlasBundle;
        //            case EResType.UIPrefab:
        //                return cacheUIPrefabBundle;
        //        }

        //        return null;
        //    }



        //    public T LoadABAssetByName<T>(string name,EResType type,bool isAsyn = false) where T : UnityEngine.Object
        //    {
        //        var cacheBundle = GetCacheAssetBundle(type);
        //        if (cacheBundle == null)
        //        {
        //            MyDebug.LogErrorFormat("LoadABAssetByName Is Called. But GetCacheAssetBundle(type) is Null.Type:{0}", type);
        //            return default(T);
        //        }

        //        string path = "";

        //        if (cacheAllBundleMap.ContainsKey(name))
        //        {
        //            path = cacheAllBundleMap[name];
        //        }

        //        AssetBundle bundle = null;
        //        if (!cacheBundle.TryGetValue(path, out bundle))
        //        {
        //            MyDebug.LogFormat("未找到该Asset。请检查");
        //            return null;
        //        }

        //        T prefab = null;

        //        name = name + ResPathDef.GetAssetBunldeSuffix(type);

        //        if (isAsyn)
        //        {
        //            AssetBundleRequest request = bundle.LoadAssetAsync(name);
        //            if (request.isDone)
        //            {
        //                prefab = request.asset as T;
        //            }
        //        }
        //        else
        //            prefab = bundle.LoadAsset<T>(name);

        //        return prefab;
        //    }

        //    public T LoadABAssetByPath<T>(string path, EResType type) where T : UnityEngine.Object
        //    {
        //        path = path.ToLower();
        //        byte[] stream = File.ReadAllBytes(path);

        //        if (stream.Length == 0)
        //        {
        //            Debug.LogErrorFormat("LoadABAssetByPath is called.But stream.Length == 0 AssetBundle.LoadFromFile(path) is null. Path:{0}", path);
        //        }

        //        AssetBundle ab = AssetBundle.LoadFromMemory(stream);
        //        if (ab == null)
        //        {
        //            Debug.LogErrorFormat("LoadABAssetByPath is called.But ab == null AssetBundle.LoadFromFile(path) is null. Path:{0}", path);
        //            return null;
        //        }

        //        T res = ab.LoadAsset<T>(ResPathDef.GetLoadAssetBundleName(path,type));
        //        return res;
        //    }

        //    public GameObject InstantiateObj(GameObject obj)
        //    {
        //        return Instantiate(obj);
        //    }

        //    /// <summary>
        //    /// 获取已缓存的bundle
        //    /// </summary>
        //    /// <param name="path"></param>
        //    /// <returns></returns>
        //    public AssetBundle GetBundle(string path)
        //    {
        //        if (hashTable.ContainsKey(path))
        //            return hashTable[path].bundle;

        //        return null;
        //    }


        //    /// <summary>
        //    /// 卸载Bundle
        //    /// </summary>
        //    /// <param name="path"></param>
        //    /// <param name="bClear"></param>
        //    public void UnLoadBundle(string path, bool bClear = false)
        //    {
        //        BundleInfo bInfo = null;
        //        hashTable.TryGetValue(path, out bInfo);
        //        if (bInfo == null)
        //            return;
        //        if (--bInfo.referencedCount == 0)
        //        {
        //            bInfo.bundle.Unload(bClear);
        //            bInfo.bundle = null;
        //            bInfo.callbacks.Clear();
        //            hashTable.Remove(bInfo.path);
        //        }
        //    }
        //}
    }
}

