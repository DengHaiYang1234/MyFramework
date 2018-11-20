using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Res
{
    public class ResPathDef
    {
        public const string RootPath = "Assets";
        public const string ResRootPath = "data";

        public const string stream = "/StreamingAssets/";

        public const string ResUGUIAtlasPackTag = "[UIAtlas]";



        public static string GetRootResAssetPath()
        {
            return string.Format("{0}/{1}",RootPath,ResRootPath);
        }

    }

}
