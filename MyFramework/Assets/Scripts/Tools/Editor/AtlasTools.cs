using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


namespace Res
{

    public class UIAtlasAttribute
    {
        public string AtlasName;
        public ETextureCompressQuality Quality = ETextureCompressQuality.ShowAs16Bit;
        public ESpritePivot Pivot = ESpritePivot.Center;
    }

    public enum ETextureCompressQuality
    {
        ShowAs16Bit = 0,//16bit
        NoCompress,    //不不压缩
        Compress,     //压缩
    }

    public enum ESpritePivot
    {
        Center = 0,
        TopLeft,
        Top,
        TopRight,
        Left,
        Right,
        BottomLeft,
        Bottom,
        BottomRight,
        Custom,
    }


    public static class AtlasTools
    {
        private static TextureImporterSettings _tmpImporterSettings;
        public const string SoyAltasConfig = "AtlasConfig.txt";

        private static string _sPath;

        private static string Path
        {
            get
            {
                if (string.IsNullOrEmpty(_sPath))
                {
                    _sPath = Application.dataPath.Replace("Assets", "");
                }
                return _sPath;
            }
        }

        [MenuItem("Assets/ImportAtlas", false, 3)]
        public static void ImportAtlas()
        {
            if (Selection.activeObject == null)
                return;
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!CheckIsValidFolder(path))
            {
                Debug.LogErrorFormat("select path is invalid! {0}", path);
                return;
            }
            ModifyCurSelectSprites();
            MakeSoyAtlas();
            CheckDuplicatedSprite();
        }

        /// <summary>
        /// 检测是否是有效路径
        /// </summary>
        /// <param name="path"> Selection地址 </param>
        /// <returns></returns>
        private static bool CheckIsValidFolder(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
                return false;

            if (!path.Contains("[UIAtlas]"))
                return false;

            string filePath = FilePathTools.GetFilePathByAssetPath(path, Application.dataPath);
            DirectoryInfo root = new DirectoryInfo(filePath);
            if (!root.Exists)
                return false;
            bool hasFindSorce = false;
            var dArray = root.GetDirectories();
            for (int i = 0; i < dArray.Length; i++)
            {
                var cDir = dArray[i];
                if (cDir.Name == "_source")
                    hasFindSorce = true;
            }

            if (!hasFindSorce)
                return false;
            return true;
        }

        /// <summary>
        /// 批量修改Sprite
        /// </summary>
        public static void ModifyCurSelectSprites()
        {
            UnityEngine.Object sss = Selection.activeObject;
            string selectPath = AssetDatabase.GetAssetPath(sss);

            //click完整路径
            string wholePath = FilePathTools.GetFilePathByAssetPath(selectPath, Application.dataPath);

            _tmpImporterSettings = new TextureImporterSettings();
            LoopSetTexture(wholePath);
            _tmpImporterSettings = null;

            SaveAssets();
        }

        /// <summary>
        /// 设置选择的文件下
        /// </summary>
        /// <param name="dir"> 根目录 </param>
        private static void LoopSetTexture(string dir)
        {
            //获取source文件
            string texturePath = string.Format("{0}/{1}", dir,
                BuildToolsConstDefine.ExportDependsAtlasTextureAssetFolder);
            List<string> fileInfo = GetTexturePath(texturePath);

            int length = fileInfo.Count;
            UIAtlasAttribute att = GetSpriteTag(dir);
            if (att == null)
            {
                Debug.LogErrorFormat("LoopSetTexture called but dir is invalid! {0}", dir);
                return;
            }

            for (int i = 0; i < length; i++)
            {
                //获取资源路径
                string path = fileInfo[i];
                ReImporterTexture(path, att.AtlasName, att.Quality, att.Pivot);
            }

        }

        /// <summary>
        /// 获取不同格式文件路径
        /// </summary>
        /// <param name="dirPath">source文件夹路径</param>
        /// <returns></returns>
        private static List<string> GetTexturePath(string dirPath)
        {
            //jpg
            List<string> jpgList = GetResourcesPath("*.jpg",dirPath);
            //png
            List<string> pngList = GetResourcesPath("*.png",dirPath);

            List<string> list = new List<string>();
            list.AddRange(jpgList);
            list.AddRange(pngList);
            return list;
        }

