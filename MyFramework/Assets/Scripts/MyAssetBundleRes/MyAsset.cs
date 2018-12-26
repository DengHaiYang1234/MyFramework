using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Res
{
    public class MyAsset
    {
        //回调
        private Action<MyAsset> completed;

        //资源路径
        public string assetName { get; private set; }

        //资源类型
        public Type assetType { get; private set; }

        //资源
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
            completed += lisenter;
        }

        /// <summary>
        /// 删除回调
        /// </summary>
        /// <param name="lisenter"></param>
        public void RemoveCompletedLisenter(Action<MyAsset> lisenter)
        {
            completed -= lisenter;
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
                if (asset.GetType() != typeof (GameObject))
                    Resources.UnloadAsset(asset);
                asset = null;
            }
            OnUnload();
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
                if (completed != null)
                {
                    completed.Invoke(this);
                    completed = null;
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


