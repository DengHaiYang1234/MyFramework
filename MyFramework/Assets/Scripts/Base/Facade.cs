using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 组件操作类
/// </summary>
public class Facade
{
    static GameObject m_GameManager;
    static Dictionary<string, object> m_Managers = new Dictionary<string, object>();

    GameObject AppGameManager
    {
        get
        {
            if (m_GameManager == null)
                m_GameManager = GameObject.Find("Launcher");

            return m_GameManager;
        }
    }

    /// <summary>
    /// 添加Manager
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public T AddManager<T>(string typeName) where T : Component
    {
        object result = null;
        m_Managers.TryGetValue(typeName, out result);
        if (result != null)
            return (T)result;

        Component c = AppGameManager.AddComponent<T>();
        m_Managers.Add(typeName, c);
        return default(T);
    }

    /// <summary>
    /// 获取Manager
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public T GetManager<T>(string typeName) where T : Component
    {
        if (!m_Managers.ContainsKey(typeName))
            return default(T);

        object manager = null;
        m_Managers.TryGetValue(typeName, out manager);
        return (T)manager;
    }


}
