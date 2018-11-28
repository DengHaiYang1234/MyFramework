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

        private static RuntimePath _instance;

        public static RuntimePath Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RuntimePath();

                return _instance;
            }
        }

        public RuntimePath()
        {
            _rootPath = Util.DataPath;
            _dataPth = ResPathDef.ResRootPath;
            _mainAssetPath = string.Format("{0}{1}/{2}", _rootPath, _dataPth, ResPathDef.AssetMainfestFolder);
        }

        public string RootPath
        {
            get { return _rootPath; }
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

