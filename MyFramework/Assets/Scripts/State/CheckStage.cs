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
        public CheckStage(FrameworkMain owner) : base((int) MyStage.check, owner)
        {
        }

        public override void BeforeEnter(object p)
        {
            if (!(Owner.HotMgr.InitLoaclFilesInfo(false)))
                Owner.HotMgr.InitLocalFilesInfoByResource();

            Owner.HotMgr.Init();
        }

        public override void OnEnter(StateBase<FrameworkMain> exitState, object param)
        {
            //if (!(Owner.HotMgr.IsInit))
            //{
            //    MyDebug.LogError("HotManager初始化失败！请检查");
            //    return;
            //}
            Owner.HotMgr.UpdateResource();
            FrameworkMain.Instance.Run = true;
        }

        public override void OnRunning(object param)
        {
            base.OnRunning(param);

            bool isComplete = Owner.HotMgr.IsComplete();

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


