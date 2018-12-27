using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Res
{
    public class LoadBundleAsset : MyAsset
    {
        protected MyBundle bundle;

        public LoadBundleAsset(string path,System.Type type) : base(path,type)
        {
            
        }

        protected override void OnLoad()
        {
            bundle = MyBundles.Load(MyAssets.GetBundleByAssetName(assetName));
            asset = bundle.LoadAsset(MyAssets.GetAssetPathByAssetName(assetName), assetType);
        }

        protected override void OnUnload()
        {
            if (bundle != null)
                bundle.Release(); //减少依赖

            bundle = null; //清除引用
        }
    }
}

