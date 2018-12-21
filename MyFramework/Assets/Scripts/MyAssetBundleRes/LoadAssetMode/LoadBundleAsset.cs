using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Res
{
    public class LoadBundleAsset : MyAsset
    {
        protected MyBundle request;

        public LoadBundleAsset(string path,System.Type type) : base(path,type)
        {
            
        }

        protected override void OnLoad()
        {
            request = MyBundles.Load(MyAssets.GetBundleByAssetName(assetName));
            asset = request.LoadAsset(MyAssets.GetAssetPathByAssetName(assetName), assetType);
        }
    }
}

