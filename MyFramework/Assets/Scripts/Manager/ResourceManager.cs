﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFramework;
using System.IO;
using Res;

namespace MyFramework
{
    public class ResourceManager : MonoBehaviour
    {
        public bool Init()
        {
            return MyAssets.Initialize();
        }

        public T Load<T>(string assetName,Action<MyAsset> callback = null) where T : UnityEngine.Object
        {
            return MyAssets.Load<T>(assetName,callback).asset as T;
        }
        
        public void LoadSync<T>(string assetName, Action<MyAsset> callback) where T : UnityEngine.Object
        {
            MyAssets.LoadSync<T>(assetName, callback);
        }

        public MyAsset LoadAsset<T>(string assetName, Action<MyAsset> callback = null) where T : UnityEngine.Object
        {
            return MyAssets.Load<T>(assetName, callback);
        }
    }
}

