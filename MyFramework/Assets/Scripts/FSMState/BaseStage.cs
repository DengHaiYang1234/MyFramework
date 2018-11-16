using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFramework
{
    public class BaseStage : StateBase<FrameworkMain>
    {
        public BaseStage(int id, FrameworkMain owner) : base(id, owner)
        {

        }

        public override void OnEnter(StateBase<FrameworkMain> exitState, object param)
        {
            base.OnEnter(exitState, param);
        }

        public override void OnExit(object param)
        {
            base.OnExit(param);
        }
    }
}

