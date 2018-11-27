using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

#region 热更部分解释
/*  热更部分
 *  
 *   实测可完成PC和Android热更.
1.资源打包入口: PackageBuild() ---> 选择不同平台 打包不同平台资源资源  资源默认打包.Lua打包需设置AppConst中LuaBunldeMode = true；
                                  打包资源切记刷新 AssetDatabase.Refresh();
2.项目入口： Main（）
3.资源下载更新：HotManager() ---> 检查是否存在已经资源文件.
                         若不存在（第一次安装游戏）,先解压项目的文件资源（游戏包资源目录 只读）至本地数据目录（可读可写）.
                         若已经存在，那么通过网络资源地址中的网络资源，与本地文件比对。检测是否需要更新（是否能进行更新主要是通过比对文件MD5）.
4.资源下载：ThreadManager() ---> 通过检测队列中是否有元素来下载资源.（lock还有点问题）
5.Lua虚拟机：LuaManager()  ---> (1) 设置lua的加载路径（可直接加载也可打包加载）.
                               (2) 启动lua虚拟机.使在Lua中也可调用C#的方法等
6.资源加载：ResourceManager() ---> 加载AB中的资源
7.日志输出：LTDebugOutput() ---> 控制Debug的打印至屏幕。并将Debug日志跟随游戏保存.
*/

#endregion
namespace MyFramework
{
    public class HotManager : BaseClass
    {
        #region  热更管理类
        string updateWord = "";
        float updatePercent = 0;
        //缓存已下载的文件
        public List<string> downLoadFiles;
        private bool isComplete = false;

        //初始化
        public void Init()
        {
            downLoadFiles = new List<string>();
        }

        void CheckExtractResource()
        {
            bool isExists = Directory.Exists(Util.DataPath) && Directory.Exists(Util.DataPath + "lua/") && File.Exists(Util.DataPath + "files.txt");
            
            
            if (isExists || AppConst.DebugMode)
            {
                StartCoroutine(OnUpdateResource());
                return;
            }
            //解压资源
            StartCoroutine(OnExtractResource());
        }

        //更新资源
        public void UpdateResource()
        {
            StartCoroutine(OnUpdateResource());
        }

        //解压资源
        public void ExtractResource()
        {
            StartCoroutine(OnExtractResource());
        }

