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
    }
}

