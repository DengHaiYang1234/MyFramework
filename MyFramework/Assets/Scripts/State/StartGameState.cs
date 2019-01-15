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
        //private ResourceManager res;
        //private LuaManager lua;
        private float time = 0f;

        public StartGameState(FrameworkMain owner) : base((int) MyStage.startGame, owner)
        {
            //res = owner.GetManager<ResourceManager>(ManagersName.resource);
            //lua = owner.GetManager<LuaManager>(ManagersName.lua);
        }

        public override void BeforeEnter(object p)
        {
            if (!Owner.ResMgr.Init())
            {
                MyDebug.LogErrorFormat("ResourceManager.Init failed!");
                return;
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
            //time += Time.deltaTime;
            //if (time > 15)
            //{
            //    MyDebug.LogError("再次清理资源");
            //    Resources.UnloadUnusedAssets();
            //    time = 0f;
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
            Owner.LuaMgr.InitStart();
            Owner.LuaMgr.DoFile("Main.lua"); //加载文件，编译文件，并且返回一个函数，不运行。 

            Util.CallMethod("Main", "start");
            Util.CallMethod("Main", "SetValue");
        }
        
        void LoadInitPrefab()
        {
            Owner.ResMgr.LoadSync<GameObject>("DownLoadPanel",LoadSyncCallback);
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
                    ReleaseAssetOnDestroy.Register(go, asset);
                    var parent = GameObject.Find("SceneUI/layer1").gameObject;
                    go.transform.parent = parent.transform;
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    go.transform.localScale = Vector3.one;
                    GameObject.Destroy(go, 10);
                }
            }

            Owner.LuaMgr.InitStart();
            Owner.LuaMgr.DoFile("Main.lua"); //加载文件，编译文件，并且返回一个函数，不运行。 

            Util.CallMethod("Main", "start");
            Util.CallMethod("Main", "SetValue");
        }
    }
}

