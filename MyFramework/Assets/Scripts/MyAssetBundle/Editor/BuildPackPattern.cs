using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MyAssetBundleEditor
{
    public class BuildPackPattern
    {
        private static PackagePattern pkgMethod;

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="isClear"></param>
        public BuildPackPattern(bool isClear)
        {
            pkgMethod = AssetDatabase.LoadAssetAtPath<PackagePattern>(BuildDefaultPath.GetBuildPattrenAssetPath());
            if (pkgMethod != null)
            {
                pkgMethod.Clear();
                AssetDatabase.DeleteAsset(BuildDefaultPath.GetBuildPattrenAssetPath());
            }
            SaveAndRefresh();
        }
        /// <summary>
        /// 打包指定路径
        /// </summary>
        /// <param name="SelectionObj"></param>
        /// <param name="buildMethod"></param>
        /// <param name="isClear"></param>
        public BuildPackPattern(Object SelectionObj,BuildType buildMethod)
        {
            pkgMethod = AssetDatabase.LoadAssetAtPath<PackagePattern>(BuildDefaultPath.GetBuildPattrenAssetPath());
            if (pkgMethod != null)
            {
                pkgMethod.MappingPackageData();
            }
            if (SelectionObj == null)
                return;

            string path = AssetDatabase.GetAssetPath(SelectionObj);
            if (!CheckIsVaildFolder(path))
            {
                Debug.LogErrorFormat("select path is invaild! {0}",path);
                return;
            }

            string name = path.Substring(path.LastIndexOf('/') + 1);
            string searchPattern = GetSerchPattern(name);
            if (pkgMethod == null)//创建
            {
                CreatScriptableObject(name, buildMethod, path, searchPattern, SearchOption.AllDirectories);
                return;
            } 

            if (pkgMethod.GetPackagInfoByAssetName(name) != null) //更新操作
            {
                UpdatePackgInfos(name, buildMethod, path, searchPattern, SearchOption.AllDirectories);
            }
            else //添加操作
            {
                AddPackgInfos(name, buildMethod, path, searchPattern, SearchOption.AllDirectories);
            }
            SaveAndRefresh();
        }
        /// <summary>
        /// 根据路径打包所有
        /// </summary>
        /// <param name="assetDataPath"></param>
        public BuildPackPattern()
        {
            pkgMethod = AssetDatabase.LoadAssetAtPath<PackagePattern>(BuildDefaultPath.GetBuildPattrenAssetPath());
            if (pkgMethod != null)
            {
                pkgMethod.Clear();
                AssetDatabase.DeleteAsset(BuildDefaultPath.GetBuildPattrenAssetPath());
            }



            CreatScriptableObject();
            SaveAndRefresh();
        }

        private static bool CheckIsVaildFolder(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
                return false;

            //if (!path.Contains("Assets/data"))
            //    return false;

            path = path.Substring(path.LastIndexOf('/') + 1);

            if (path[0] != '[' || path[path.Length - 1] != ']')
                return false;

            return true;
        }

        /// <summary>
        /// 所有资源采用默认方法打包
        /// </summary>
        private static void CreatScriptableObject()
        {
            var asset = ScriptableObject.CreateInstance<PackagePattern>();
            string[] dirs = Directory.GetDirectories(BuildDefaultPath.GetAssetDataPath());
            foreach (var dir in dirs)
            {
                if (CheckIsVaildFolder(dir))
                {
                    string name = dir.Substring(dir.LastIndexOf('/') + 1);
                    asset.packagInfos.Add(SetData(name, GetBuildType(name), dir,
                        GetSerchPattern(name), SearchOption.AllDirectories));
                }
            }
            AssetDatabase.CreateAsset(asset, BuildDefaultPath.GetBuildPattrenAssetPath());
        }
        
        private static void CreatScriptableObject(string name,BuildType buildMethod,string searchPath,string searchPattern, SearchOption option)
        {
            string path = BuildDefaultPath.GetBuildPattrenAssetPath();
            var asset = ScriptableObject.CreateInstance<PackagePattern>();
            asset.packagInfos.Add(SetData(name,buildMethod,searchPath,searchPattern,option));
            AssetDatabase.CreateAsset(asset, path);
        }

        private static void UpdatePackgInfos(string name, BuildType buildMethod, string searchPath, string searchPattern, SearchOption option)
        {
            pkgMethod.RemovePackagInfoByAssetName(name);
            pkgMethod.packagInfos.Add(SetData(name, buildMethod, searchPath, searchPattern, option));
        }

        private static void AddPackgInfos(string name, BuildType buildMethod, string searchPath, string searchPattern,
            SearchOption option)
        {
            pkgMethod.packagInfos.Add(SetData(name, buildMethod, searchPath, searchPattern, option));
        }

        private static BuildPackageInfo SetData(string name, BuildType buildMethod, string searchPath, string searchPattern, SearchOption option)
        {
            BuildPackageInfo pInfo = new BuildPackageInfo
            {
                assetName = name,
                BuildType = buildMethod,
                searchPath = searchPath,
                searchPattern = searchPattern,
                searchOption = option,
                bundleName = BuildDefaultPath.bundleName,
            };
            return pInfo;
        }

        private static string GetSerchPattern(string name)
        {
            switch (name)
            {
                case BuildDefaultPath.assetsAtlasFloder:
                    return "*.png";
                case BuildDefaultPath.assetsPrefabFloder:
                    return "*.prefab";
            }

            return "";
        }

        private static BuildType GetBuildType(string path)
        {
            switch (path)
            {
                case BuildDefaultPath.assetsAtlasFloder:
                    return BuildType.BuildAssetsWithDirectroyName;
                case BuildDefaultPath.assetsPrefabFloder:
                    return BuildType.BuildAssetsWithFilename;
            }
            Debug.LogError(string.Format("GetBuildDefaultPath Is Called .But return Null.Check 【path】:{0} ", path));
            return BuildType.None;
        }

        private static void SaveAndRefresh()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }


}

