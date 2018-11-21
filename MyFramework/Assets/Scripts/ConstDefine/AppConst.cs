using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFramework
{
    public class AppConst
    {
        //程序名称(项目目录变更需变化)
        public const string AppName = "MyFrameworkHotFix";
        //资源扩展名
        public const string BundleName = ".assetbundle";
        //临时目录
        public const string LuaTempDir = "LuaTempDir/";
        
        //!调试模式-用于内部测试
        public static bool DebugMode = false;

        //lua代码Assetbundle模式
        public static bool LuaBunldeMode = false;

        //Lua字节码模式
        public static bool LuaByteMode = false;

        //更新模式(开启热更必须关闭 DebugMode)
        public static bool UpdateMode = true;
        
        //下载地址
        public static string WebUrl = "http://192.168.93.192:8081/StreamingAssets/";

        //热更新根目录
        public static string HotFixRoot = Application.dataPath + "/";

        public const string StandaloneWindows = "PC";
        public const string Android = "Android";
        public const string Ios = "Ios";
        public const string BinFolderName = "Bin";
    }
}

