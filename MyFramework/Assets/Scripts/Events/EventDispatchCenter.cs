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


    public void Registry(string iMsgId, Action<object> callback, bool doImd)
    {
        LTEventCenter.Regist(iMsgId,callback,!doImd);
    }

    public void Registry(string iMsgId, Action<object> callback)
    {
        Registry(iMsgId, callback, false);
    }

    public void Dispatch(string iMsgId)
    {
        Dispatch(iMsgId, null);
    }

    public void Dispatch(string iMsgId,object obj)
    {
        LTEventCenter.SendNotify(iMsgId, obj);
    }

    public void UnRegistry(string iMSgId,Action<object> callback)
    {
        
    }

}
