using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MyAssetBundleEditor
{
    public class BuildPackMethod
    {
        private static PackageMethod pkgMethod;

        public BuildPackMethod(bool isClear)
        {
            pkgMethod = AssetDatabase.LoadAssetAtPath<PackageMethod>(BuildDefaultPath.GetBuildMethodFloderPath());
            if (pkgMethod != null)
            {
                pkgMethod.Clear();
                AssetDatabase.DeleteAsset(BuildDefaultPath.GetBuildMethodFloderPath());
            }
            SaveAndRefresh();
        }

        public BuildPackMethod(Object SelectionObj,string buildMethod,bool isClear = false)
        {
            pkgMethod = AssetDatabase.LoadAssetAtPath<PackageMethod>(BuildDefaultPath.GetBuildMethodFloderPath());
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
        
        private static void CreatScriptableObject(string name,string buildMethod,string searchPath,string searchPattern, SearchOption option)
        {
            string path = BuildDefaultPath.GetBuildMethodFloderPath();
            var asset = ScriptableObject.CreateInstance<PackageMethod>();
            asset.packagInfos.Add(SetData(name,buildMethod,searchPath,searchPattern,option));
            AssetDatabase.CreateAsset(asset, path);
        }

        private static void UpdatePackgInfos(string name, string buildMethod, string searchPath, string searchPattern, SearchOption option)
        {
            pkgMethod.RemovePackagInfoByAssetName(name);
            pkgMethod.packagInfos.Add(SetData(name, buildMethod, searchPath, searchPattern, option));
        }

        private static void AddPackgInfos(string name, string buildMethod, string searchPath, string searchPattern,
            SearchOption option)
        {
            pkgMethod.packagInfos.Add(SetData(name, buildMethod, searchPath, searchPattern, option));
        }

        private static BuildPackageInfo SetData(string name, string buildMethod, string searchPath, string searchPattern, SearchOption option)
        {
            BuildPackageInfo pInfo = new BuildPackageInfo
            {
                assetName = name,
                BuildType = buildMethod,
                searchPath = searchPath,
                searchPattern = searchPattern,
                option = option,
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

        private static void SaveAndRefresh()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

    }
}

