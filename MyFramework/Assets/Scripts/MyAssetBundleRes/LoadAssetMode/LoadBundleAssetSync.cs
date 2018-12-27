using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Res
{
    public class LoadBundleAssetSync : LoadBundleAsset
    {
        private AssetBundleRequest assetBundleRequest;
        public LoadBundleAssetSync(string path, System.Type type) : base(path,type)
        {
            
        }

        protected override void OnLoad()
        {
            bundle = MyBundles.LoadSync(MyAssets.GetBundleByAssetName(assetName));
        }


        

        protected override void OnUnload()
        {
            base.OnUnload();
            assetBundleRequest = null;
            loadState = 0;
        }

        private int loadState;

        public override bool isDone
        {
            get
            {
                if (loadState == 2)
                {
                    return true;
                }


                if (bundle.error != null) //bundle下载完成
                {
                    return true;
                }

                for (int i = 0; i < bundle.dependencies.Count; i++)
                {
                    var dep = bundle.dependencies[i];
                    if (dep.error != null)  //配置文件全部下载完毕
                    {
                        return true;
                    }
                }

                if (loadState == 1)
                {
                    if (assetBundleRequest.isDone) //资源下载成功
                    {
                        asset = assetBundleRequest.asset;
                        loadState = 2;
                        return true;
                    }
                }
                else
                {
                    bool allReady = true;
                    if (!bundle.isDone) //检测Bundle是否下载完成
                    {
                        allReady = false; //是否可以开始下载资源
                    }

                    if (bundle.dependencies.Count > 0)
                    {
                        //TrueForAll:获取或设置该内部数据结构在不调整大小的情况下能够容纳的元素总数。
                        if (!bundle.dependencies.TrueForAll(bundle => bundle.isDone)) //依赖资源是否下载完成
                        {
                            allReady = false;
                        }
                    }

                    if (allReady) //Bundle下载完成.开始下载Assets
                    {
                        assetBundleRequest = bundle.LoadAssetSync(MyAssets.GetAssetPathByAssetName(assetName),assetType);
                        if (assetBundleRequest == null) //下载完成。但未发现指定资源
                        {
                            loadState = 2;
                            return true;
                        }
                        loadState = 1; //下载完成。存在资源
                    }
                }
                return false;
            }
        }
    }
}

