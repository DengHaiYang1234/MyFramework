using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MyFramework
{
    public static class LTEventCenter
    {
        public static bool ENBALE_THREAD = true;


        /// <summary>
        /// 在update内队列执行
        /// </summary>
        private static Dictionary<string, List<Action<object>>> _queueEventMap =
            new Dictionary<string, List<Action<object>>>();

        /// <summary>
        /// 立即执行
        /// </summary>
        private static Dictionary<string, List<Action<object>>> _eventMap =
            new Dictionary<string, List<Action<object>>>();

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="notifyID"> 消息ID </param>
        /// <param name="action"> 回调函数callback </param>
        /// <param name="inQueue"> 是否是Update队列中执行 </param>
        public static void Regist(string notifyID, Action<object> action, bool inQueue)
        {
            var serchDict = inQueue ? _queueEventMap : _eventMap;
            List<Action<object>> findList = null; //引用  回调集合
            if (serchDict.TryGetValue(notifyID, out findList))  // 已注册消息。判断是否有相同的回调
            {
                //已有相同的回调
                if (findList.Contains(action))
                {
                    MyDebug.LogErrorFormat("{0}已存在同样时间注册{1}，本次注册取消", notifyID, action);
                }
                else
                {
                    findList.Add(action); //添加该回调
                }
            }
            else //该消息还没有注册过回调函数
            {
                findList = new List<Action<object>>();
                findList.Add(action);

                serchDict.Add(notifyID, findList); //添加对应事件
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="notifyID">  消息ID </param>
        /// <param name="obj"> 消息参数 </param>
        public static void SendNotify(string notifyID, object obj)
        {
            bool bCall = false;
            List<Action<object>> findList = null;
            if (_eventMap.TryGetValue(notifyID, out findList)) //若在立即执行集合中找到该消息。那么立即执行该消息
            {
                for (int i = 0; i < findList.Count; i++)
                {
                    DoActionImd(findList[i], obj);
                    bCall = true;
                }
            }

            if (_queueEventMap.TryGetValue(notifyID, out findList)) //若需要在Update中通过队列执行
            {
                for (int i = 0; i < findList.Count; i++)
                {
                    if (ENBALE_THREAD) //开启多线程。启用队列处理
                    {
                        bCall = true;
                        DoActionInQueue(findList[i], obj); //一个消息。多个回调
                    }
                    else
                    {
                        bCall = true;
                        DoActionImd(findList[i], obj);
                    }
                }
            }

            if (!bCall)
            {
                MyDebug.LogErrorFormat("Dispatch called SendNotify is successd! but not found this callBack");
            }
        }

        /// <summary>
        /// 立即执行回调
        /// </summary>
        /// <param name="action"> callback </param>
        /// <param name="obj"> 回调参数 </param>
        private static void DoActionImd(Action<object> action, object obj)
        {
            try
            {
                action(obj);
            }
            catch(Exception e)
            {
                MyDebug.LogError(e);
            }
        }

        /// <summary>
        /// 依次处理回调
        /// </summary>
        /// <param name="action"></param>
        /// <param name="obj"></param>
        private static void DoActionInQueue(Action<object> action,object obj)
        {
            Loom.QueueOnMainThread(action,obj);
        }


        public static void UnRegist(string notifyID, Action<object> action)
        {
            UnRegist(notifyID, action, true);//清空队列表
            UnRegist(notifyID, action, false);//清空立即执行表
        }

        /// <summary>
        /// 取消注册事件 
        /// </summary>
        /// <param name="notifyID"> 消息ID </param>
        /// <param name="action"> callback </param>
        /// <param name="inQueue"> 是否启动线程 </param>
        public static void UnRegist(string notifyID, Action<object> action, bool inQueue)
        {
            var searchDict = inQueue ? _queueEventMap : _eventMap;

            List<Action<object>> findList = null;
            if (searchDict.TryGetValue(notifyID, out findList))
            {
                if (findList.Contains(action))  //删除该消息下的回调
                {
                    findList.Remove(action);
                }


                if (findList.Count == 0)
                {
                    searchDict.Remove(notifyID);  //回调都已被删除.那么清楚该消息
                }
            }

        }
    }
}


