using System.Collections;
using System.Collections.Generic;
using MyFramework;
using UnityEngine;
using Res;

namespace MyFramework
{
    public class ResMain
    {
        private FrameworkMain _owner;
        private ResourceManager res;
        private ResourceManifest mainfest;
        private static ResMain _instance;

        private Dictionary<EResType, int> resLoadNum;

        private Dictionary<EResType, List<string>> _cachedResAssetPathDic;

        private int cacheCount = 0;

        private bool isComplete = false;

        private int count = 0;

        public bool IsComplete
        {
            get { return isComplete; }
            set { isComplete = value; }
        }

        public static ResMain Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ResMain();

                return _instance;
            }
        }
        
        public ResMain()
        {
            RuntimePath rt = new RuntimePath();
            EventDispatchCenter.Instance.Registry("C2C_ASYN_LOAD_ATLAL_SPRITE",OnAsynLoadSprite);
            res = AppFacade.Instance.GetManager<ResourceManager>(ManagersName.resource);
            mainfest = res.LoadABAssetByPath<ResourceManifest>(rt.MainfestPath);
            mainfest.MappingAllData();
            resLoadNum = new Dictionary<EResType, int>();
            InitCacheResInfos();
        }


        private void OnAsynLoadSprite(object o)
        {
            count += 1;
            SDDebug.LogErrorFormat("收到下载Sprite消息{0}",o);
            if (count >= 2)
            {
                isComplete = true;
            }
        }

        private int GetCacheCount(EResType type)
        {
            return resLoadNum[type];
        }

        private int GetCacheCount()
        {
            return GetCacheCount(EResType.Atlas) + GetCacheCount(EResType.UIPrefab);
        }



        private void InitCacheResInfos()
        {
            _cachedResAssetPathDic = new Dictionary<EResType, List<string>>();
            _cachedResAssetPathDic.Add(EResType.Atlas, GetStreamInfo(EResType.Atlas));
            _cachedResAssetPathDic.Add(EResType.UIPrefab, GetStreamInfo(EResType.UIPrefab));
            cacheCount = GetCacheCount();
        }


        
        public List<string> GetStreamInfo(EResType type)
        {
            string directoryPath = Util.DataPath + BuildToolsConstDefine.GetBuildingFolderByResType(type);
            List<string> pathList = BuildingAssetHolder.Instance.GetStreamAssetFilePath(directoryPath, type);
            resLoadNum.Add(type, pathList.Count);
            return pathList;
        }
        
        public void CacheAtlas()
        {
            List<string> atlasPath = null;
            if (_cachedResAssetPathDic.TryGetValue(EResType.Atlas, out atlasPath))
            {
                foreach (var path in atlasPath)
                {
                    res.CacheBundle(path, EResType.Atlas, delegate(int num)
                    {
                        IsCacheAssetIsComplete(num);
                    });
                }
            }
            else
            {
                SDDebug.LogErrorFormat("InitCacheAtlas Is Called. But _cachedResAssetPathDic.TryGetValue(EResType.Atlas, out atlasPath) atlasPath is Null!!!!!");
            }
        }
        
        public void CacheUIPrefab()
        {
            List<string> uiPrefabPath = null;

            if (_cachedResAssetPathDic.TryGetValue(EResType.UIPrefab, out uiPrefabPath))
            {
                foreach (var path in uiPrefabPath)
                {
                    res.CacheBundle(path, EResType.UIPrefab, delegate(int num)
                    {
                        IsCacheAssetIsComplete(num);
                    });
                }
            }
            else
            {
                SDDebug.LogErrorFormat("InitCacheAtlas Is Called. But _cachedResAssetPathDic.TryGetValue(EResType.Atlas, out uiPrefabPath) atlasPath is Null!!!!!");
            }
        }



        void IsCacheAssetIsComplete(int num)
        {
            SDDebug.LogError("IsCacheAssetIsComplete:" + num);
            int cacheNum = GetCacheCount();
            if (cacheCount == num)
            {
                isComplete = true;
                //res.AdvanceLoadAssetBundleByType(EResType.Atlas);
            }
        }



        public Sprite GetSpriteByName(string name)
        {
             return res.GetSpriteByName(name);
        }



        public GameObject GetUIPrefab(string name)
        {
            //string dataPath = BuildToolsConstDefine.GetBuildingFolderByResType(EResType.UIPrefab);
            //string assetpath = string.Format("{0}/{1}",dataPath,name);
            name = name.ToLower();

            GameObject obj = res.LoadABAssetByPath<GameObject>(name,EResType.UIPrefab);

            return res.InstantiateObj(obj);
        }
    }
}


