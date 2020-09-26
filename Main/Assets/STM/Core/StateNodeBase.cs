using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using STM.Param;

namespace STM.STN
{
    [Serializable]
    public abstract class StateNodeBase : MonoBehaviour
    {
        protected Transform RetentionItemsRoot
        {
            get;
            private set;
        } = null;

        protected StateParamManager StateParameter
        {
            get;
            private set;
        } = null;

        public virtual void Initialize(Transform retentionItemsRoot,StateParamManager stateParameter)
        {
            RetentionItemsRoot = retentionItemsRoot;
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