        /// <summary>
        /// 获取图片路径
        /// </summary>
        /// <param name="fileType"> 图片格式 </param> 
        /// <param name="dirPath"> source文件夹路径 </param> 
        /// <returns></returns>
        private static List<string> GetResourcesPath(string fileType, string dirPath)
        {
            var directoryInfo = new DirectoryInfo(dirPath);
            if (!directoryInfo.Exists)
            {
                Debug.LogErrorFormat("dirPath {0} is not exists!",dirPath);
                return new List<string>();
            }

            var filePath = new List<string>();
            foreach (FileInfo fi in directoryInfo.GetFiles(fileType, SearchOption.TopDirectoryOnly)) //搜索当前目录
            {
                string path = fi.FullName;
                path = path.Remove(0,Path.Length);
                path = path.Replace('\\', '/');
                filePath.Add(path);
            }

            return filePath;
        }

        /// <summary>
        /// 获取图集属性标签
        /// </summary>
        /// <param name="filePath"> 根目录 </param>
        /// <returns></returns>
        private static UIAtlasAttribute GetSpriteTag(string filePath)
        {
            string configPath = string.Format("{0}/{1}", filePath.Replace('\\', '/'), SoyAltasConfig);
            string[] strArray = filePath.Split('/');
            if (strArray.Length < 3)
            {
                Debug.LogErrorFormat("GetSprite Called But filePath is invalid! {0}", filePath);
                return null;
            }

            return ParseSoyAtlasAttr(strArray[strArray.Length - 1], configPath);
        }

        /// <summary>
        /// 设置图集属性
        /// </summary>
        /// <param name="atlasName"> 图集名称 </param>
        /// <param name="configPath"> 根目录 + AtlasConfig.txt </param>
        /// <returns></returns>
        private static UIAtlasAttribute ParseSoyAtlasAttr(string atlasName,string configPath)
        {
            if (string.IsNullOrEmpty(atlasName))
            {
                Debug.LogError("PressSoyAtlasAttr falied atlasName is null or empty");
                return null;
            }

            if (string.IsNullOrEmpty(configPath))
            {
                Debug.LogError("PressSoyAtlasAttr falied configPath is null or empty");
                return null;
            }

            string content = "";
            if (File.Exists(configPath))
                content = File.ReadAllText(configPath);

            string[] array = content.Split('_');
            UIAtlasAttribute data = new UIAtlasAttribute();
            data.AtlasName = atlasName;
            if (array.Length >= 1)
                data.Quality = (ETextureCompressQuality) GetInValueByString(array[0]);
            else
                data.Quality = (ETextureCompressQuality) GetInValueByString("");
            if (array.Length >= 2)
                data.Pivot = (ESpritePivot) GetInValueByString(array[1]);
            else
                data.Pivot = (ESpritePivot) GetInValueByString("");
            return data;

        }

        private static int GetInValueByString(string value)
        {
            int res = 0;
            //如果转换成功则返回true。否则返回false.fasle 为0，true为该string对应的数字
            //int.TryParse(string s, out int i) 的参数： s是要转换的字符串，i 是转换的结果
            int.TryParse(value, out res);
            return res;
        }

