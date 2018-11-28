
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Res
{
    public class ResourceManifest : ScriptableObject
    {

        [SerializeField]
        public List<SDAssetInfo> AssetInfoList = new List<SDAssetInfo>();

        [SerializeField]
        public List<SpriteAtlasRelation> SpriteAtlasRelationData = new List<SpriteAtlasRelation>();

        [NonSerialized]
        private Dictionary<string, string> _cachedSpriteAtlasRelationDic;

        [NonSerialized]
        private Dictionary<string, SDAssetInfo> _cachedAssetInfoListDic;



        private Dictionary<string, List<ushort>> _referenceDataDic;

        public void MappingAllAndClearRedundancy()
        {
            MappingTheSpriteAndAtlasRelation();
            MappingTheAssetInfo();

            SpriteAtlasRelationData = null;
            //AssetInfoList = null;
        }

        public void MappingTheAssetInfo()
        {
            if (AssetInfoList == null)
            {
                Debug.LogError("MappingTheAssetInfo called but AssetInfoList is null!");
                return;
            }
            _cachedAssetInfoListDic = new Dictionary<string, SDAssetInfo>();
            _referenceDataDic = new Dictionary<string, List<ushort>>();
            for (int i = 0; i < AssetInfoList.Count; i++)
            {
                var item = AssetInfoList[i];
                if (item == null || string.IsNullOrEmpty(item.AssetName))
                {
                    continue;
                }
                item.AssetId = (ushort)i;
                if (item.Depends != null)
                {
                    for (int j = 0; j < item.Depends.Count; j++)
                    {
                        var dependItem = item.Depends[j];
                        List<ushort> refList = null;
                        if (!_referenceDataDic.TryGetValue(dependItem, out refList))
                        {
                            refList = new List<ushort>();
                            _referenceDataDic.Add(dependItem, refList);
                        }
                        refList.Add(item.AssetId);
                    }
                }

                if (_cachedAssetInfoListDic.ContainsKey(item.AssetName))
                {
                    Debug.LogErrorFormat("Asset n duplicated n is {0} type is {1}", item.AssetName, item.ResType);
                    continue;
                }
                _cachedAssetInfoListDic.Add(item.AssetName, item);
            }
        }

        public List<ushort> GetRefMeList(string aName)
        {
            List<ushort> res = null;
            _referenceDataDic.TryGetValue(aName, out res);
            return res;
        }

        public SDAssetInfo GetAssetInfoByName(string n)
        {
            if (string.IsNullOrEmpty(n))
            {
                SDDebug.LogError("GetAssetInfoByName called but n is null or empty!!@!");
                return null;
            }
            if (_cachedAssetInfoListDic == null)
            {
                Debug.LogError("GetAssetInfoByName called but _cachedAssetInfoListDic is null!");
                return null;
            }
            SDAssetInfo res;
            if (!_cachedAssetInfoListDic.TryGetValue(n, out res))
            {
                Debug.LogErrorFormat("GetAssetInfoByName called but n is invalid!  n is {0}**", n);
            }
            return res;
        }

        public string GetAtlasNameBySpriteName(string spriteName)
        {
            if (_cachedSpriteAtlasRelationDic == null)
            {
                SDDebug.LogError("GetAtlasNameBySpriteName called but _cachedSpriteAtlasRelationDic is null!");
                return null;
            }
            string res = null;
            if (!_cachedSpriteAtlasRelationDic.TryGetValue(spriteName, out res))
            {
                SDDebug.LogError("GetAtlasNameBySpriteName called but spriteName is invalid!  spriteName is " + spriteName);
            }
            return res;
        }

        public SDAssetInfo GetAssetInfoById(ushort id)
        {
            if (AssetInfoList == null)
            {
                SDDebug.LogError("GetAssetInfoById called but AssetInfoList is null!");
                return null;
            }
            if (AssetInfoList.Count <= id)
            {
                SDDebug.LogErrorFormat("GetAssetInfoById called but id {0}  is invalid!,AssetInfoList.Count is {1}", id, AssetInfoList.Count);
                return null;
            }
            return AssetInfoList[id];
        }

        public List<ushort> GetAssetBeRefList(string n)
        {
            if (_referenceDataDic == null)
            {
                SDDebug.LogError("GetAssetBeRefList called but _referenceDataDic is null!");
                return null;
            }
            List<ushort> resList = null;
            if (!_referenceDataDic.TryGetValue(n, out resList))
            {
                SDDebug.LogErrorFormat("GetAssetBeRefList called but assetName {0}  is invalid!", n);
                return null;
            }
            return resList;
        }

        private void MappingTheSpriteAndAtlasRelation()
        {
            if (SpriteAtlasRelationData == null)
            {
                Debug.LogError("MappingTheSpriteAndAtlasRelation called but SpriteAtlasRelationData is null!");
                return;
            }
            _cachedSpriteAtlasRelationDic = new Dictionary<string, string>();
            for (int i = 0; i < SpriteAtlasRelationData.Count; i++)
            {
                var item = SpriteAtlasRelationData[i];
                if (item == null)
                {
                    continue;
                }

                if (_cachedSpriteAtlasRelationDic.ContainsKey(item.SpriteName))
                {
                    Debug.LogError("Sprite name duplicated name is " + item.SpriteName);
                    continue;
                }
                _cachedSpriteAtlasRelationDic.Add(item.SpriteName, item.AtlasName);
            }
        }
    }

    [Serializable]
    public class SpriteAtlasRelation
    {
        public string AtlasName;
        public string SpriteName;
    }

    [Serializable]
    public class AvatarSpineTextureRelation
    {
        public string MatName;
        public string TextureName;
    }

    [Serializable]
    public class SDAssetInfo
    {
        public string AssetName;
        public EResType ResType;
        public string Md5;
        public List<string> Depends;
        public List<string> RelationMainAsset;

        private ushort _assetId;

        public ushort AssetId
        {
            get { return _assetId; }
            set { _assetId = value; }
        }
    }

}
