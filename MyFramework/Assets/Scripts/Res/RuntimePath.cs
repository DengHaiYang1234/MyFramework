using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFramework;

namespace Res
{
    public class RuntimePath
    {
        private string _mainAssetPath;
        private string _rootPath;
        private string _dataPth;
        public RuntimePath()
        {
            _rootPath = Util.DataPath;
            _dataPth = ResPathDef.ResRootPath;
            _mainAssetPath = string.Format("{0}{1}/{2}", _rootPath, _dataPth, ResPathDef.AssetMainfestFolder);
        }

        public string MainfestPath
        {
            get
            {
                return string.Format("{0}/{1}{2}", _mainAssetPath, ResPathDef.ExportMainfestAssetName,
                    ResPathDef.AssetBundleSufix);
            }
        }

    }
}

