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

        private int loadState; //1.资源下载完毕  2.资源未找到或资源已获取 

        public override bool isDone
        {
            get
            {
                if (loadState == 2) //特殊判断  存在资源下载完成，但有错误(Null)  直接跳出
                {
                    return true;
                }
                
                if (bundle.error != null) //特殊判断  bundle异步下载完成，但有错误(Null).  直接跳出
                {
                    return true;
                }

                for (int i = 0; i < bundle.dependencies.Count; i++)
                {
                    var dep = bundle.dependencies[i];
                    if (dep.error != null)  //特殊判断   依赖下载完毕,但存在错误(Null) 直接跳出 
                    {
                        return true;
                    }
                }
                
                    
                if (loadState == 1)
                {
                    if (assetBundleRequest.isDone) //资源已下载成功。获取资源
                    {
                        asset = assetBundleRequest.asset;
                        MyDebug.LogErrorFormat("{0}:资源下载完毕.", asset.name);
                        loadState = 2;
                        return true;
                    }
                }
                else
                {
                    bool allReady = true;

                    if (!bundle.isDone) //检测Bundle是否下载完成.未完成继续等待Bundle下载完成
                    {
                        allReady = false; 
                    }

                    if (bundle.dependencies.Count > 0)
                    {
                        //TrueForAll:获取或设置该内部数据结构在不调整大小的情况下能够容纳的元素总数。
                        if (!bundle.dependencies.TrueForAll(bundle => bundle.isDone)) //依赖资源是否下载完成.未完成则继续等待依赖文件下载完成
                        {
                            allReady = false;
                        }
                    }

                    if (allReady) //Bundle下载完成 依赖文件下载完成 .开始下载Assets
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

