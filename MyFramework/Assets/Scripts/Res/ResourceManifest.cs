using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Res
{
    /// <summary>
    /// 可扩展类.主要用来存储数据
    /// </summary>
    public class ResourceManifest : ScriptableObject
    {
        [SerializeField] public List<SpriteAtlasRelation> SpriteAtlasRelation = new List<SpriteAtlasRelation>();

        [NonSerialized] private Dictionary<string, string> _cachedSpriteAtlasRelationDic;

        public void MappingAllData()
        {
            MappingTheSpriteAndAtlasRelation();
        }

        void MappingTheSpriteAndAtlasRelation()
        {
            if (SpriteAtlasRelation == null)
            {
                Debug.LogError("MappingTheSpriteAndAtlasRelation called but SpriteAtlasRelationData is null!");
                return;
            }

            _cachedSpriteAtlasRelationDic = new Dictionary<string, string>();
            for (int i = 0; i < SpriteAtlasRelation.Count; i++)
            {
                var item = SpriteAtlasRelation[i];
                if (item == null)
                    continue;

                if (_cachedSpriteAtlasRelationDic.ContainsKey(item.SpriteName))
                {
                    Debug.LogError("Sprite name duplicated name is " + item.SpriteName);
                    continue;
                }

                _cachedSpriteAtlasRelationDic.Add(item.SpriteName,item.AtlasName);
            }
        }

    }

    [SerializeField]
    public class SpriteAtlasRelation
    {
        public string AtlasName;
        public string SpriteName;
    }



}