        /// <summary>
        /// 设置sprite属性并重新导入
        /// </summary>
        /// <param name="path"> sprite 路径(相对) </param>
        /// <param name="atlasName"> 图集名称 </param>
        /// <param name="quality"> 图片压缩质量 </param>
        /// <param name="pivot"> piovt </param>
        public static void ReImporterTexture(string path, string atlasName, ETextureCompressQuality quality,
            ESpritePivot pivot)
        {
            var impor = AssetImporter.GetAtPath(path) as TextureImporter;
            if (impor == null)
            {
                Debug.LogErrorFormat("ReImporterTexture called but AssetImport.GetAtPath(path) as TextureImporter is null! path is{0}",path);
                return;
            }

            impor.ReadTextureSettings(_tmpImporterSettings);

            //Texture Type
            impor.textureType = TextureImporterType.Sprite;
            impor.spriteImportMode = SpriteImportMode.Single;
            impor.spritePackingTag = atlasName;
            impor.fadeout = false;
            impor.spriteBorder = _tmpImporterSettings.spriteBorder;

            //图片压缩
            impor.textureCompression = TextureImporterCompression.Compressed;
            int maxSize = 1024;
            //mipmap关闭
            bool mipmapEnabled = false;
            impor.mipmapEnabled = mipmapEnabled;
            //过滤
            impor.filterMode = FilterMode.Bilinear;
            impor.spritePixelsPerUnit = 100;
            //置灰？
            impor.fadeout = false;
            impor.wrapMode = TextureWrapMode.Repeat;

            TextureImporterPlatformSettings settingAndroid = new TextureImporterPlatformSettings();
            settingAndroid.overridden = true;
            settingAndroid.name = "Android";
            settingAndroid.maxTextureSize = maxSize;
            settingAndroid.format = TextureImporterFormat.ETC2_RGBA8;
            //设置安卓平台
            impor.SetPlatformTextureSettings(settingAndroid);

            impor.SaveAndReimport();
            return;
        }

