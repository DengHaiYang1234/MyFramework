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
            if (!(Owner.HotMgr.IsInitLocalFile))
            {
                MyDebug.Log("HotManager初始化本地文件失败！开始下载服务器资源!");
            }
            Owner.HotMgr.UpdateResource();
            FrameworkMain.Instance.Run = true;
            base.OnEnter(exitState,param);
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


