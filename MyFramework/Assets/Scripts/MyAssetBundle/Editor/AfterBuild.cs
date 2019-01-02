using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MyFramework;
using UnityEditor;

namespace MyAssetBundleEditor
{
    /// <summary>
    /// 打包之后的处理
    /// </summary>
    public class AfterBuild
    {
        public AfterBuild()
        {
            Directory.Delete(BuildDefaultPath.GetLuaTempDataPath, true);
            StartCopyLuaFiles();
            GenerateVersion();
            GenerateFileMD5();
            SaveAndRefresh();
        }

        #region 拷贝Lua脚本
        //文件路径
        List<string> paths = new List<string>();
        //文件
        List<string> files = new List<string>();
        private void StartCopyLuaFiles()
        {
            string destDir = BuildDefaultPath.GetBuildLuaPath;
            string[] sourcesDirs =
            {
                BuildDefaultPath.GetLuaDataPath,
                BuildDefaultPath.GetToLuaDataPath,
            };

            CopyLuaFiles(destDir, sourcesDirs);
        }

        private void CopyLuaFiles(string destDir, string[] sourcesDirs)
        {
            for (int i = 0; i < sourcesDirs.Length; i++)
            {
                paths.Clear();
                files.Clear();

                string luaDataPath = sourcesDirs[i].ToLower();
                Recursive(luaDataPath);
                int n = 0;
                foreach (string f in files)
                {
                    if (f.EndsWith(".meta"))
                        continue;

                    string newFile = f.Replace(luaDataPath, "");
                    string newPath = destDir + newFile;
                    string path = Path.GetDirectoryName(newPath);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    if (File.Exists(newPath))
                        File.Delete(newPath);

                    File.Copy(f, newPath, true);
                }
            }
        }

        private void Recursive(string path)
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
                paths.Add(dir.Replace('\\', '/'));
                Recursive(dir);
            }
        }
        #endregion

        #region 生成版本时间
        static void GenerateVersion()
        {
            string versionPath = BuildDefaultPath.GetAssetBundlePath + "version.txt";
            if (File.Exists(versionPath))
                File.Delete(versionPath);

            FileStream fs = new FileStream(versionPath, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);
            string currentTime = System.DateTime.Now.ToString("yyyymmddhhmmss");
            sw.WriteLine(currentTime);
            sw.Close();
            fs.Close();
        }
        #endregion

        #region 记录文件对应的MD5码
        private void GenerateFileMD5()
        {
            string newFilePath = BuildDefaultPath.GetAssetBundlePath + "files.txt";
            if (File.Exists(newFilePath))
                File.Delete(newFilePath);

            paths.Clear();
            files.Clear();
            Recursive(BuildDefaultPath.GetAssetBundlePath);

            FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < files.Count; i++)
            {
                if (EditorUtility.DisplayCancelableProgressBar(string.Format("PackingLuaFile... [{0}/{1}]", i, files.Count),
    files[i], i * 1f / files.Count))
                {
                    break;
                }

                string file = files[i];
                Path.GetExtension(file);
                if (file.EndsWith(".meta") || file.EndsWith(".manifest"))
                    continue;

                string md5 = Util.MD5File(file);
                string value = file.Replace(BuildDefaultPath.GetAssetBundlePath, string.Empty);
                sw.WriteLine(value + "|" + md5);
            }

            sw.Close();
            fs.Close();
            EditorUtility.ClearProgressBar();
        }
        #endregion

        private void SaveAndRefresh()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

