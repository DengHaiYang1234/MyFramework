using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LuaInterface;
using Res;

namespace MyFramework
{
    public class LuaLoader : LuaFileUtils
    {
        public LuaLoader()
        {
            instance = this;
            beZip = FrameworkDefaultSetting.LuaBunldeMode;
        }


        public void AddBundle(string bundleName)
        {
            string url = RuntimeResPath.GetLuaAssetsDataPath + bundleName.ToLower();
            if (File.Exists(url))
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(url);
                if (bundle != null)
                {
                    base.AddSearchBundle(bundleName.ToLower(), bundle);
                }
            }
        }

        public override byte[] ReadFile(string fileNaMe)
        {
            return base.ReadFile(fileNaMe);
        }
    }
}

