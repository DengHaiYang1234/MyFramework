using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Res;

namespace MyFramework
{
    public class HotManager : MonoBehaviour
    {
        #region  热更管理类
        string updateWord = "";

        float updatePercent = 0;

        //缓存已下载的文件 便于比对文件
        public List<string> downLoadFiles;

        private bool isComplete = false;

        public bool IsInitLocalFile
        {
            get { return isInitLocalFile; }
        }

        private bool isInitLocalFile = false;

        //key: file   value:MD5
        private Dictionary<string, string> localFileInfosDic = new Dictionary<string, string>();

        private Dictionary<string, string> remoteDownloadFileInfosDic = new Dictionary<string, string>();

        //初始化
        public void Init()
        {
            MyDebug.LogError("DataPath:" + Application.dataPath);
            downLoadFiles = new List<string>();
        }

        //更新资源
        public void UpdateResource()
        {
            StartCoroutine(OnUpdateResource());
        }

        /// <summary>
        /// 初始化本地文件信息
        /// </summary>
        public bool InitLoaclFilesInfo(bool isText, string filesInfos = null)
        {
            localFileInfosDic.Clear();
            if (!Directory.Exists(RuntimeResPath.GetLocalDataPath))
                Directory.CreateDirectory(RuntimeResPath.GetLocalDataPath);

            string fileTextPath = string.Empty;
            if (!isText)
            {
                if (filesInfos == null)
                {
                    fileTextPath = RuntimeResPath.GetLocalDataPath + "files.txt";
                }
                else
                    fileTextPath = filesInfos;
            }


            if (isText || File.Exists(fileTextPath))
            {
                string filesText = string.Empty;
                if (isText)
                    filesText = filesInfos;
                else
                    filesText = ResUtility.ReadFile(fileTextPath);
                string[] filesInfo = filesText.Split('\n');

                if (filesInfo.Length <= 0)
                {
                    MyDebug.LogErrorFormat(" files.txt 文件为空.请注意!:{0}", fileTextPath);
                    return false;
                }
                else
                {
                    for (int i = 0; i < filesInfo.Length; i++)
                    {
                        if (string.IsNullOrEmpty(filesInfo[i]))
                            continue;
                        string[] infos = filesInfo[i].Split('|');
                        string file = infos[0];
                        string filemd5 = infos[1];
                        if (!localFileInfosDic.ContainsKey(file))
                            localFileInfosDic.Add(file, filemd5);
                        else
                            MyDebug.LogErrorFormat("文件重复！！！！！请检查  file:{0}", file);

                        string localFile = (RuntimeResPath.GetLocalDataPath + file).Trim();
                        string dir = Path.GetDirectoryName(localFile);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                    }
                    isInitLocalFile = true;
                    return true;
                }
            }
            else
            {
                MyDebug.LogErrorFormat(" files.txt 文件不存在.请检查!:{0}", fileTextPath);
                return false;
            }
        }

        /// <summary>
        /// 从包体中获取本地信息文件
        /// </summary>
        /// <returns></returns>
        public void InitLocalFilesInfoByResource()
        {
            StartCoroutine(GetLocalFilesInfos());
        }

        //"jar:file://" + 

        IEnumerator GetLocalFilesInfos()
        {
            string resourceDataPath = string.Empty;
            if (Application.platform == RuntimePlatform.Android)
            {
                resourceDataPath = "jar:file://" + ResUtility.GetPlatformResourcesPath;
                WWW www = new WWW(resourceDataPath + "files.txt");
                yield return www;
                if (www.isDone)
                {
                    if (www.error == null)
                    {
                        InitLoaclFilesInfo(true, www.text);
                    }
                    else
                    {
                        MyDebug.LogErrorFormat("GetLocalFilesInfos Is Called.But WWW.text Is Null. 【resourceDataPath】:{0}", resourceDataPath);
                        yield break;
                    }
                }
            }
            else
            {
                InitLoaclFilesInfo(false, resourceDataPath + "files.txt");
                yield break;
            }
        }

