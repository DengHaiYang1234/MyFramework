using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MyFramework;

namespace MyFramework
{
    /// <summary>
    /// 检查更新
    /// </summary>
    public class CheckStage : BaseStage
    {
        private HotManager hotFix;
        private ResourceManager res;

        public CheckStage(FrameworkMain owner) : base((int) MyStage.check, owner)
        {
            hotFix = owner.GetManager<HotManager>(ManagersName.hot);
            res = owner.GetManager<ResourceManager>(ManagersName.resource);
        }

        public override void OnEnter(StateBase<FrameworkMain> exitState, object param)
        {
            base.OnEnter(exitState, param);
            hotFix.Init();
            bool isExists = Directory.Exists(Util.DataPath) && Directory.Exists(Util.DataPath + "lua/") && File.Exists(Util.DataPath + "files.txt");
            if (isExists || AppConst.DebugMode)
            {
                hotFix.UpdateResource();
            }
            else
            {
                hotFix.ExtractResource();
            }

            FrameworkMain.Instance.Run = true;
        }

        public override void OnRunning(object param)
        {
            base.OnRunning(param);

            bool isComplete = hotFix.IsComplete();

            if (isComplete)
                OnCheckUpdateComplete();
        }

        public override void OnExit(object param)
        {
            base.OnExit(param);
            FrameworkMain.Instance.Run = false;
        }

        public void OnCheckUpdateComplete()
        {
            //res.Initialize();

            Owner.FSM.ChangeState((int) MyStage.startGame);
        }
    }
}


