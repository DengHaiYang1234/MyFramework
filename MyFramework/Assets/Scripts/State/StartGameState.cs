using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Res;


namespace MyFramework
{
    /// <summary>
    /// 显示UI
    /// </summary>
    public class StartGameState : BaseStage
    {
        private ResourceManager res;
        private LuaManager lua;

        public StartGameState(FrameworkMain owner) : base((int) MyStage.startGame, owner)
        {
            res = owner.GetManager<ResourceManager>(ManagersName.resource);
            lua = owner.GetManager<LuaManager>(ManagersName.lua);
        }
        public override void BeforeEnter(object p)
        {
            if (!res.Init())
            {
                MyDebug.LogErrorFormat("ResourceManager.Init failed!");
            }
        }

        public override void OnEnter(StateBase<FrameworkMain> exitState, object param)
        {
            base.OnEnter(exitState, param);
            LoadInitPrefab();
            FrameworkMain.Instance.Run = true;
        }
        
        public override void OnRunning(object param)
        {
            base.OnRunning(param);
            //if (ResMain.Instance.IsComplete)
            //{
            //    ResMain.Instance.IsComplete = false;
            //    ShowGameTestUI();
            //    FrameworkMain.Instance.Run = false;
            //}
        }

        public override void OnExit(object param)
        {
            base.OnExit(param);
        }

        private void ShowGameTestUI()
        {
            string path = "DownLoadPanel";
            //LoadInitPrefab(path);
            lua.InitStart();
            lua.DoFile("Main.lua"); //加载文件，编译文件，并且返回一个函数，不运行。 

            Util.CallMethod("Main", "start");
            Util.CallMethod("Main", "SetValue");
        }
        
        void LoadInitPrefab()
        {
            res.LoadSync<GameObject>("DownLoadPanel",LoadSyncCallback);
        }
        
        void LoadSyncCallback(MyAsset asset)
        {
            if (asset != null)
            {
                var prefab = asset.asset;
                if (prefab != null)
                {
                    var go = GameObject.Instantiate(prefab) as GameObject;
                    go.AddComponent<DownPanel>();
                    var parent = GameObject.Find("SceneUI/layer1").gameObject;
                    go.transform.parent = parent.transform;
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    go.transform.localScale = Vector3.one;
                }
            }
        }
    }
}

