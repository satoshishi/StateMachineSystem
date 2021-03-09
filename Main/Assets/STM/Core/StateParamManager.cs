using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace STM.Param
{
    /// <summary>
    /// StateParameterを継承するParameterManagerを管理する
    /// </summary>
    public class StateParamManager : MonoBehaviour
    {
        public class StateParamContainer : ParamManagerBase_RegisterInstanceAtRuntime<StateMachineParamManager>
        {
            public override void Initialize(List<StateMachineParamManager> parameters)
            {

                Parameters = new List<StateMachineParamManager>(parameters);
            }
        }

        public StateParamContainer Collections
        {
            get;
            private set;
        } = null;

        public void Initialize(List<StateMachineParamManager> parameters)
        {
            Collections = new StateParamContainer();
            Collections.Initialize(parameters);
        }
    }

    public class StateMachineParamManager : MonoBehaviour
    {

    }
}