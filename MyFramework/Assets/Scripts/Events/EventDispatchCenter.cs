using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFramework;

public class EventDispatchCenter
{
    private static EventDispatchCenter m_instance = null;

    public static EventDispatchCenter Instance
    {
        get {
            if (m_instance == null)
                m_instance = new EventDispatchCenter();
            return m_instance;
        }
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="iMsgId"> 事件ID </param>
    /// <param name="callback"> 事件回调 </param>
    /// <param name="doImd"> 是否立即执行 </param>
    public void Registry(string iMsgId, Action<object> callback, bool doImd)
    {
        LTEventCenter.Regist(iMsgId,callback,!doImd);
    }

    /// <summary>
    /// 注册队列事件
    /// </summary>
    /// <param name="iMsgId"> 事件ID </param>
    /// <param name="callback"> 事件回调 </param>
    public void Registry(string iMsgId, Action<object> callback)
    {
        Registry(iMsgId, callback, false);
    }
    /// <summary>
    /// 派发事件(不带参数)
    /// </summary>
    /// <param name="iMsgId"></param>
    public void Dispatch(string iMsgId)
    {
        Dispatch(iMsgId, null);
    }

    /// <summary>
    /// 派发事件(带回调参数)
    /// </summary>
    /// <param name="iMsgId"></param>
    /// <param name="obj"></param>
    public void Dispatch(string iMsgId,object obj)
    {
        LTEventCenter.SendNotify(iMsgId, obj);
    }

    /// <summary>
    /// 取消注册指定事件及回调
    /// </summary>
    /// <param name="iMSgId"></param>
    /// <param name="callback"></param>
    public void UnRegistry(string iMSgId,Action<object> callback)
    {
        LTEventCenter.UnRegist(iMSgId, callback);
    }

}