        IEnumerator OnUpdateResource()
        {
            downLoadFiles.Clear();
            if(!AppConst.UpdateMode)
            {
                Initialize(OnUpdateOver);
                yield break;
            }

            //下载url
            string url = AppConst.WebUrl;
            //Util.LogErr("url:" + url);
  
            //if (Application.platform == RuntimePlatform.IPhonePlayer)
            //    url += "ios/";
            //else if(Application.platform == RuntimePlatform.Android)
            //    url += "android/";

            string random = DateTime.Now.ToString("yyyymmddhhmmss");

            //路径
            string dataUrl = url + "files.txt?v=" + random;
            //Util.LogErr("dataUrl:" + dataUrl);
            WWW www = new WWW(dataUrl);

            updateWord = "版本检测中：           " + (www.progress * 100).ToString() + "%";
            updatePercent = www.progress;

            SDDebug.Log(updateWord);

            yield return www;

            //下载错误情况
            if (www.error != null)
            {
                OnUpdateFailed(www.error);
                yield break;
            }

            //热更模式 d:/hotfix/
            string dataPath = Util.DataPath;
            if (!Directory.Exists(dataPath))
                Directory.CreateDirectory(dataPath);

            //将下载的所有bytes文件存放至files.txt中
            File.WriteAllBytes(dataPath + "files.txt" , www.bytes);
            string filesText = www.text;
            string[] files = filesText.Split('\n');


            bool isCheckVer = false;
            string message = string.Empty;
            //版本文件的url
            string strVerUrl = "";
            //版本文件的本地路径
            string strLocalVer = "";
            //版本文件更新状态
            bool isCanUpdateVer = false;


            for(int i = 0;i < files.Length;i++)
            {
                if (string.IsNullOrEmpty(files[i]))
                    continue;

                //文件与MD5分隔  keyValue[0]:文件名 keyValue[1]:文件MD5
                string[] keyValue = files[i].Split('|');
                string f = keyValue[0];
                string localFile = (dataPath + f).Trim();
                string path = Path.GetDirectoryName(localFile);
                //创建文件目录
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //服务器文件路径
                string fileUrl = url + keyValue[0] + "?v=" + random;
                //是否能下载取决于 本地是否已经存在这个文件  本地不存在该文件.那么表示可以下载
                bool canUpdate = !File.Exists(localFile);
                bool isVerFile = false;

                //是否存在版本txt
                isVerFile = f.Equals("version.txt");

                //本地存在该文件
                if(!canUpdate)
                {
                    //获取文件MD5
                    string remoteMD5 = keyValue[1].Trim();
                    
                    //计算该文件MD5
                    string localMD5 = Util.MD5File(localFile);
                    
                    //检验MD5是否匹配  匹配即不用更新.反之亦然
                    canUpdate = !remoteMD5.Equals(localMD5);

                    //若已检测到了两个MD5不同,那么删除本地文件.开始下载
                    if (canUpdate)
                        File.Delete(localFile);

                    //检测是否有版本文件
                    if(!isCheckVer && isVerFile)
                    {
                        //存在版本文件
                        isCheckVer = isVerFile;

                        //如果版本文件存在,并且版本文件不需要更新.那么就提示更新检测完毕.不需要更新.跳出循环
                        if(isCheckVer && !canUpdate)
                        {
                            updateWord = "更新检测完毕，进入游戏！";
                            SDDebug.Log(updateWord);
                            updateWord = message;
                            updatePercent = 1;
                            //DownPanel.SetProgressAndFile(updatePercent, updateWord);

                            OnUpdateMessageComplete(message);
                            //开始初始化
                            Initialize(OnUpdateOver);
                            yield break;
                        }
                    }
                }


                //版本文件有变化
                if(isVerFile)
                {
                    //版本文件的url
                    strVerUrl = fileUrl;
                    //版本文件的本地路径
                    strLocalVer = localFile;
                    //版本文件更新状态
                    isCanUpdateVer = canUpdate;
                }

                //如果是可更新的文件（不包含版本文件）
                if(canUpdate && !isVerFile)
                {
                    //updateWord = "downloading>>>" + fileUrl;
                    
                    OnUpdateMessageDownLoad(message);
                    //开始下载文件
                    BeginDownload(fileUrl, localFile);
                    while (!(IsDownOk(localFile)))
                    {
                        updatePercent = (float) i/files.Length;
                        updateWord = "开始下载资源  " + i.ToString() + "/" + files.Length.ToString() +
                                     "                                " + Math.Ceiling(updatePercent*100) + "%";
                        //DownPanel.SetProgressAndFile(updatePercent, updateWord);
                        SDDebug.Log(updateWord);
                        yield return new WaitForEndOfFrame();
                    }
                }
            }

            //版本文件可更新.开始下载版本文件
            if (isCanUpdateVer)
            {
                BeginDownload(strVerUrl, strLocalVer);
                while (!(IsDownOk(strLocalVer)))
                    yield return new WaitForEndOfFrame();
            }

            updatePercent = 100f;
            updateWord = "更新游戏完成         " + files.Length.ToString() + "/" + files.Length.ToString() +
                         "                                100%";
            SDDebug.Log(updateWord);
            //DownPanel.SetProgressAndFile(updatePercent, updateWord);
            yield return new WaitForEndOfFrame();
            //Util.LogErr("更新完成!!!!!");

            Initialize(OnUpdateOver);
        }

