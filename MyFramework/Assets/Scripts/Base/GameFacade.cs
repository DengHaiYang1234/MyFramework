using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFramework;

namespace MyFramework
{
    /// <summary>
    /// Facade单利 主要用来获取各脚本
    /// </summary>
    public class GameFacade : Facade
    {
        private static GameFacade _instance;

        public static GameFacade Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameFacade();
                return _instance;
            }
        }

        public void StartUp()
        {
            AddManager<ResourceManager>(ManagersName.resource);
            AddManager<ThreadManager>(ManagersName.thread);
            AddManager<LuaManager>(ManagersName.lua);
            AddManager<HotManager>(ManagersName.hot);
            AddManager<UIManager>(ManagersName.ui);
        }
    }

}
