using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFramework
{
    /// <summary>
    /// 状态机
    /// </summary>
    public abstract class StateBase<T> where T : class
    {
        private T owner;
        /// <summary>
        /// 当前状态
        /// </summary>
        public T Owner
        {
            get { return owner; }
        }

        private int _id;
        /// <summary>
        /// 当前状态ID
        /// </summary>
        public int id
        {
            get { return _id; }
        }

        protected bool _isFinished;
        /// <summary>
        /// 当前状态是否已完成
        /// </summary>
        public bool IsFinished
        {
            get { return _isFinished; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="owner"></param>
        protected StateBase(int id, T owner)
        {
            _id = id;
            this.owner = owner;
        }
        
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="exitState"></param>
        /// <param name="param"></param>
        public virtual void OnEnter(StateBase<T> exitState, object param)
        {
            
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        /// <param name="param"></param>
        public virtual void OnRunning(object param)
        {
            
        }

        /// <summary>
        /// 退出状态
        /// </summary>
        /// <param name="param"></param>
        public virtual void OnExit(object param)
        {
        }
        /// <summary>
        /// 进入状态后
        /// </summary>
        /// <param name="param"></param>
        public virtual void OnAfterEnter(object param)
        {
        }
        /// <summary>
        /// 状态切换是否有效
        /// </summary>
        /// <param name="currState"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual bool IsValidChange(StateBase<T> currState, object param)
        {
            return currState == null || currState.id != this.id;
        }
        /// <summary>
        /// 进入状态之前
        /// </summary>
        /// <param name="p"></param>
        public virtual void BeforeEnter(object p)
        {

        }


    }
}

