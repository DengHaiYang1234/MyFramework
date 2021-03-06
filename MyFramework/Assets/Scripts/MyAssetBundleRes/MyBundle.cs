﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Res
{
    public class MyBundle
    {
        public string path { get; protected set; }
        public string name { get; set; }
        public virtual AssetBundle assetBundle {
            get { return _assetBundle; }
        }

        public virtual string error { get; protected set; }
        public virtual float progress {
            get { return 1; }
        }

        public virtual bool isDone {
            get { return true; }
        }

        public int references { get; private set; }

        public readonly List<MyBundle> dependencies = new List<MyBundle>();

        protected Hash128 version;

        private AssetBundle _assetBundle;

        //限定的是只有在同一程序集中可访问，可以跨类  internal
        internal MyBundle(string url,Hash128 hash)
        {
            path = url;
            version = hash;
        }

        internal void Load()
        {
            MyDebug.LogFormat("开始下载Bundle【Path】：{0}", path);
            OnLoad();
        }

        internal void UnLoad()
        {
            MyDebug.LogErrorFormat("正在卸载Bundle【Path】：{0}", path);
            OnUnLoad();
        }

        protected virtual void OnLoad()
        {
            _assetBundle = AssetBundle.LoadFromFile(path);
            if (_assetBundle == null)
            {
                error = path + "LoadFromFile is falied. path";
                MyDebug.LogErrorFormat("LoadFromFile failed . path: {0}", path);
            }
        }


        protected virtual void OnUnLoad()
        {
            if (_assetBundle != null)
            {
                _assetBundle.Unload(false); //是释放AssetBundle文件的内存镜像，不包含Load创建的Asset内存对象。  
                                            //若设置为tue是释放那个AssetBundle文件内存镜像和并销毁所有用Load创建的Asset内存对象。
                _assetBundle = null;
            }
        }

        public void Retain()
        {
            references++;
            MyDebug.LogErrorFormat("【Bundle】：{0}，【Retain】：{1}", name, references);
        }

        public T LoadAsset<T>(string assetName) where T : Object
        {
            if (error != null)
                return null;

            return assetBundle.LoadAsset(assetName, typeof(T)) as T;
        }

        public Object LoadAsset(string assetName, System.Type assetType)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                MyDebug.LogErrorFormat("资源加载失败。MyBundle LoadAsset Is Called . But 【assetName】 is Null");
                return null;
            }

            try
            {
                return assetBundle.LoadAsset(assetName, assetType);
            }
            catch(Exception e)
            {
                MyDebug.LogErrorFormat("MyBundle LoadAsset Is Called .But Have Error:{0},",e);
                return null;
            }
            
        }

        public AssetBundleRequest LoadAssetSync(string assetName, System.Type assetType)
        {
            if (error != null)
            {
                return null;
            }

            if (assetName == null)
            {
                return null;
            }

            return assetBundle.LoadAssetAsync(assetName, assetType);
        }

        public void Release()
        {
            if (--references < 0)
            {
                MyDebug.LogErrorFormat("references < 0");
            }

            MyDebug.LogErrorFormat("【Bundle】：{0}，【Release】：{1}",name,references);
        }

    }
}


