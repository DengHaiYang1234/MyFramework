using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Res
{
    public class AssetPathContainer
    {
        public class ResPathData
        {
            public string AssetPath;
            public string FilePath;


            public ResPathData(string filePath)
            {
                FilePath = filePath;
                AssetPath = FilePathTools.GetAssetPathByFilePath(FilePath, Application.dataPath);
            }
        }


        public readonly string FolderName;
        private List<ResPathData> _pathDataList;


        public int Count
        {
            get { return _pathDataList.Count; }
        }


        public ResPathData this[int index]
        {
            get { return GetItem(index); }
        }

        public ResPathData GetItem(int index)
        {
            if (_pathDataList == null || index >= _pathDataList.Count)
            {
                Console.WriteLine("GetItem Called but _pathDataList == null || index >= _pathDataList.Count!");
                return null;
            }

            return _pathDataList[index];
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="folderName"></param>
        public AssetPathContainer(string folderName)
        {
            FolderName = folderName;
            _pathDataList = new List<ResPathData>();
        }

        /// <summary>
        /// 获取资源路径
        /// </summary>
        /// <param name="dirs"></param>
        public void InitAssetPathList(DirectoryInfo[] dirs)
        {
            if (dirs == null)
                return;
            for (int i = 0; i < dirs.Length; i++)
            {
                var item = dirs[i];
                if (CheckIsTargetFloder(item))  //添加指定文件路径
                {
                    _pathDataList.Add(new ResPathData(FilePathTools.ConvertFilePathToBackslashStyle(item.FullName)));
                }
            }
        }

        /// <summary>
        /// 检查是否是需求的目标文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool CheckIsTargetFloder(DirectoryInfo path)
        {
            return path.Name.Contains(FolderName);
        }
    }
}


