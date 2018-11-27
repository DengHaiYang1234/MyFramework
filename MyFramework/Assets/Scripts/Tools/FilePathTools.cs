using System.Collections;
using System.Collections.Generic;
using MyFramework;
using UnityEngine;

public class FilePathTools 
{
    public static string GetFilePathByAssetPath(string path, string dataPath)
    {
        if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(dataPath))
            return "";

        string relDataPath = ConvertFilePathToBackslashStyle(dataPath);
        return string.Format("{0}{1}", relDataPath, path.Substring(6, path.Length - 6)); //不要前6位
    }


    /// <summary>
    /// 将"\\"替换为"/"
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public static string ConvertFilePathToBackslashStyle(string r)
    {
        if (string.IsNullOrEmpty(r))
            return "";

        return r.Replace('\\', '/');
    }

    public static string GetAssetPathByFilePath(string path,string dataPath)
    {
        if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(dataPath))
        {
            return "";
        }

        string realfilePath = ConvertFilePathToBackslashStyle(path);
        string realDataPath = ConvertFilePathToBackslashStyle(dataPath);
        int length = realDataPath.Length;
        return realfilePath.Substring(length - 6, realfilePath.Length - length + 6);
    }

    public static string GetStreamAssetPathByFilePath(string dataPath)
    {
        if (string.IsNullOrEmpty(dataPath))
        {
            return "";
        }
        dataPath = dataPath.Substring(dataPath.IndexOf('/') + 1);
        return dataPath.Substring(0,dataPath.IndexOf('.'));
    }
}