        IEnumerator OnExtractResource()
        {
            string dataPath = Util.DataPath;
            string resPath = Util.AppContentPath();

            if (Directory.Exists(dataPath))
                Directory.Delete(dataPath, true);

            Directory.CreateDirectory(dataPath);

            string infile = resPath + "files.txt";
            string outfile = dataPath + "files.txt";

            if (File.Exists(outfile))
                File.Delete(outfile);

            updateWord = "开始解压资源...";
            SDDebug.Log(updateWord);
            //Util.Log(message);

            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW(infile);
                updateWord = "版本检测中   " + (www.progress*100).ToString() + "%";
                SDDebug.Log(updateWord);
                updatePercent = www.progress;
                //DownPanel.SetProgressAndFile(updatePercent, updateWord);
                yield return www;

                if (www.isDone)
                {
                    //Util.LogErr("下载文件：" + www.bytes);
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return 0;
            }
            else
            {
                File.Copy(infile, outfile, true);
            }

            yield return new WaitForEndOfFrame();

            string[] files = File.ReadAllLines(outfile);
            int index = 0;
            foreach (var file in files)
            {
                index++;
                string[] fs = file.Split('|');
                infile = resPath + fs[0];
                outfile = dataPath + fs[0];

                updateWord = "正在解包文件:>" + fs[0];
                SDDebug.Log(updateWord);
                //Util.Log(message);

                string dir = Path.GetDirectoryName(outfile);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (Application.platform == RuntimePlatform.Android)
                {
                    WWW www = new WWW(infile);
                    yield return www;

                    if (www.isDone)
                        File.WriteAllBytes(outfile, www.bytes);

                    yield return 0;
                }
                else
                {
                    if (File.Exists(outfile))
                        File.Delete(outfile);
                    File.Copy(infile, outfile, true);
                }

                updatePercent = (float) index/files.Length;
                updateWord = "解压文件中          " + Math.Ceiling((updatePercent*100)) + "%";
                SDDebug.Log(updateWord);
                //DownPanel.SetProgressAndFile(updatePercent, updateWord);
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(1f);

            StartCoroutine(OnUpdateResource());
        }

        void OnUpdateFailed(string file)
        {
            updateWord = "更新失败：=======================<" + file + ">";
            SDDebug.Log(updateWord);
            //Debug.LogError(message);
            //Util.LogErr(message);
            return;
        }

        void OnUpdateMessageComplete(string message)
        {
            
            Util.LogErr(message);
            return;
        }
            
        void OnUpdateMessageDownLoad(string file)
        {
            updateWord = "更新下载：=======================<" + file + ">";
            SDDebug.Log(updateWord);
            //Debug.LogError(message);
            //Util.LogErr(message);
            return;
        }

        public void ShowDownOrExtractLog(string log)
        {
            SDDebug.Log(log);
        }

        /// <summary>
        /// 开始下载 注意：此处下载完成的文件,存放的地址位于file
        /// </summary>
        /// <param name="url"></param> 服务器文件路径(下载路径)
        /// <param name="file"></param> 本地文件路径
        void BeginDownload(string url,string file)
        {
            object[] param = new object[2] { url, file };
            ThreadEvent ev = new ThreadEvent();
            ev.key = NotiConst.UPDATE_DOWNLOAD;
            //添加实现了ICollection接口的一个集合的所有元素到指定集合的末尾
            ev.evParams.AddRange(param);
            //添加线程事件.通过线程开始执行下载
            ThreadManager_.AddEvent(ev, OnThreadCompleted);
        }

        /// <summary>
        /// 下载成功.缓存下载的资源名称.继续开启下一步下载任务
        /// </summary>
        /// <param name="data"></param>
        void OnThreadCompleted(NotiData data)
        {
            switch (data.evName)
            {
                case NotiConst.UPDATE_EXTRACT:
                    break;
                case NotiConst.UPDATE_DOWNLOAD:
                    if (!downLoadFiles.Contains(data.evParam.ToString()))
                        downLoadFiles.Add(data.evParam.ToString());
                    break;
            }
        }



        bool IsDownOk(string file)
        {
            return downLoadFiles.Contains(file);
        }


        public bool IsComplete()
        {
            return isComplete;
        }

        
        void Initialize(Action func)
        {
            //ResourceManager_.Initialize();
            if (func != null)
                func();
        }

        void OnUpdateOver()
        {
            isComplete = true;


            //string path = "game/DownLoadPanel";
            ////ResourceManager_.CacheBundle(path);
            //LoadInitPrefab(path);
            //LuaManager_.InitStart();
            //LuaManager_.DoFile("Main.lua"); //加载文件，编译文件，并且返回一个函数，不运行。 

            //Util.CallMethod("Main", "start");
            //Util.CallMethod("Main", "SetValue");
        }

        //void LoadInitPrefab(string path)
        //{
        //    GameObject obj =  ResourceManager_.CreatGamePrefab(path);
        //    var parent =  GameObject.Find("UI_Canvas").gameObject;
        //    obj.transform.parent = parent.transform;
        //    obj.AddComponent<DownPanel>();
        //    obj.transform.localPosition = Vector3.zero;
        //    //RectTransform
        //}
        #endregion
    }
}
