using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFramework;
using System;
using System.IO;
using UnityEditor;

namespace MyAssetBundleEditor
{
    /// <summary>
    /// 打包Lua
    /// </summary>
    public class BuildLua : BaseBuild
    {

        public BuildLua()
        {

        }

        public BuildLua(string path, string pattern, SearchOption option, string assetBundleName) : base(path, pattern, option)
        {

        }

        public override void Build()
        {
            string perDir = Application.persistentDataPath;
            if (!Directory.Exists(perDir))
                Directory.CreateDirectory(perDir);

            string tempDir = BuildDefaultPath.GetLuaTempDataPath;
            CreatLuaBytesFiles(searchPath, tempDir);
            CreatLuaBytesFiles(BuildDefaultPath.GetToLuaDataPath, tempDir);
            SaveAndRefresh();


            string[] dirs = Directory.GetDirectories(tempDir, "*", SearchOption.AllDirectories);
            for (int i = 0; i < dirs.Length; i++)
            {
                if (EditorUtility.DisplayCancelableProgressBar(string.Format("PackingLuaFile... [{0}/{1}]", i, dirs.Length),
                    dirs[i], i * 1f / dirs.Length))
                {
                    break;
                }

                string dir = dirs[i].Remove(0, tempDir.Length);
                BuildLuaFile(dir);
            }

            BuildLuaFile(null);
            SaveAndRefresh();
        }

        public override string GetAssetBundleName(string assetPath)
        {
            throw new NotImplementedException();
        }

        private void CreatLuaBytesFiles(string searchPath, string destDir, bool appendext = true)
        {
            if (!Directory.Exists(searchPath))
            {
                Debug.LogError("CreatLuaBytesFiles Is Called.But 【searchPath】 is Not Exists!    searchPath:   " + searchPath);
                return;
            }

            string[] files = Directory.GetFiles(searchPath, searchPattern, option);

            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i].Replace(searchPath, "");
                string dest = destDir + file;
                if (appendext)
                    dest += ".bytes";

                string dir = Path.GetDirectoryName(dest);
                Directory.CreateDirectory(dir);

                File.Copy(files[i], dest, true);
            }
        }

        private void BuildLuaFile(string dir)
        {
            string path = BuildDefaultPath.GetLuaTempDataPath + dir;
            string[] files = Directory.GetFiles(path, "*.lua.bytes");

            string _bundleName = "lua";

            for (int i = 0; i < files.Length; i++)
                files[i] = files[i].Replace('\\', '/');

            if (dir != null)
            {
                dir = dir.Replace('\\', '_').Replace('/', '_');
                _bundleName = "lua_" + dir.ToLower();
            }

            _bundleName = BuildDefaultPath.GetLuaTempDataPath + "/" + _bundleName;

            AssetBundleBuild build = new AssetBundleBuild();
            bundleName = BuildDefaultPath.BuildAssetBunldNameWithAssetPath(path);
            build.assetBundleName = _bundleName;
            build.assetNames = files;
            builds.Add(build);
        }

        


        private void SaveAndRefresh()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

