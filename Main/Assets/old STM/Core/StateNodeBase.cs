using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using STM.Param;

namespace STM.STN
{
    public class StateNodeBase : MonoBehaviour
    {

        protected StateParamManager StateParameter
        {
            get;
            private set;
        } = null;

        public virtual void Initialize(StateParamManager stateParameter)
        {
            StateParameter = stateParameter;
        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnExit()
        {
            foreach(Transform child in this.transform)
                Destroy(child.gameObject);
        }
    }
}