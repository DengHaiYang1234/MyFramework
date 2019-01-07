using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameworkDefaultSetting
{
    //lua代码Assetbundle模式
    public static bool LuaBunldeMode = true;
    //是否直接使用Prefab
    public static bool useEditorPrefab = false;
    //!调试模式-读取项目资源
    public static bool DebugMode = false;
    //更新模式(开启热更必须关闭 DebugMode)
    public static bool UpdateMode = true;
    //Log目录
    public const string BinFolderName = "Bin";


    //下载地址
    public static string WebUrl = "http://192.168.93.230:8081/StreamingAssets/";
}
