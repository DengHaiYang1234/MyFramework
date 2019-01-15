using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Res;
using System;

namespace MyFramework
{
    public class BasePanel : MonoBehaviour
    {
        private MyAsset asset = null;

        private Dictionary<BasePanel, List<MyAsset>> assetDic = new Dictionary<BasePanel, List<MyAsset>>();

        protected BasePanel Owner { get; set; }

        protected T OnLoadAssets<T>(string name) where T : UnityEngine.Object
        {
            asset = FrameworkMain.Instance.ResMgr.LoadAsset<T>(name);
            FrameworkMain.Instance.UIMgr.AddAsset(Owner,asset);
            return asset.asset as T;
        }

        protected void OnLoadAssetsSync<T>(string name, Action<MyAsset> callback) where T : UnityEngine.Object
        {
            asset = MyAssets.LoadSync<T>(name, callback);
            FrameworkMain.Instance.UIMgr.AddAsset(Owner, asset);
        }

        public virtual void OnDestroy()
        {
            FrameworkMain.Instance.UIMgr.ReleaseAsset(Owner);
        }
    }
}

