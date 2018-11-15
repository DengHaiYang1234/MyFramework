using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LuaInterface;

namespace MyFramework
{
    public class LuaLoader : LuaFileUtils
    {
        public LuaLoader()
        {
            instance = this;
            beZip = AppConst.LuaBunldeMode;
        }


        public void AddBundle(string bundleName)
        {
            if (!bundleName.EndsWith(AppConst.BundleName))
                bundleName += AppConst.BundleName;

            string url = Util.DataPath + bundleName.ToLower();
            if (File.Exists(url))
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(url);
                if (bundle != null)
                {
                    bundleName = bundleName.Replace("lua/", "").Replace(AppConst.BundleName, "");
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

