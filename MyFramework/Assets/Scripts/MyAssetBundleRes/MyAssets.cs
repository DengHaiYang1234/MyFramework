using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Res
{
    /// <summary>
    /// 应用于某个类时，sealed 修饰符可阻止其他类继承自该类
    /// </summary>
    public sealed class MyAssets : MonoBehaviour
    {
        private static MyAssets instance;

        static void CheckInstace()
        {
            if (instance == null)
            {
                GameObject go = new GameObject("Assets");
                DontDestroyOnLoad(go);
                instance = go.AddComponent<MyAssets>();
            }
        }

        public static bool Initialize()
        {
            CheckInstace();
#if UNITY_EDITOR
            if (ResUtility.useEditorPrefab)
            {
                
            }
#endif
        }


        static bool InitializeBundle()
        {
            string relativePath = Path.Combine(ResUtility.AssetBundlesOutputPath, ResUtility.GetPlatformName());
            var url = 
#if UNITY_EDITOR
                relativePath + "/";
#else
                Path.Combine(Application.streamingAssetsPath, relativePath) + "/";
#endif
        }

    }
}

