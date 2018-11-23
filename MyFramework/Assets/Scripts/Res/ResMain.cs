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

        private static ResMain _instance;

        private Dictionary<EResType, int> resLoadNum;

        private int cacheCount = 0;

        private bool isComplete = false;

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
            res = AppFacade.Instance.GetManager<ResourceManager>(ManagersName.resource);
            resLoadNum = new Dictionary<EResType, int>();
        }

        private int GetCacheCount(EResType type)
        {
            return resLoadNum[type];
        }
        
        public void InitCacheAtlas()
        {
            string directoryName = Util.DataPath + BuildToolsConstDefine.GetBuildingFolderByResType(EResType.Atlas);
            List<string> atlasPath = BuildingAssetHolder.Instance.GetStreamAssetFilePath(directoryName,EResType.Atlas);
            resLoadNum.Add(EResType.Atlas, atlasPath.Count);
            foreach (var path in atlasPath)
            {
                res.CacheBundle(path,EResType.Atlas, delegate (int num)
                {
                    IsCacheAssetIsComplete(num);
                });
            }

            res.AdvanceLoadAssetBundleByType(EResType.Atlas);
        }
        
        public void InitCacheUIPrefab()
        {
            string directoryName = Util.DataPath + BuildToolsConstDefine.GetBuildingFolderByResType(EResType.UIPrefab);
            List<string> uiPrefabPath = BuildingAssetHolder.Instance.GetStreamAssetFilePath(directoryName,EResType.UIPrefab);
            resLoadNum.Add(EResType.UIPrefab, uiPrefabPath.Count);
            foreach (var path in uiPrefabPath)
            {
                res.CacheBundle(path,EResType.UIPrefab, delegate(int num)
                {
                    IsCacheAssetIsComplete(num);
                });
            }
        }


        void IsCacheAssetIsComplete(int num)
        {
            SDDebug.LogError("IsCacheAssetIsComplete:" + num);
            int cacheNum = GetCacheCount(EResType.Atlas) + GetCacheCount(EResType.UIPrefab);
            if (cacheCount == num)
            {
                isComplete = true;
            }
        }



        public Sprite GetSpriteByName(string name)
        {
            return res.GetSpriteByName(name);
        }

        public GameObject GetUIPrefab(string name)
        {
            string dataPath = BuildToolsConstDefine.GetBuildingFolderByResType(EResType.UIPrefab);
            string assetpath = string.Format("{0}/{1}",dataPath,name);

            return res.LoadUIPrefabAssetByPath(assetpath);
        }



    }
}


