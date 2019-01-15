using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Res;

namespace MyFramework
{
    public class UIManager : MonoBehaviour
    {
        private Dictionary<BasePanel, List<MyAsset>> assetDic = new Dictionary<BasePanel, List<MyAsset>>();

        /// <summary>
        /// 管理界面加载的资源
        /// </summary>
        /// <param name="panel"> 面板 </param>
        /// <param name="asset"> 所加载的资源 </param>
        public void AddAsset(BasePanel panel, MyAsset asset)
        {
            List<MyAsset> assets = null;
            if (assetDic.TryGetValue(panel, out assets))
            {
                assets.Add(asset);
            }
            else
            {
                assets = new List<MyAsset>();
                assets.Add(asset);
                assetDic[panel] = assets;
            }
        }

        /// <summary>
        /// 释放界面所加载的资源
        /// </summary>
        /// <param name="panel"> 面板 </param>
        public void ReleaseAsset(BasePanel panel)
        {
            List<MyAsset> assets = null;
            if (assetDic.TryGetValue(panel, out assets))
            {
                foreach (var asset in assets)
                {
                    if (asset != null)
                    {
                        asset.Release();
                    }
                }
            }
            assetDic.Remove(panel);
        }
    }
}

