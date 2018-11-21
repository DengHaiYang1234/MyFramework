using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Res
{
    public class BuildingAssetHolder
    {
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

        //缓存所有Sprite
        private Dictionary<string, Sprite> _cachedAllSprite;
        //图片所对应图集名称
        private Dictionary<string,string> _cachedAllSpriteAtlasReleation;
        //缓存所有UIPrefabs
        private Dictionary<string, GameObject> _cachedAllUIPrefabs;


        public BuildingAssetHolder()
        {
            _cachedAllSprite = new Dictionary<string, Sprite>();
            _cachedAllSpriteAtlasReleation = new Dictionary<string, string>();
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

                _cachedAllSprite.Add(sprite.name,sprite);
                _cachedAllSpriteAtlasReleation.Add(sName, atlas.name);
            }
        }

        

    }
}

