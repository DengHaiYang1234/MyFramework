using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFramework
{
    public class StateMachine<T> where T : class
    {
        //const只能在初期就使用常量初始化好。对于每一次编译后的结果，const的值是固定的，而readonly的值是可以在运行的时候才确定值的
        protected readonly Dictionary<int, StateBase<T>> _states = new Dictionary<int, StateBase<T>>();

        /// <summary>
        /// 总状态数量
        /// </summary>
        public int Count
        {
            get { return _states.Count; }
        }


        protected StateBase<T> _currState;
        /// <summary>
        /// 获取当前状态
        /// </summary>
        public StateBase<T> currState
        {
            get { return _currState; }
        }

        protected StateBase<T> _lastState;

        /// <summary>
        /// 获取上一次状态
        /// </summary>
        public StateBase<T> lastState
        {
            get { return _lastState; }
        }

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public int Add(StateBase<T> state)
        {
            if (_states.ContainsKey(state.id))
            {
                MyDebug.LogErrorFormat("alread contins state:" + state.id.ToString());
            }
            else
            {
                _states.Add(state.id, state);
            }
            return _states.Count;
        }

        /// <summary>
        /// 删除指定状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Remove(int id)
        {
            StateBase<T> state;
            if (_states.TryGetValue(id, out state))
                _states.Remove(id);
            else
                MyDebug.LogErrorFormat("not exist state:" + id.ToString());

            return _states.Count;
        }

        /// <summary>
        /// 删除所有状态
        /// </summary>
        public void RemoveAll()
        {
            _states.Clear();
            _currState = null;
        }

        /// <summary>
        /// 退出当前状态
        /// </summary>
        public void ExitCurrState(object param = null, bool isClearHistory = true)
        {
            if (_currState != null)
            {
                _currState.OnExit(param);
                _currState = null;
            }
        }

        /// <summary>
        /// 退出当前状态.执行指定状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <returns></returns>
        public virtual bool ChangeState(int id, object param1 = null, object param2 = null)
        {
            StateBase<T> state;
            if (_states.TryGetValue(id, out state))
            {
                if (state.IsValidChange(_currState, param1))
                {
                    if (_currState != null)
                        _currState.OnExit(param1); //退出当前状态
                    _lastState = _currState;
                    _currState = state;
                    //进入状态前可以提前设置参数  //state 指定状态
                    state.BeforeEnter(param2);
                    state.OnEnter(_lastState, param1);
                    if (!state.IsFinished)
                    {
                        state.OnAfterEnter(param1);
                    }

                    return true;
                }
                return false;
            }
            MyDebug.LogError("ChangeState  not exist state id:" + id.ToString());
            return false;
        }

        public void OnRunning(object param)
        {
            if (_currState == null)
                return;
            _currState.OnRunning(param);
        }

        public virtual StateBase<T> Find(int id)
        {
            StateBase<T> target;
            return _states.TryGetValue(id, out target) ? target : null;

        }

    }
}

