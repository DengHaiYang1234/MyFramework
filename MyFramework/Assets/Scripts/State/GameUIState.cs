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
            ResMain.Instance.CacheAtlas();
            ResMain.Instance.CacheUIPrefab();
            FrameworkMain.Instance.Run = true;
        }
        
        public override void OnRunning(object param)
        {
            base.OnRunning(param);
            if (ResMain.Instance.IsComplete)
            {
                ResMain.Instance.IsComplete = false;
                ShowGameTestUI();
                FrameworkMain.Instance.Run = false;
            }
        }

        public override void OnExit(object param)
        {
            base.OnExit(param);
        }


        private void ShowGameTestUI()
        {
            string path = "DownLoadPanel";
            LoadInitPrefab(path);
            lua.InitStart();
            lua.DoFile("Main.lua"); //加载文件，编译文件，并且返回一个函数，不运行。 

            Util.CallMethod("Main", "start");
            Util.CallMethod("Main", "SetValue");
        }



        void LoadInitPrefab(string name)
        {
            GameObject obj = ResMain.Instance.GetUIPrefab(name);
            //GameObject obj = res.CreatGamePrefab(path);
            obj.AddComponent<DownPanel>();
            var parent = GameObject.Find("SceneUI/layer1").gameObject;
            obj.transform.parent = parent.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            obj.transform.localScale = Vector3.one;
        }
    }
}

