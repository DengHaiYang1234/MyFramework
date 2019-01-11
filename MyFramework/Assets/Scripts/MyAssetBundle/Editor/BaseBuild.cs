using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using MyFramework;

namespace MyAssetBundleEditor
{
    public abstract class BaseBuild
    {
        protected static List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
        protected static List<string> packedAssets = new List<string>(); //记录打包的资源
        private static List<BaseBuild> patterns = new List<BaseBuild>();
        private static Dictionary<string, List<string>> allDependencies = new Dictionary<string, List<string>>(); //所有文件依赖信息

        public string searchPath;
        public string searchPattern;
        public SearchOption option;
        public string bundleName;
        public string assetFolderName;

        static BaseBuild()
        {

        }

        protected BaseBuild(string path, string pattern, SearchOption option)
        {
            searchPath = path;
            searchPattern = pattern;
            this.option = option;
        }

        protected BaseBuild()
        {

        }

        public abstract void Build();

        public abstract string GetAssetBundleName(string assetPath);

        public static List<AssetBundleBuild> GetBuilds()
        {
            packedAssets.Clear();
            builds.Clear();
            //allDependencies.Clear();
            builds.Add(BuildManifest());//添加配置文件

            string packagePatternPath = BuildDefaultPath.GetBuildPattrenAssetPath();

            if (!File.Exists(packagePatternPath))//获取打包方式
                new BuildPackPattern();

            if (!LoadEachPatterns())
                return null;

            foreach (var item in patterns)
            {
                if (item.searchPath == "")
                {
                    Debug.LogErrorFormat("assetName:{0}   searchPath is null ! Check !!!", item.assetFolderName);
                    continue;
                }

                if (item.searchPattern == "")
                {
                    Debug.LogErrorFormat("assetName:{0}   searchPattern is null ! Check !!!", item.assetFolderName);
                    continue;
                }

                CollectDependencies(ResFileInfo.GetFilesWithoutDirectores(item.searchPath, item.searchPattern,
                    item.option)); //收集资源依赖
            }


            foreach (var item in patterns)
            {
                item.Build(); //获取打包信息
            }

            BuildAtlas();//编辑图集资源

            UnityEditor.EditorUtility.ClearProgressBar();

            return builds;
        }

