using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using MyFramework;
using Object = UnityEngine.Object;

namespace Res
{
    public class BuildingAssetHolder
    {
        //资源结构
        class BundleInfo
        {
            public string path;
            public int referencedCount = 1; //资源引用数量
            public AssetBundle bundle = null;
            public List<Action<AssetBundle>> callbacks;
            public bool isDone = false;

            public BundleInfo(string bPath, AssetBundle bBundle = null)
            {
                path = bPath;
                bundle = bBundle;
                callbacks = new List<Action<AssetBundle>>();
                referencedCount = 1;
            }
        }


        private static BuildingAssetHolder _instance;

        public static BuildingAssetHolder Instance
        {
            get {
                if (_instance == null)
                {
                    _instance = new BuildingAssetHolder();
                }
                return _instance; }
        }

        //根据EResType缓存StreamingAssets资源路径
        private Dictionary<EResType, string> _cacheAssetStreamPath;
        //缓存所有Sprite
        private Dictionary<string, Sprite> _cachedAllSprite;
        //图片所对应图集名称
        private Dictionary<string,string> _cachedAllSpriteAtlasReleation;
        //缓存所有UIPrefabs
        private Dictionary<string, GameObject> _cachedAllUIPrefabs;

        private Dictionary<EResType, List<string>> _cacheAllAssetPath;


        public Dictionary<string, string> AllSpriteAtlasReleation
        {
            get { return _cachedAllSpriteAtlasReleation; }
        }

        public BuildingAssetHolder()
        {
            _cachedAllSprite = new Dictionary<string, Sprite>();
            _cachedAllSpriteAtlasReleation = new Dictionary<string, string>();
            _cacheAssetStreamPath = new Dictionary<EResType, string>();
            _cacheAllAssetPath = new Dictionary<EResType, List<string>>();
        }
        
        public void AddAssetPath(EResType type,string assetPath)
        {
            List<string> _lsit = null;
            if (!_cacheAllAssetPath.TryGetValue(type, out _lsit))
            {
                _lsit = new List<string>();
                _lsit.Add(assetPath);
                _cacheAllAssetPath.Add(type, _lsit);
            }
            else
            {
                if (_lsit.Contains(assetPath))
                {
                    Debug.LogErrorFormat("AssetPath {0} duplicated !! in EResType {1}", assetPath, type);
                    return;
                }

                _lsit.Add(assetPath);
            }
        }


        public List<string> GetAllAtlasAssetPath()
        {
            return _cacheAllAssetPath[EResType.Atlas];
        }


        public List<string> GetAllUIPrefabAssetPath()
        {
            return _cacheAllAssetPath[EResType.UIPrefab];
        }

        public List<SpriteAtlasRelation> SpriteRelation
        {
            get {
                List<SpriteAtlasRelation> res = new List<SpriteAtlasRelation>();
                if (_cachedAllSpriteAtlasReleation == null)
                {
                    return res;
                }
                var e = _cachedAllSpriteAtlasReleation.GetEnumerator();
                while (e.MoveNext())
                {
                    res.Add(new SpriteAtlasRelation()
                    {
                        AtlasName = e.Current.Value,
                        SpriteName = e.Current.Key
                    });
                }
                return res;
            }
        }

        public List<string> GetStreamAssetFilePath(string directoryName,EResType type)
        {
            List<string> _list = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(directoryName);
            FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".manifest") || files[i].Name.EndsWith(".manifest.meta") ||
                    files[i].Name.EndsWith(".meta"))
                    continue;

                string _name = FilePathTools.ConvertFilePathToBackslashStyle(files[i].FullName);
                _name = _name.Replace(Util.DataPath,"");

                string fileName = _name.Substring(0, _name.IndexOf('.'));
                
                _list.Add(fileName);
            }

            return _list;
        }


        public void AddAtlas(UGUIAtlas atlas)
        {
            if (atlas == null || atlas.CachedSprites == null)
            {
                Debug.LogError("AddAtlas called but atlas == null || atlas.CachedSprites == null");
                return;
            }

            for (int i = 0; i < atlas.CachedSprites.Count; i++)
            {
                var sprite = atlas.CachedSprites[i];
                if (sprite == null)
                {
                    Debug.LogError("atlas.CachedSprites[i] is null index is " + i);
                    continue;
                }

                string sName = sprite.name;
                if (_cachedAllSprite.ContainsKey(sprite.name))
                {
                    Debug.LogErrorFormat("Sprite name {0} duplicated !! in atlas {1}", sprite.name, atlas.name);
                    continue;
                }

                _cachedAllSprite.Add(sprite.name, sprite);
                _cachedAllSpriteAtlasReleation.Add(sName, atlas.name);
            }

            SDDebug.LogErrorFormat("打包完成后的_cachedAllSpriteAtlasReleation:{0}", _cachedAllSpriteAtlasReleation.Count);
        }

#if UNITY_EDITOR
        public T DirectLoadAseetByName<T>(string name,EResType type) where T : Object
        {
            string dataPath =  BuildToolsConstDefine.GetBuildingAssetPath(type);
            string assetPath = string.Format("{0}/{1}", dataPath, name);
            T res = AssetDatabase.LoadMainAssetAtPath(assetPath) as T;
            return res;
        }
#endif


    }
}

