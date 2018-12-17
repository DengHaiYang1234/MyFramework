using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Res
{
    public class MyBundle
    {
        public string path { get; protected set; }
        public string name { get; internal set; }
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
            
        }

        internal void UnLoad()
        {
            
        }

        protected virtual void OnLoad()
        {
            _assetBundle = AssetBundle.LoadFromFile(path);
            if (_assetBundle == null)
                MyDebug.LogErrorFormat("LoadFromFile failed . path: {0}",path);
        }


        protected virtual void OnUnLoad()
        {
            if (_assetBundle != null)
            {
                _assetBundle.Unload(false);
                _assetBundle = null;
            }
        }

        public void Retain()
        {
            references++;
        }

        public T LoadAsset<T>(string assetName) where T : Object
        {
            if (error != null)
                return null;

            return assetBundle.LoadAsset(assetName, typeof(T)) as T;
        }

    }
}


