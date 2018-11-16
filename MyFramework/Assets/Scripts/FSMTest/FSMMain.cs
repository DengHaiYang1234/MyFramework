using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyFramework
{
    //public class FSMMain : MonoBehaviour
    //{
    //    public bool walk = false;
    //    public bool dead = false;


    //    private static FSMMain _instance;
    //    private void Awake()
    //    {
    //        _instance = this;
    //        InitFSM();
    //    }

    //    public static FSMMain Instance
    //    {
    //        get { return _instance; }
    //    }

    //    private readonly StateMachine<FSMMain> _gameStateFsm = new StateMachine<FSMMain>();

    //    public StateMachine<FSMMain> FSM
    //    {
    //        get { return _gameStateFsm; }
    //    }

    //    private void InitFSM()
    //    {
    //        _gameStateFsm.Add(new State_Walk(this));
    //        _gameStateFsm.Add(new State_Dead(this));
    //    }


    //    private void Update()
    //    {
    //        if (walk && !dead)
    //        {
    //            FSM.ChangeState((int)EStage_Test.Walk);
    //            if (FSM.currState.id == (int)EStage_Test.Walk)
    //            {
    //                FSM.OnRunning(EStage_Test.Walk);
    //            }
    //        }
    //        else if (!walk && dead)
    //        {
    //            bool isStart = FSM.ChangeState((int)EStage_Test.Dead);
    //            if (FSM.currState.id == (int)EStage_Test.Dead)
    //            {
    //                FSM.OnRunning(EStage_Test.Dead);
    //            }
    //        }
    //        else if (!walk && !dead)
    //        {
    //            if (FSM.currState != null)
    //            {
    //                SDDebug.LogError("退出当前状态：" +  FSM.currState.id);
    //                FSM.ExitCurrState();
    //                SDDebug.LogError("当前状态：" + FSM.currState);
    //            }
    //        }
    //        else if (walk && dead)
    //        {
    //            //根据当前状态切换.
    //            Debug.Log("当前状态：" + FSM.currState.id);
    //            if (FSM.currState.id == (int)EStage_Test.Dead)
    //            {
    //                FSM.ChangeState((int)EStage_Test.Walk);
    //            }
    //            else
    //            {
    //                FSM.ChangeState((int)EStage_Test.Dead);
    //            }
    //            Debug.Log("切换至状态：" + FSM.currState.id);
    //        }
    //    }
    //}
}


