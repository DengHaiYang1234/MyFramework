using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Res
{
    public class MyBundleWWW : MyBundle
    {

        private WWW _request;
        public MyBundleWWW(string url,Hash128 hash):base(url,hash)
        {
            
        }

        public override AssetBundle assetBundle
        {
            get
            {
                if (error != null)
                {
                    return null;
                }
                return _request.assetBundle;

            }
        }


        public override bool isDone
        {
            get
            {
                if (error != null)
                {
                    return true;
                }

                if (_request.error != null)
                {
                    return true;
                }

                if (_request.isDone && _request.assetBundle != null)
                {
                    return true;
                }

                return false;
            }
        }

        protected override void OnLoad()
        {
            _request = WWW.LoadFromCacheOrDownload(path, version);
            if (_request == null)
            {
                error = path + "LoadFromCacheOrDownload is Failed";
                MyDebug.LogErrorFormat("LoadFromCacheOrDownload is Failed. path:{0}", path);
            }
        }

        protected override void OnUnLoad()
        {
            if (_request != null)
            {
                if (_request.assetBundle != null)
                    _request.assetBundle.Unload(false);

                _request = null;
            }
        }

    }
}

