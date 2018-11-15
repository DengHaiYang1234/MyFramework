using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFramework;

/// <summary>
/// 所有Manager的基类
/// </summary>
public class BaseClass : MonoBehaviour
{
    private ThreadManager m_ThreadMgr;
    private LuaManager m_LuaMgr;
    private HotManager m_hotFixMgr;
    private GamerManager m_GameMgr;
    private ResourceManager m_rescourMgr;

    Facade facde = new Facade();

    public ThreadManager ThreadManager_
    {
        get
        {
            if (m_ThreadMgr == null)
                m_ThreadMgr = facde.GetManager<ThreadManager>(ManagersName.thread);
            return m_ThreadMgr;
        }
    }

    public LuaManager LuaManager_
    {
        get
        {
            if (m_LuaMgr == null)
                m_LuaMgr = facde.GetManager<LuaManager>(ManagersName.lua);
            return m_LuaMgr;
        }
    }

    public HotManager HotManager_
    {
        get
        {
            if (m_hotFixMgr == null)
                m_hotFixMgr = facde.GetManager<HotManager>(ManagersName.hot);
            return m_hotFixMgr;
        }
    }


    public ResourceManager ResourceManager_
    {
        get
        {
            if (m_rescourMgr == null)
                m_rescourMgr = facde.GetManager<ResourceManager>(ManagersName.resource);

            return m_rescourMgr;
        }
    }

    public GamerManager GamerManager_
    {
        get
        {
            if (m_GameMgr == null)
                m_GameMgr = facde.GetManager<GamerManager>(ManagersName.game);
            return m_GameMgr;
        }
    }


}
