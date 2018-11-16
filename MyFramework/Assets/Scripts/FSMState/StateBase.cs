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

        public T Owner
        {
            get { return owner; }
        }

        private int _id;

        public int id
        {
            get { return _id; }
        }

        private bool _isFinished;

        public bool IsFinished
        {
            get { return _isFinished; }
        }

        protected StateBase(int id, T owner)
        {
            _id = id;
            this.owner = owner;
        }




        public virtual void OnEnter(StateBase<T> exitState, object param)
        {
        }

        public virtual void OnRunning(object param)
        {
        }

        public virtual void OnExit(object param)
        {
        }

        public virtual void OnAfterEnter(object param)
        {
        }

        public virtual bool IsValidChange(StateBase<T> currState, object param)
        {
            return currState == null || currState.id != this.id;
        }

        public virtual void BeforeEnter(object p)
        {

        }


    }
}

