using System.Runtime.InteropServices;
using System;

namespace MyAssetBundleEditor
{
    /*
     [ComVisibleAttribute] 指示应用该属性的对象是否对COM可见

     COM = Component Object Model，微软的上一代编程模型

     */

     /// <summary>
     /// 打包方式
     /// </summary>
    [ComVisible(true)]
    [Serializable]
    public enum BuildType
    {
        BuildAssetsWithAssetBundleName,
        BuildAssetsWithDirectroyName,
        BuildAssetsWithFilename,
        BuildLua,
        None,
    }
}

