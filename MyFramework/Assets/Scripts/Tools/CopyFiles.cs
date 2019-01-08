using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class CopyFiles
{
    //文件
    static List<string>  files = new List<string>();

    /// <summary>
    /// 拷贝文件
    /// </summary>
    /// <param name="destDir"> 目标目录 </param>
    /// <param name="sourceDirs"> 源目录 </param>
    public static void Copy(string destDir, string[] sourceDirs, bool isShowProgress = true)
    {
        for (int dirIndex = 0; dirIndex < sourceDirs.Length; dirIndex++)
        {
            files.Clear();

            string dataPath = sourceDirs[dirIndex].ToLower();
            Recursive(dataPath);
            int n = 0;
            for (int i = 0; i < files.Count; i++)
            {
#if UNITY_EDITOR
                if (isShowProgress && EditorUtility.DisplayCancelableProgressBar(string.Format("In Copy Resources... [{0}/{1}]", i, files.Count),
files[i], i * 1f / files.Count))
#endif
                    if (files[i].EndsWith(".meta"))
                        continue;

                string newFile = files[i].Replace(dataPath, "");
                string newPath = destDir + newFile;
                string path = Path.GetDirectoryName(newPath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (File.Exists(newPath))
                    File.Delete(newPath);

                File.Copy(files[i], newPath, true);
            }

#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif
        }
    }

    private static void Recursive(string path)
    {
        string[] _files = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string fileName in _files)
        {
            string ext = Path.GetExtension(fileName);
            if (ext.Equals(".meta"))
                continue;
            files.Add(fileName.Replace('\\', '/'));
        }

        foreach (string dir in dirs)
        {
            if (dir.EndsWith(".svn") || dir.EndsWith(".svn\\") || dir.EndsWith(".svn/"))
                continue;
            Recursive(dir);
        }
    }
}
