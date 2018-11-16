using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using MyFramework;
using System.Diagnostics;

public class PackageBuild : MonoBehaviour
{
    //文件路径
    static List<string> paths = new List<string>();
    //文件
    static List<string> files = new List<string>();
    //打包集合
    static List<AssetBundleBuild> bundles = new List<AssetBundleBuild>();
    //项目路径
    static string AppdataPath
    {
        get { return Application.dataPath.ToLower(); }
    }
    //流文件路径
    static string AppStreamPath
    {
        get { return AppdataPath + "/StreamingAssets/"; }
    }

    #region windows资源打包入口
    [MenuItem("AssetsBundle/Build Windows Resource", false, 100)]
    public static void BuildWindowsResource()
    {
        BuildAssetResource(BuildTarget.StandaloneWindows, false);
    }
    #endregion

    [MenuItem("AssetsBundle/Build Android Resource", false, 102)]
    public static void BuildAndroidResource()
    {
        BuildAssetResource(BuildTarget.Android, false);
    }

    #region  根据选择平台的不同 开始打包资源
    public static void BuildAssetResource(BuildTarget target, bool delfold = true)
    {
        if (Directory.Exists(Util.DataPath) && delfold)
            Directory.Delete(Util.DataPath, true);
        string streamPath = Application.streamingAssetsPath;
        if (Directory.Exists(streamPath) && delfold)
            Directory.Delete(streamPath, true);

        AssetDatabase.Refresh();

        if (AppConst.LuaBunldeMode)
            HandleLuaBundle();
        else
            HandleCopyLuaFile();

        GenerateVersion();
        BuildFileMD5();

        BuildAssetsResource(target);
        Util.LogErr("若需拷贝资源至服务器。请稍等刷新一会在拷贝！！！！！！");
        AssetDatabase.Refresh();
    }
    #endregion

    #region ---------------------(Lua资源打包模式)---------------------
    #region 1.(资源打包模式)开始打包并拷贝资源
    static void HandleLuaBundle()
    {
        BuildLuaAssetsBundles();
        string luaPath = AppStreamPath + "lua/";
        string[] luaPaths =
        {
            CustomSettings.luaDir,
            CustomSettings.toLuaDir
        };

        CopyLuaFiles(luaPath, luaPaths);
    }
    #endregion

    #region 2.拷贝项目Lua至临时目录 并改为bytes文件 开始Bundle
    static void BuildLuaAssetsBundles()
    {
        CreatStreamDir("lua/");
        //CreatStreamDir(AppConst.LuaTempDir);

        string dir = Application.persistentDataPath;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string streamDir = Application.dataPath + "/" + AppConst.LuaTempDir;
        CopyLuaBytesFiles(CustomSettings.luaDir, streamDir);
        CopyLuaBytesFiles(CustomSettings.toLuaDir, streamDir);

        AssetDatabase.Refresh();
        string[] dirs = Directory.GetDirectories(streamDir, "*", SearchOption.AllDirectories);
        for (int i = 0; i < dirs.Length; i++)
        {
            string str = dirs[i].Remove(0, streamDir.Length);
            BuildLuaBundle(str);
        }

        BuildLuaBundle(null);
        Directory.Delete(streamDir, true);
        AssetDatabase.Refresh();
    }

    #endregion

    #region 3.在StreamingAssets目录中创建细分资源目录
    static void CreatStreamDir(string dir)
    {
        dir = Application.streamingAssetsPath + "/" + dir;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }
    #endregion

    #region 4.将项目Lua文件全部拷贝至目标目录  并修改为bytes文件.便于打包
    static void CopyLuaBytesFiles(string sourceDir, string destDir, bool appendext = true)
    {
        if (!Directory.Exists(sourceDir))
            return;

        string[] _files = Directory.GetFiles(sourceDir, "*.lua", SearchOption.AllDirectories);
        int len = sourceDir.Length;

        if (sourceDir[len - 1] == '/' || sourceDir[len - 1] == '\\')
        {
            len--;
        }

        for (int i = 0; i < _files.Length; i++)
        {
            string str = _files[i].Remove(0, len);
            string dest = destDir + str;
            if (appendext)
                dest += ".bytes";
            string dir = Path.GetDirectoryName(dest);
            Directory.CreateDirectory(dir);

            if (AppConst.LuaByteMode)
            {
                EncodeLuaFile(_files[i], dest);
            }
            else
            {
                File.Copy(_files[i], dest, true);
            }
        }
    }
    #endregion

