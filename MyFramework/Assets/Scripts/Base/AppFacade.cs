using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFramework;

namespace MyFramework
{
    /// <summary>
    /// Facade单利 主要用来获取各脚本
    /// </summary>
    public class AppFacade : Facade
    {
        private static AppFacade _instance;

        public static AppFacade Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AppFacade();
                return _instance;
            }
        }

        public void StartUp()
        {
            AddManager<ResourceManager>(ManagersName.resource);
            AddManager<ThreadManager>(ManagersName.thread);
            AddManager<LuaManager>(ManagersName.lua);
            AddManager<HotManager>(ManagersName.hot);
            AddManager<GamerManager>(ManagersName.game);
        }

    }

}
