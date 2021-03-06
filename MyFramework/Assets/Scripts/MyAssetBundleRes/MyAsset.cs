﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Res
{
    public class MyAsset
    {
        //回调
        private Action<MyAsset> callback;

        //资源路径
        public string assetName { get; private set; }

        //资源类型
        public Type assetType { get; private set; }

        //资源Obj
        public UnityEngine.Object asset { get; protected set; }

        //是否加载完成
        public virtual bool isDone {
            get { return true; }
        }

        //引用
        public int references { get; private set; }

        /// <summary>
        /// 加载完成回调
        /// </summary>
        /// <param name="lisenter"></param>
        public void AddCompletedLisenter(Action<MyAsset> lisenter)
        {
            callback += lisenter;
        }

        /// <summary>
        /// 删除回调
        /// </summary>
        /// <param name="lisenter"></param>
        public void RemoveCompletedLisenter(Action<MyAsset> lisenter)
        {
            callback -= lisenter;
        }

        public MyAsset(string path,Type type)
        {
            assetName = path;
            assetType = type;
        }

        public void Load()
        {
            OnLoad();
        }

        /// <summary>
        /// 卸载
        /// </summary>
        public void UnLoad()
        {
            if (asset != null)
            {
                MyDebug.LogErrorFormat("卸载资源：{0}", asset.name);
                if (asset.GetType() != typeof (GameObject))
                    Resources.UnloadAsset(asset);  //释放指定已经没有引用的Asset. 注意：只能卸载从磁盘加载的文件
                asset = null;
            }
            OnUnload(); //更新资源与Bundle之间的引用关系
            assetName = null;
        }

        /// <summary>
        /// 从项目直接加载
        /// </summary>
        protected virtual void OnLoad()
        {
#if UNITY_EDITOR
            asset = UnityEditor.AssetDatabase.LoadAssetAtPath(MyAssets.GetAssetPathByAssetName(assetName), assetType);
#endif
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        protected virtual void OnUnload()
        {
            
        }

        public void OnLoadManifest()
        {
            var request = MyBundles.Load(RuntimeResPath.GetManifestAssetPathExceptSuffix);
            asset = request.LoadAsset(RuntimeResPath.GetManifestAssetPath, assetType);
        }

        /// <summary>
        /// 加载完成执行回调
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            if (isDone)
            {
                if (callback != null)
                {
                    callback.Invoke(this);
                    callback = null;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新引用
        /// </summary>
        public void Retain()
        {
            Update();
            references++;
        }

        /// <summary>
        /// 释放引用
        /// </summary>
        public void Release()
        {
            if (--references < 0)
            {
                Debug.LogError("references < 0");
            }
        }
    }
}