        IEnumerator OnUpdateResource()
        {
            downLoadFiles.Clear();

            if (!FrameworkDefaultSetting.UpdateMode)
            {
                OnUpdateMessageComplete();
                yield break;
            }

            string url = string.Format("{0}{1}/", FrameworkDefaultSetting.WebUrl, RuntimeResPath.GetAssetBundleDirectory);

            string random = DateTime.Now.ToString("yyyymmddhhmmss");
            //远端文件列表地址
            string remoteFileTextUrl = url + "files.txt?v=" + random;

            MyDebug.LogError("remoteFileTextUrl" + remoteFileTextUrl);

            WWW www = new WWW(remoteFileTextUrl);

            yield return www;

            //下载失败处理
            if (www.error != null)
            {
                OnUpdateFailed(www.error);
                yield break;
            }

            //远端文件
            string remoteFilesText = www.text;
            //文件信息
            string[] remoteFiles = remoteFilesText.Split('\n');
            //是否已经检测过版本文件
            bool isCheckVer = false;
            //远端文件Url
            string remoteVerUrl = string.Empty;
            //本地文件
            string localVerFile = string.Empty;
            //是否更新版本文件
            bool isCanUpdateVerFile = false;

            for (int i = 0; i < remoteFiles.Length; i++)
            {
                if (string.IsNullOrEmpty(remoteFiles[i]))
                    continue;

                string[] remoteFileInfos = remoteFiles[i].Split('|');
                string remoteFile = remoteFileInfos[0];
                string remoteFileMD5 = remoteFileInfos[1].Trim(); //Trim() 删除空白字符
                string localFile = (RuntimeResPath.GetLocalDataPath + remoteFile).Trim();
                string dir = Path.GetDirectoryName(localFile);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                //---------------------------------开始下载检测-----------------------------------
                //远端文件下载路径
                string remoteFileUrl = url + remoteFile + "?v=" + random;
                //是否可以下载
                bool canUpdate = false;
                //是否是版本文件
                bool isVerFile = false;

                //当前文件是否为版本文件
                isVerFile = remoteFile.Equals("version.txt");
                //当前文件MD5
                string currentFlieMD5 = string.Empty;

                //将远端文件与本地文件相比较
                if (localFileInfosDic.TryGetValue(remoteFile, out currentFlieMD5))
                {
                    if (!currentFlieMD5.Equals(remoteFileMD5)) //若本地文件与远端文件的MD5码不相同。那么可更新
                        canUpdate = true;
                }
                else
                    canUpdate = true; //本地文件不存在当前的这个远端文件.那么可更新

                if (canUpdate)
                    File.Delete(localFile);

                //-------------------------------------第一次检测到版本文件
                if (!isCheckVer && isVerFile)
                {
                    isCheckVer = true;//标记已检测过版本文件
                    remoteVerUrl = remoteFileUrl; //获取版本文件的下载地址
                    localVerFile = localFile; //本地版本文件替换
                    isCanUpdateVerFile = canUpdate; //检测当前版本文件是否可以更新

                    //已检测过版本文件并且当前版本文件不需要更新。那么版本是相同的，不需要更新
                    if (isCheckVer && !canUpdate)
                    {
                        OnUpdateMessageComplete();//更新检测完毕
                        yield break;
                    }
                }

                //-------------------------------------当前文件不是版本文件.并且可以更新
                if (canUpdate && !isVerFile)
                {
                    OnStartDownLoad(remoteFileUrl);
                    BeginDownload(remoteFileUrl, localFile); //开始下载
                    while (!(IsDownOk(localFile)))
                    {
                        OnUpdateMessageDownLoad(i, remoteFiles.Length);
                        yield return new WaitForEndOfFrame();
                    }
                }
            }

            //最后更新版本文件
            if (isCanUpdateVerFile)
            {
                BeginDownload(remoteVerUrl, localVerFile);
                while (!(IsDownOk(localVerFile)))
                {
                    OnUpdateMessageDownLoad(remoteFiles.Length - 1, remoteFiles.Length);
                    yield return new WaitForEndOfFrame();
                }
                //将远端信息更新至本地
                File.WriteAllBytes(RuntimeResPath.GetLocalDataPath + "files.txt", www.bytes);
            }

            OnUpdateMessageComplete();
            yield break;
        }

        void OnUpdateFailed(string file)
        {
            updateWord = "更新失败：=======================<" + file + ">";
            MyDebug.LogError(updateWord);
        }

        void OnUpdateMessageComplete()
        {
            MyDebug.LogError("资源更新完毕!");
            isComplete = true;
        }

        void OnStartDownLoad(string file)
        {
            updateWord = "正在下载：=======================<" + file + ">";
            MyDebug.Log(updateWord);
        }

        void OnUpdateMessageDownLoad(int curr, int count)
        {
            updateWord = "下载进度：=======================<已下载文件个数:" + curr + "/" + count + "> 下载文件进度：" + Math.Ceiling(((float)curr / count) * 100) + "%";
            MyDebug.Log(updateWord);

        }

        public void ShowDownOrExtractLog(string log)
        {
            MyDebug.Log(log);
        }

        /// <summary>
        /// 开始下载 注意：此处下载完成的文件,存放的地址位于file
        /// </summary>
        /// <param name="url"></param> 服务器文件路径(下载路径)
        /// <param name="file"></param> 本地文件路径
        void BeginDownload(string url, string file)
        {
            object[] param = new object[2] { url, file };
            ThreadEvent ev = new ThreadEvent();
            ev.key = NotiConst.UPDATE_DOWNLOAD;
            //添加实现了ICollection接口的一个集合的所有元素到指定集合的末尾
            ev.evParams.AddRange(param);
            //添加线程事件.通过线程开始执行下载
            FrameworkMain.Instance.ThreadMgr.AddEvent(ev, OnThreadCompleted);
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
        }
        #endregion
    }
}
