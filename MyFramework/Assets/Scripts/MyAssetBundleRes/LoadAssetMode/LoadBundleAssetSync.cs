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

        private int loadState;

        public override bool isDone
        {
            get
            {
                if (loadState == 2)
                {
                    return true;
                }


                if (bundle.error != null)
                {
                    return true;
                }

                for (int i = 0; i < bundle.dependencies.Count; i++)
                {
                    var dep = bundle.dependencies[i];
                    if (dep.error != null)
                    {
                        return true;
                    }
                }

                if (loadState == 1)
                {
                    if (assetBundleRequest.isDone)
                    {
                        asset = assetBundleRequest.asset;
                        loadState = 2;
                        return true;
                    }
                }
                else
                {
                    bool allReady = true;
                    if (!bundle.isDone)
                    {
                        allReady = false;
                    }

                    if (bundle.dependencies.Count > 0)
                    {
                        //TrueForAll:获取或设置该内部数据结构在不调整大小的情况下能够容纳的元素总数。
                        if (!bundle.dependencies.TrueForAll(bundle => bundle.isDone))
                        {
                            allReady = false;
                        }
                    }

                    if (allReady)
                    {
                        assetBundleRequest = bundle.LoadAssetSync(MyAssets.GetAssetPathByAssetName(assetName),assetType);
                        if (assetBundleRequest == null)
                        {
                            loadState = 2;
                            return true;
                        }
                        loadState = 1;
                    }
                }
                return false;
            }
        }
    }
}