        private static bool LoadEachPatterns()
        {
            bool isError = true;
            //在程序编译阶段，编译器会自动将using语句生成为try-finally语句，并在finally块中调用对象的Dispose方法，来清理资源。所以，using语句等效于try-finally语句
            patterns.Clear();
            var pkgPattern = AssetDatabase.LoadAssetAtPath<PackagePattern>(BuildDefaultPath.GetBuildPattrenAssetPath());
            if (pkgPattern != null)
            {
                pkgPattern.MappingPackageData();
                var data = pkgPattern.GetCacheAssetDataInfos();
                var e = data.GetEnumerator();
                while (e.MoveNext())
                {
                    if (e.Current.Value.assetFolderName != null)
                    {
                        if (e.Current.Value.BuildType == BuildType.None)
                        {
                            MyDebug.LogErrorFormat("LoadEachPatterns is Called,But  BuildType == None! 【assetName】:{0}", e.Current.Value.assetFolderName);
                            isError = false;
                            continue;
                        }

                        var type =
                            typeof(BaseBuild).Assembly.GetType("MyAssetBundleEditor." + Enum.GetName(typeof(BuildType), e.Current.Value.BuildType)); //反射得到对应对象

                        if (type != null)
                        {
                            //实例化
                            var pattern = Activator.CreateInstance(type) as BaseBuild;
                            pattern.searchPath = e.Current.Value.searchPath;
                            pattern.searchPattern = e.Current.Value.searchPattern;
                            pattern.bundleName = e.Current.Value.bundleName;
                            pattern.option = e.Current.Value.searchOption;
                            pattern.assetFolderName = e.Current.Value.assetFolderName;
                            patterns.Add(pattern);
                        }
                        else
                        {
                            Debug.LogError(string.Format("BuildType is Have.But MyAssetBundleEditor.{0} is null!!!!", e.Current.Value.BuildType));
                        }
                    }
                    else
                    {
                        Debug.LogErrorFormat("LoadEachPatterns is Called.But assetName == null  {0}", e.Current.Key);
                    }
                }
            }

            return isError;
        }
        /// <summary>
        /// 搜集依赖
        /// </summary>
        /// <param name="files"> 资源 </param>
        protected static void CollectDependencies(List<string> files)
        {
            for (int i = 0; i < files.Count; i++)
            {
                var item = files[i];
                var dependencies = AssetDatabase.GetDependencies(item);

                if (
                    UnityEditor.EditorUtility.DisplayCancelableProgressBar(
                        string.Format("CollectDependencies....[{0}/{1}]", i, files.Count), item, i * 1f / files.Count))
                    break;

                foreach (var assetPath in dependencies)
                {
                    if (!allDependencies.ContainsKey(assetPath))
                        allDependencies[assetPath] = new List<string>();

                    if (!allDependencies[assetPath].Contains(item))  //收集资源依赖情况
                        allDependencies[assetPath].Add(item);
                }
            }
            UnityEditor.EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 获取packedAssets之外的资源
        /// </summary>
        /// <param name="searchPath"></param>
        /// <param name="searchPattern"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        protected static List<string> GetFilesWithoutPacked(string searchPath, string searchPattern, SearchOption option)
        {
            var files = ResFileInfo.GetFilesWithoutDirectores(searchPath, searchPattern, option);
            var filesCount = files.Count;
            var removeAll = files.RemoveAll((string file) =>
            {
                //TODO
                return packedAssets.Contains(file);
            });

            Debug.LogError(string.Format("RemoveAll {0} size: {1}", removeAll, filesCount));

            return files;
        }

        /// <summary>
        /// 对应路径下的依赖文件
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        protected static List<string> GetDependencies(string pathName)
        {
            var assets = AssetDatabase.GetDependencies(pathName);
            List<string> assetNames = new List<string>();
            foreach (var assetPath in assets)
            {
                //1.剔除prefab  2.去除自身  3.去除已打包资源  4.去除.cs文件
                if (assetPath.Contains(".prefab") || assetPath.Equals(pathName) || packedAssets.Contains(assetPath) ||
                    assetPath.EndsWith(".cs", StringComparison.CurrentCulture))
                {
                    continue;
                }

                if (allDependencies[assetPath].Count == 1) //排除多依赖文件
                    assetNames.Add(assetPath);
            }

            return assetNames;
        }

        #region Sprite 与 Texture
        /*
         DXT格式是Nvidia Tegra提供的，
         ETC是安卓原生支持的，
         OPNEGL2.0都支持。
         ETC2只有OPENGL3.0支持，
         PVRTC是Imagination PowerVR提供的，
         ATC是Qualcomm Snapdragon提供的。
         一般来说，IOS只支持PVRTC的压缩格式。一旦相应的贴图格式不兼容的时候，U3D会自动将其转换成RGB(A)格式。
         最好的兼容是针对GPU进行打包，例如针对小米的都用ATC格式，但一般开发做不到太细化的选择。
         所有设备对RGB 16BITS/ARGB 16BITS/RGB A16BITS/RGB 24BITS/ARGB 32BITS等支持都很好，只是这些格式算是非压缩格式，对内存消耗和渲染消耗非常不友好。 



        一、Sprite，精灵
         1.2D--UI
          1.Sprite 用在 Image 组件上.,可以用来制作动画，可以设置Simple模式，作为一般UI,
            设置为Sliced模式，即九宫格模式，在图集中设置图图片边界后，使图片的拉伸只拉伸中间部分，不拉伸边界
            设置为Tiled 模式，实现过个图片重复平铺的效果，
            设置为Filed模式，实现图片的部分到整体的播放，可以用来做技能冷却，或者游戏的进度条（不是压缩的拉伸）
            材质通常不需要选择，很少用到，可以用来实现一些特殊效果，如凹凸感觉
          2.Sprite 有图集的概念，可以选择整图导入，UNITY中使用SpriteEditor切割，也可以选择导入后设置图片的packageTag系统自动打包图集,
            图片小的，重复性比较高的图片最好打成图集。
           注意：
           1，一个图集内的图片用UISprite，那么它就是一个DrawCall。但是如果你做了一个图集是1024X1024的。
            此时你的界面上只用了图集中的一张很小的图，那么很抱歉1024X1024这张大图都需要载入你的内存里面，1024就是4M的内存，
            如果你做了10个1024的图集，你的界面上刚好都只用了每个图集里面的一张小图，那么再次抱歉你的内存直接飙40M
           2.带透明通道和不带透明通道的，CreatMipMap和不Create 的，不能制作成同一图集  
            2.3D---场景
            1.单个Sprite 直接拖入场景中，系统自动添加SpriteRanderder 组件，作为3D物体直接使用，
            2. 多个Sprite直接拖入场景，可以直接制作帧动画，在2D中同样也可以。

       二、Texture 纹理
            2D.---UI
            1.Texture用在Raw Image组件上，可以用来制作动画
            2.tuxture没有图集的概念,这样内存里只会占用你这一张图的大小，内存虽然小了但是DrawCall就上去了。因为每一张UITexture就是一次DrawCall。原画，或者背景图建议直接使用UITexture。
            3.可以通过UV 调节图片显示的偏移，和重复（可以用来制作多格子血条）
            3D--场景
            1.无论单个，多个，不可以直接拖入3D场景中！！,2D也不行
            2.用于3D模型贴图,（Shader代码把贴图和纹理坐标映射）,再由GPU把模型渲染出来
            MeshFiiter组件中模型网格，存储的纹理坐标信息（Unity自己创建的Cube会自动添加纹理坐标所以创建后就能贴上纹理，3D建模时如果忽略 没有给模型生成纹理坐标，会导致模型贴上贴图没有效果）
            MesherRenderder 物体渲染组件
         */
        #endregion


        static void BuildAtlas()
        {
            foreach (var item in builds)
            {
                var assetsPath = item.assetNames;
                foreach (var assetPath in assetsPath)
                {
                    if (assetPath.IndexOf(BuildDefaultPath.assetsAtlasFloder) != -1)
                    {
                        /*
                          Texture Type：贴图类型
　　                                  Alpha from Grayscale：从灰度图中是否产生Alpha通道
　　                                  Wrap Mode：贴图与贴图之间的拼接模式
　　                                  Filter Mode：过滤模式
　　                                  AnIso Level：异向性过滤等级

                           Texture：普通贴图
　　                              Normal map：法线贴图
　　                              Editor GUI and Legacy GUI：UI贴图
　　                              Sprite(2D and UI)：精灵
　　                              Cursor：鼠标指针
　　                              Reflection：反射贴图
　　                              Cookie：遮罩贴图
　　                              Lightmap：烘焙贴图
　　                              Advanced：高级(可自定义一些贴图属性)
　　                              Texture 修改Tilling参数
                        */
                        var importer = AssetImporter.GetAtPath(assetPath);
                        var ti = importer as TextureImporter;
                        if (ti.textureType != TextureImporterType.Sprite)
                        {
                            ti.textureType = TextureImporterType.Sprite;
                        }

                        var tex = AssetDatabase.LoadAssetAtPath<Texture>(assetPath);
                        if (tex.texelSize.x >= 1024 || tex.texelSize.y >= 1024)
                        {
                            continue;
                        }

                        var tag = item.assetBundleName.Replace("/", "_");
                        if (!tag.Equals(ti.spritePackingTag))
                        {
                            var settings = ti.GetPlatformTextureSettings(ResUtility.GetPlatformPath);
                            /*
                                Compressed 压缩格式，如果纹理没有透明通道，一般使用该项，优化内存量，如果有透明通道，显示原图片有可能出现问题。4位
                                16bit 低质量真彩格式。16位
                                TrueColor 真彩模式。质量最高，是压缩格式的8倍，但也更消耗内存，32位
                                Crunched 这种类型将会根据显卡的GPU来选择合适的压缩格式进行压缩然后会选用一种CPU上就能处理的压缩格式再压缩一遍。
                                如果在制作供人下载的资源包的时候这种类型非常的合适。这个类型的压缩需要很长时间，但在运行时解压是非常快的。
                            */
                            settings.format = ti.GetAutomaticFormat(ResUtility.GetPlatformPath); //根据平台设置图片压缩方式
                            settings.overridden = true;
                            ti.spritePackingTag = tag;
                            ti.spriteImportMode = SpriteImportMode.Single;
                            ti.fadeout = false;
                            ti.textureCompression = TextureImporterCompression.Compressed;
                            /*
                                Mipmap针对的纹理贴图资源,使用Mipmap后，贴图会根据摄像机距离的远近，选择使用不同精度的贴图。
                                缺点：会占用内存，因为mipmap会根据摄像机远近不同而生成对应的八个贴图，所以必然占内存！
                                优点：会优化显存带宽，因为可以根据实际情况，会选择适合的贴图来渲染，距离摄像机越远，显示的贴图像素越低，反之，像素越高！
                             */
                            ti.mipmapEnabled = false;
                            /*
                                图片在发生拉伸变化时使用那种滤波模式，point ，Biliner,Trilinear,得到依次滤波效果提升的图片，
                                point 使用最邻近滤波，采样像素通常只有一个，图像放大缩小后会有像素风格，在制作棋盘时，不希望有模糊效果选择这这种模式更好。
                                Biliner使用线性滤波，找相邻四个像素差值，放大缩小后会有模糊效果，会被模糊，
                                Trilinear,几乎和Biliner是一样的，只是Triliner在多级纹理渐变中进行了混合，如果一个纹理没有使用该技术（Creat MitMap）几乎是一样效果。
                            */
                            ti.filterMode = FilterMode.Bilinear;
                            ti.spritePixelsPerUnit = 100;
                            ti.fadeout = false;
                            /*
                                Warp Mode 设置可以纹理在渲染超过纹理坐标时，Climp只选择重复纹理边缘像素，还是repeat模式重复整个纹理的模式
                            */
                            ti.wrapMode = TextureWrapMode.Repeat;
                            ti.SaveAndReimport();
                            /*
                                 MaxSize 该纹理的最大尺寸，如原图尺寸为1024*568，该项设置成4096，unity也只会使用它的原尺寸大小，改值的大小大于等于图片原尺寸，如果小于该纹理质量会有损失
                            */
                        }
                    }
                }
            }
        }

        private static AssetBundleBuild BuildManifest()
        {
            string manifestAssetsName = BuildDefaultPath.GetManifestAssetPath();
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = manifestAssetsName.Substring(0, manifestAssetsName.LastIndexOf('.')).ToLower();
            build.assetNames = new string[] { manifestAssetsName };
            return build;
        }
    }

}


