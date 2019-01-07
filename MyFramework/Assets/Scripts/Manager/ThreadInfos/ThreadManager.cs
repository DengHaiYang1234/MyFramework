using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using UnityEngine;

namespace MyFramework
{
    public class NotiData
    {
        #region 线程消息数据结构
        public string evName;
        public object evParam;

        public NotiData(string name,object param)
        {
            this.evName = name;
            this.evParam = param;
        }
        #endregion
    }


    public class ThreadManager : MonoBehaviour
    {
        #region 线程下载管理类
        private Thread thread;
        //回调
        private Action<NotiData> func;
        //提供一组方法和属性，可用于准确地测量运行时间。
        private Stopwatch sw = new Stopwatch();
        //当前下载的文件
        private string currDownFile = string.Empty;
        //lock
        private static readonly object m_lockObj = new object();
        //线程队列
        private static Queue<ThreadEvent> events = new Queue<ThreadEvent>();

        private delegate void ThreadSyncEvent(NotiData data);

        private ThreadSyncEvent m_SyncEvent;
        
        private void Awake()
        {
            m_SyncEvent = OnSyncEvent;
            thread = new Thread(OnUpdate);
        }
        //启动线程
        private void Start()
        {
            thread.Start();
        }
        //回调
        void OnSyncEvent(NotiData data)
        {
            if (this.func != null)
                this.func(data);
        }
        //新的下载任务加入线程队列
        public void AddEvent(ThreadEvent ev, Action<NotiData> func)
        {
            lock (m_lockObj)
            {
                this.func = func;

                events.Enqueue(ev);
            }
        }
        //每帧执行  看是否有新队列可以执行下载任务
        void OnUpdate()
        {
            while (true)
            {
                lock (m_lockObj)
                {
                    if (events.Count > 0)
                    {
                        ThreadEvent e = events.Dequeue();
                        try
                        {
                            switch (e.key)
                            {
                                //解包
                                case NotiConst.UPDATE_EXTRACT:
                                {
                                    OnExtractFile(e.evParams);
                                }
                                    break;
                                //下载
                                case NotiConst.UPDATE_DOWNLOAD:
                                {
                                    OnDownloadFile(e.evParams);
                                }
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            UnityEngine.Debug.LogError(ex.Message);
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        void OnExtractFile(List<object> evParams)
        {
            NotiData data = new NotiData(NotiConst.UPDATE_EXTRACT, null);
            if (m_SyncEvent != null) m_SyncEvent(data);
        }

        void OnDownloadFile(List<object> evparam)
        {
            //下载路径
            string url = evparam[0].ToString();
            //本地路径
            currDownFile = evparam[1].ToString();
            using(WebClient client = new WebClient())
            {
                sw.Start();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                // 将具有指定 URI 的资源下载到本地文件。此方法不会阻止调用线程。
                //url:要下载的资源的 URI。
                //currDownFile:要放在本地计算机上的文件的名称。
                client.DownloadFileAsync(new System.Uri(url), currDownFile);
            }
        }

        void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            string value = string.Format("{0}kb/s", (e.BytesReceived/1024d/sw.Elapsed.TotalSeconds).ToString("0.00"));
            NotiData data = new NotiData(NotiConst.UPDATE_PROGRESS, value);
            //下载成功回调
            if (m_SyncEvent != null) m_SyncEvent(data);

            if (e.ProgressPercentage == 100 && e.BytesReceived == e.TotalBytesToReceive)
            {
                sw.Reset();
                data = new NotiData(NotiConst.UPDATE_DOWNLOAD, currDownFile);
                //防止下载完成并不会执行回调
                FrameworkMain.Instance.HotMgr.downLoadFiles.Add(data.evParam.ToString());
                if (m_SyncEvent != null) m_SyncEvent(data);
            }
        }

        private void OnDestroy()
        {
            thread.Abort();
        }

        #endregion
    }
}


