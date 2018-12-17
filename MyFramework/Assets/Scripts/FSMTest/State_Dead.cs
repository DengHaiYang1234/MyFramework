using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFramework
{

    /// <summary>
    /// 测试启动需将BaseStage类 泛型T Main改变为FSMMain改变为
    /// </summary>

    //public class State_Dead : BaseStage
    //{

    //    private bool _hasInit = false;

    //    public State_Dead(FSMMain owner) : base((int)EStage_Test.Dead,owner)
    //    {

    //    }

    //    public override void OnEnter(StateBase<FSMMain> exitState, object param)
    //    {
    //        base.OnEnter(exitState, param);
    //        if (!_hasInit)
    //        {
    //            MyDebug.LogError("角色死亡-进入");
    //            _hasInit = true;
    //        }
    //        else
    //        {
    //            MyDebug.LogError("状态已开启:" + EStage_Test.Dead.ToString());
    //        }
    //    }


    //    public override void OnRunning(object param)
    //    {
    //        base.OnRunning(param);
    //        if (_hasInit)
    //        {
    //            MyDebug.LogError("角色死亡-执行中");
    //        }
    //        else
    //        {
    //            MyDebug.LogError("状态未开启:" + EStage_Test.Dead.ToString());
    //        }
    //    }

    //    public override void OnExit(object param)
    //    {
    //        base.OnExit(param);
    //        if (_hasInit)
    //        {
    //            MyDebug.LogError("角色死亡-退出");
    //            _hasInit = false;
    //        }
    //        else
    //        {
    //            MyDebug.LogError("状态未开启:" + EStage_Test.Dead.ToString());
    //        }
    //    }
    //}
}

