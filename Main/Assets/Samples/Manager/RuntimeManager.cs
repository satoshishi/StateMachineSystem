using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.Param;

namespace Runtime
{

    public class RealtimeObj{}

    public class RuntimeManager : StateMachineParamManager
    {
        public class StateParamContainer : ParamManagerBase_RegisterInstanceAtRuntime<RealtimeObj>
        {

        }

        public StateParamContainer Collections
        {
            get;
            private set;
        } = null;

        private void Awake()
        {
            Collections = new StateParamContainer();
        }
    }
}