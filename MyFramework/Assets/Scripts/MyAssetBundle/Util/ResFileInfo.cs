using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace MyFramework
{
    public class ResFileInfo
    {
        /// <summary>
        /// 根据Path 获取当前目录下的所有文件
        /// </summary>
        /// <param name="searchPath"> 搜索路径 </param>
        /// <param name="searchPattern"> 匹配模式 </param>
        /// <param name="optin"> SearchOption </param>
        /// <returns></returns>
        public static List<string> GetFilesWithoutDirectores(string searchPath, string searchPattern, SearchOption optin)
        {
            var files = Directory.GetFiles(searchPath, searchPattern, optin);
            List<string> items = new List<string>();
            foreach (var item in files)
            {
                var assetPath = item.Replace('\\', '/');
                if (!Directory.Exists(assetPath))
                    items.Add(assetPath);
            }

            return items;
        }

        

    }
}

