using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyFramework
{
    /// <summary>
    /// 显示UI
    /// </summary>
    public class GameUIState : BaseStage
    {
        private ResourceManager res;
        private LuaManager lua;

        public GameUIState(FrameworkMain owner) : base((int) MyStage.gameUI,owner)
        {
            res = owner.GetManager<ResourceManager>(ManagersName.resource);
            lua = owner.GetManager<LuaManager>(ManagersName.lua);
        }

        public override void OnEnter(StateBase<FrameworkMain> exitState, object param)
        {
            base.OnEnter(exitState, param);
            //FrameworkMain.Instance.Run = true;
            ResMain.Instance.InitCacheAtlas();
            ResMain.Instance.InitCacheUIPrefab();
        }


        public override void OnRunning(object param)
        {
            base.OnRunning(param);
        }

        public override void OnExit(object param)
        {
            base.OnExit(param);
        }


        private void ShowGameTestUI()
        {
            string path = "game/DownLoadPanel";
            LoadInitPrefab(path);
            lua.InitStart();
            lua.DoFile("Main.lua"); //加载文件，编译文件，并且返回一个函数，不运行。 

            Util.CallMethod("Main", "start");
            Util.CallMethod("Main", "SetValue");
        }

        void LoadInitPrefab(string path)
        {
            GameObject obj = res.CreatGamePrefab(path);
            var parent = GameObject.Find("UI_Canvas").gameObject;
            obj.transform.parent = parent.transform;
            obj.AddComponent<DownPanel>();
            obj.transform.localPosition = Vector3.zero;
        }
    }
}

