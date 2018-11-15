using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFramework;

namespace MyFramework
{
    /// <summary>
    /// 保存Debug信息至本地
    /// </summary>
    public class SDRootPath : ClassicalSingleton<SDRootPath>
    {
        public const string RootFolderName = "SDRes";
        private string _buildInPackageRoot;
        private string _rootPath;
        private string _assetPath;
        private string _logFolderPath = null;
        private string _curLogFilePath = null;


        public string BuildPackageRoot
        {
            get { return _buildInPackageRoot; }
        }


        public void Init()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.WindowsPlayer)
            {
                _rootPath = Application.dataPath + "/../" + RootFolderName;
                _buildInPackageRoot = string.Format("file://{0}/{1}", Application.streamingAssetsPath,
                    Util.GetPlatfromFoldername());
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                _rootPath = Application.persistentDataPath + "/" + RootFolderName;
                _buildInPackageRoot = string.Format("{0}/{1}", Application.streamingAssetsPath,
                    Util.GetPlatfromFoldername());
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _rootPath = Application.persistentDataPath + "/" + RootFolderName;
                string.Format("{0}/{1}", Application.streamingAssetsPath,
                    Util.GetPlatfromFoldername());
            }
            else
            {
                SDDebug.LogErrorFormat("_rootPath set error ! set to {0}!", Application.dataPath);
                _rootPath = Application.dataPath;
            }

            _assetPath = string.Format("{0}/{1}", _rootPath, AppConst.BinFolderName);
            _logFolderPath = string.Format("{0}/{1}", _assetPath, "log");
            _curLogFilePath = string.Format("{0}/outLog_{1}.txt", _logFolderPath,
                DateTime.Now.ToString("yy_MM_dd_hh_mm_ss"));

        }


        public string GetBinFileRootPath()
        {
            return _assetPath;
        }

        public string GetLogFolderPath()
        {
            return _logFolderPath;
        }

        public string GetCurLogFilePath()
        {
            return _curLogFilePath;
        }

    }
}

