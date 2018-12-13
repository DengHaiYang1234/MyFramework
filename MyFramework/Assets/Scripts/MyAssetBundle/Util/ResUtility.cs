
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MyAssetBundleEditor
{
    public class ResUtility
    {

        public static string GetPlatformName()
        {
#if UNITY_EDITOR
            return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
            return GetPlatformForAssetBundles(Application.platform);
#endif
        }

        static string GetPlatformForAssetBundles(RuntimePlatform platform)
        {
            if (platform == RuntimePlatform.Android)
            {
                return "Android";
            }

            if (platform == RuntimePlatform.IPhonePlayer)
            {
                return "iOS";
            }

            if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor)
            {
                return "Windows";
            }

            return null;
        }

#if UNITY_EDITOR
        static string GetPlatformForAssetBundles(BuildTarget target)
        {
            if (target == BuildTarget.Android)
            {
                return "Android";
            }

            if (target == BuildTarget.iOS)
            {
                return "iOS";
            }

            if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
            {
                return "Windows";
            }

            return null;
        }
#endif
    }
}