    #region 5.编码Lua文件（功能暂且还不知晓）
    static void EncodeLuaFile(string srcFile, string outFile)
    {
        if (!srcFile.ToLower().EndsWith(".lua"))
        {
            File.Copy(srcFile, outFile, true);
            return;
        }

        bool isWin = true;
        string luaExe = string.Empty;
        string args = string.Empty;
        string exeDir = string.Empty;
        string currDir = Directory.GetCurrentDirectory();
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            isWin = true;
            luaExe = "luajit.exe";
            args = "-b" + srcFile + " " + outFile;
            exeDir = AppdataPath.Replace("assets", "") + "LuaEncoder/luajit";
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            isWin = false;
            luaExe = "./luac";
            args = "-o " + outFile + " " + srcFile;
            exeDir = AppdataPath.Replace("assets", "") + "LuaEncoder/luavm";
        }

        Directory.SetCurrentDirectory(exeDir);
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = luaExe;
        info.Arguments = args;
        info.WindowStyle = ProcessWindowStyle.Hidden;
        info.UseShellExecute = isWin;
        info.ErrorDialog = true;

        SDDebug.Log(info.FileName + " " + info.Arguments);

        Process pro = Process.Start(info);
        pro.WaitForExit();
        Directory.SetCurrentDirectory(currDir);
    }
    #endregion

    #region 6. 开始打包 将同一目录下的Lua文件 打包时打成同一个AB
    static void BuildLuaBundle(string dir)
    {
        bundles.Clear();

        BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets
            | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle;

        string path = "Assets/" + AppConst.LuaTempDir + dir;
        string[] _files = Directory.GetFiles(path, "*.lua.bytes");
        AssetBundleBuild build = new AssetBundleBuild();
        string bundleName = "lua" + AppConst.BundleName;
        if (dir != null)
        {
            dir = dir.Replace('\\', '_').Replace('/', '_');
            if (dir.EndsWith("lpeg") || dir.EndsWith("protobuf") || dir.EndsWith("socket"))
                return;

            bundleName = "lua_" + dir.ToLower() + AppConst.BundleName;
        }

        build.assetBundleName = bundleName;
        build.assetNames = _files;
        bundles.Add(build);

        if (_files.Length > 0)
        {
            string outPut = Application.streamingAssetsPath + "/lua/";
            if (File.Exists(outPut))
                File.Delete(outPut);

            BuildPipeline.BuildAssetBundles(outPut, bundles.ToArray(), options, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
        }
    }
    #endregion

    #region 7.递归遍历得到所有lua文件
    static void Recursive(string path)
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

    #region 8.将项目中所有Lua文件全部拷贝至StreamingAssets
    static void CopyLuaFiles(string destDir, string[] sourcesDirs)
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

                if (AppConst.LuaByteMode)
                    EncodeLuaFile(f, newPath);
                else
                {
                    File.Copy(f, newPath, true);
                }

                UpdateProgress(n++, files.Count, newPath);

            }
        }

        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }
    #endregion

    #region 9.更新提示面板信息

    static void UpdateProgress(int progress, int progressMax, string desc)
    {
        string title = "Progressing .. [" + progress + " - " + progressMax + "]";
        float value = (float)progress / (float)progressMax;
        EditorUtility.DisplayProgressBar(title, desc, value);
    }
    #endregion

    #region 10.打包其他项目资源（如场景，模型，特效等等）
    static void BuildAssetsResource(BuildTarget target)
    {
        string assetPath = AppStreamPath;
        if (!Directory.Exists(assetPath))
            Directory.CreateDirectory(assetPath);

        BuildAssetBundleOptions options = BuildAssetBundleOptions.CompleteAssets |
                                          BuildAssetBundleOptions.CollectDependencies |
                                          BuildAssetBundleOptions.DeterministicAssetBundle;
        
        BuildPipeline.BuildAssetBundles(assetPath, BuildAssetBundleOptions.None, target);
        AssetDatabase.Refresh();
    }

    #endregion

    #endregion

    #region---------------------(非Lua打包)---------------------
    #region 拷贝lua资源至StreamingAssets
    static void HandleCopyLuaFile()
    {
        string luaPath = AppStreamPath + "lua";

        if (!Directory.Exists(luaPath))
            Directory.CreateDirectory(luaPath);

        string[] luaPaths =
            {
                CustomSettings.luaDir,
                CustomSettings.toLuaDir
            };

        CopyLuaFiles(luaPath, luaPaths);
    }
    #endregion
    #endregion

    #region 生成版本时间
    static void GenerateVersion()
    {
        string versionPath = AppStreamPath + "/version.txt";
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
    static void BuildFileMD5()
    {
        string newFilePath = AppStreamPath + "/files.txt";
        if (File.Exists(newFilePath))
            File.Delete(newFilePath);

        paths.Clear();
        files.Clear();
        Recursive(AppStreamPath);

        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for(int i = 0; i< files.Count;i++)
        {
            string file = files[i];
            Path.GetExtension(file);
            if (file.EndsWith(".meta"))
                continue;

            string md5 = Util.MD5File(file);
            string value = file.Replace(AppStreamPath, string.Empty);
            sw.WriteLine(value + "|" + md5);
        }

        sw.Close();
        fs.Close();
    }
    #endregion
}
