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

        public override void BeforeEnter(object p)
        {
            if (!(hotFix.InitLoaclFilesInfo(false)))
                hotFix.InitLocalFilesInfoByResource();

            hotFix.Init();
        }

        public override void OnEnter(StateBase<FrameworkMain> exitState, object param)
        {
            if (!(hotFix.IsInit))
            {
                MyDebug.LogError("HotManager初始化失败！请检查");
                return;
            }
            hotFix.UpdateResource();
            FrameworkMain.Instance.Run = true;
        }

        public override void OnRunning(object param)
        {
            base.OnRunning(param);

            bool isComplete = hotFix.IsComplete();

            if (isComplete)
            {
                OnCheckUpdateComplete();
                isComplete = false;
            }
        }

        public override void OnExit(object param)
        {
            base.OnExit(param);
            FrameworkMain.Instance.Run = false;
        }

        public void OnCheckUpdateComplete()
        {
            Owner.FSM.ChangeState((int) MyStage.startGame);
        }
    }
}


