using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFramework
{
    //public class State_Walk : BaseStage
    //{
    //    private bool _hasInit = false;

    //    public State_Walk(FSMMain owner) : base((int)EStage_Test.Walk, owner)
    //    {

    //    }

    //    public override void OnEnter(StateBase<FSMMain> exitState, object param)
    //    {
    //        base.OnEnter(exitState, param);
    //        if (!_hasInit)
    //        {
    //            SDDebug.LogError("角色行走-进入");
    //            _hasInit = true;
    //        }
    //        else
    //        {
    //            SDDebug.LogError("状态已开启:" + EStage_Test.Dead.ToString());
    //        }
    //    }


    //    public override void OnRunning(object param)
    //    {
    //        base.OnRunning(param);
    //        if (_hasInit)
    //        {
    //            SDDebug.LogError("角色行走-执行中");
    //        }
    //        else
    //        {
    //            SDDebug.LogError("状态未开启:" + EStage_Test.Dead.ToString());
    //        }
    //    }

    //    public override void OnExit(object param)
    //    {
    //        base.OnExit(param);
    //        if (_hasInit)
    //        {
    //            SDDebug.LogError("角色行走-退出");
    //            _hasInit = false;
    //        }
    //        else
    //        {
    //            SDDebug.LogError("状态未开启:" + EStage_Test.Dead.ToString());
    //        }
    //    }
    //}
}


