using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Res
{
    public class UGUIAtlas : ScriptableObject
    {
        [SerializeField] public List<Sprite> CachedSprites;
        [NonSerialized] private bool _hasInited = false;
        [NonSerialized] private Dictionary<string, Sprite> _cachedSpritesDic;

        private Dictionary<string, Sprite> CachedSpritesDic
        {
            get
            {
                if (!_hasInited)
                {
                    InitData();
                }
                return _cachedSpritesDic;
            }
        }


        private void InitData()
        {
            _cachedSpritesDic = new Dictionary<string, Sprite>();

            if (CachedSprites == null)
            {
                SDDebug.Log("CachedSprites is null! ");
                return;
            }

            for (int i = 0; i < CachedSprites.Count; i++)
            {
                var tempSprite = CachedSprites[i];
                if (tempSprite == null)
                {
                    SDDebug.LogErrorFormat("Srite is null,in atlas {0},index {1}",name,i);
                    continue;
                }

                if (!_cachedSpritesDic.ContainsKey(tempSprite.name))
                    _cachedSpritesDic.Add(tempSprite.name, tempSprite);
                else
                    SDDebug.LogError(string.Format("Sprite name {0} duplicated.!",tempSprite.name));
            }

            _hasInited = true;
        }

        public Sprite GetSpriteByName(string spriteName)
        {
            if (CachedSpritesDic == null)
            {
                SDDebug.LogError("GetSpriteByName called but CachedSpritesDic is null!");
                return null;
            }

            Sprite outSprite = null;
            if (!CachedSpritesDic.TryGetValue(spriteName, out outSprite))
            {
                SDDebug.LogError(string.Format("GetSpriteByName called but spriteName {0} is invalid",spriteName));
            }

            return outSprite;
        }

    }
}