        /// <summary>
        /// 保存并刷新
        /// </summary>
        public static void SaveAssets()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }




        /// <summary>
        /// 生成可视化  ScriptableObject
        /// </summary>
        public static void MakeSoyAtlas()
        {
            string rootPath = GetAtlasRootPath();
            //所有图集文件路径
            List<string> list = GetValidAtlasAssetPath(rootPath);

            var mainfestContent = new List<SpriteAtlasRelation>();
            //已经查找过的文件路径
            var hasSearchedTexturePath = new List<string>();
            var totalSprite = new List<Sprite>();
            for (int i = 0; i < list.Count; i++)
            {
                string tmpAssetPath = list[i];
                //_source文件夹路径
                tmpAssetPath = string.Format("{0}/{1}", tmpAssetPath,
                    BuildToolsConstDefine.ExportDependsAtlasTextureAssetFolder);

                if (AssetDatabase.IsValidFolder(tmpAssetPath))
                {
                    //查找明确定义了类型的资源，可以用在资源类型前添加关键字 “t:"的方法。
                    //在tmpAssetPath查找Sprite资源.返回GUID(全局唯一标识符)。
                    string[] findResult = AssetDatabase.FindAssets("t:Sprite", new[] {tmpAssetPath});
                    hasSearchedTexturePath.Clear();
                    for (int j = 0; j < findResult.Length; j++)
                    {
                        //返回指定资源的GUID下的路径path
                        string textureAssetPath = AssetDatabase.GUIDToAssetPath(findResult[j]);
                        if (hasSearchedTexturePath.Contains(textureAssetPath))
                            continue;

                        List<Sprite> tmpList = GetSpriteAssetByPath(textureAssetPath);
                        hasSearchedTexturePath.Add(textureAssetPath);
                        totalSprite.AddRange(tmpList);
                    }

                    string atlasMainName = GetAtlasAssetName(list[i]);

                    //ScriptableObject 名称
                    atlasMainName = string.Format("{0}Atlas", atlasMainName);
                    var resDic = new Dictionary<string, Sprite>();
                    for (int z = 0; z < totalSprite.Count; z++)
                    {
                        //检测重复
                        if (resDic.ContainsKey(totalSprite[z].name))
                        {
                            Debug.LogErrorFormat(string.Format("Sprite name{0} duplicated path is{1} !",
                                totalSprite[z].name, AssetDatabase.GetAssetPath(totalSprite[z])));

                            return;
                        }

                        resDic.Add(totalSprite[z].name,totalSprite[z]);
                        mainfestContent.Add(new SpriteAtlasRelation
                        {
                            AtlasName = atlasMainName,
                            SpriteName = totalSprite[z].name,
                        });
                    }

                    var asset = ScriptableObject.CreateInstance<UGUIAtlas>();
                    asset.CachedSprites = totalSprite;

                    string atlasAssetPath = string.Format("{0}/{1}{2}", list[i], atlasMainName,
                        BuildToolsConstDefine.AssetSuffix);

                    AssetDatabase.CreateAsset(asset, atlasAssetPath);
                    SaveAssets();
                    totalSprite = new List<Sprite>();
                }
            }
        }

        /// <summary>
        /// 获取Atlas根目录
        /// </summary>
        /// <returns></returns>
        public static string GetAtlasRootPath()
        {
            return string.Format("{0}/data/[UIAtlas]",Application.dataPath);
        }

        /// <summary>
        /// 获取[Atlas]下所有有效的文件夹
        /// </summary>
        /// <param name="path"> [Atlas]根目录 </param>
        /// <returns></returns>
        private static List<string> GetValidAtlasAssetPath(string path)
        {
            var res = new List<string>();
            string wholePath = path;
            var info = new DirectoryInfo(wholePath);
            //获取根目录下所有的文件夹信息
            DirectoryInfo[] subFolders = info.GetDirectories();
            string tmpPath;
            for (int i = 0; i < subFolders.Length; i++)
            {
                if (
                    //搜索含有"_source文件夹"的目录
                    subFolders[i].GetDirectories(BuildToolsConstDefine.ExportDependsAtlasTextureAssetFolder,
                        SearchOption.TopDirectoryOnly).Length > 0)
                {
                    //各文件的相对路径
                    tmpPath = subFolders[i].FullName.Substring(Application.dataPath.Length,
                        subFolders[i].FullName.Length - Application.dataPath.Length);

                    tmpPath = string.Format("{0}{1}", BuildToolsConstDefine.AssetRootFolder, tmpPath);
                    tmpPath = tmpPath.Replace('\\', '/');
                    res.Add(tmpPath);
                }
            }
            return res;
        }

        /// <summary>
        /// sprite路径
        /// </summary>
        /// <param name="assetPath"> sprite路径 </param>
        /// <returns></returns>
        private static List<Sprite> GetSpriteAssetByPath(string assetPath)
        {
            var res = new List<Sprite>();
            Object[] assetArray = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            for (int i = 0; i < assetArray.Length; i++)
            {
                if (assetArray[i] is Sprite)
                    res.Add(assetArray[i] as Sprite);
            }

            return res;
        }

        private static string GetAtlasAssetName(string path)
        {
            string[] array = path.Split('/');
            return array[array.Length - 1];
        }



        [MenuItem("AtlasTools/检查Sprite是否有重复")]
        public static void CheckDuplicatedSprite()
        {
            bool isOk = true;
            Dictionary<string, List<Sprite>> spriteNameCount = new Dictionary<string, List<Sprite>>();
            string sRootPath = Application.dataPath + "\\data\\[UIAtlas]";
            //资源文件目录
            sRootPath = FilePathTools.GetAssetPathByFilePath(sRootPath, Application.dataPath);
            //获取sRootPath下的所有Sprite文件
            var asPathArray = AssetDatabase.FindAssets("t:Sprite", new string[] {sRootPath});
            for (int i = 0; i < asPathArray.Length; i++)
            {
                //资源路径
                string assetPath = AssetDatabase.GUIDToAssetPath(asPathArray[i]);
                //获取资源
                Sprite x = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);

                //如果不存在那么就初始化，并添加
                //如果存在直接再次添加.
                List<Sprite> sList = null;
                if (!spriteNameCount.TryGetValue(x.name, out sList))
                {
                    sList = new List<Sprite>();
                    spriteNameCount[x.name] = sList;
                }
                sList.Add(x);
            }

            var enu = spriteNameCount.GetEnumerator();
            while (enu.MoveNext())
            {
                var cur = enu.Current.Value;
                if (cur.Count > 1)
                {
                    isOk = false;
                    for (int i = 0; i < cur.Count; i++)
                    {
                        Debug.LogError("Sprite duplicated target is:" + cur[i].name);
                    }
                }
            }

            if (isOk)
                Debug.LogError("图集重复项,检查完成");
            else
                Debug.LogError("有问题,检查上面错误日志");
        }

    }


}


