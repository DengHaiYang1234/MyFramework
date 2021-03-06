﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Res
{
    public class MyBundleAsync : MyBundle
    {

        public override AssetBundle assetBundle
        {
            get { return _request.assetBundle; }
        }

        private AssetBundleCreateRequest _request;

        protected override void OnLoad()
        {
            _request = AssetBundle.LoadFromFileAsync(path);
            if (_request == null)
            {
                error = path + "LoadFromFileAsync is falied. path";
                MyDebug.LogErrorFormat("LoadFromFileAsync is falied. path:{0}", path);
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

        // <summary>
        // 是否成功下载
        // </summary>
        public override bool isDone
        {
            get
            {
                if (error != null)
                {
                    return true;
                }
                return _request.isDone;
            }
        }

        public MyBundleAsync(string url,Hash128 hash):base(url,hash)
        {
            
        }
    }
}

