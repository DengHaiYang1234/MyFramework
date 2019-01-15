using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Res
{
    public class ReleaseAssetOnDestroy : MonoBehaviour
    {
        public MyAsset asset;
        public GameObject go;

        public static ReleaseAssetOnDestroy Register(GameObject go,MyAsset asset)
        {
            ReleaseAssetOnDestroy component = go.GetComponent<ReleaseAssetOnDestroy>();
            if (component == null)
                component = go.AddComponent<ReleaseAssetOnDestroy>();

            component.asset = asset;
            component.go = go;
            return component;
        }

        private void OnDestroy()
        {
            if (asset != null)
            {
                asset.Release();
                asset = null;
                go = null;
            }
        }
    }

}
