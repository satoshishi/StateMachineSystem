using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.Param;

namespace STM.DomainModel
{
    public class StateMachineDomainModel : MonoBehaviour
    {
        [SerializeField]
        private StateMachineSettings m_stmSettings;
        public StateMachineSettings STMSettings
        {
            get
            {
                return m_stmSettings;
            }
            set
            {
                m_stmSettings = value;
            }
        }

        [SerializeField]
        private Transform m_stateNodeRoot;
        public Transform STNRoot
        {
            get{return m_stateNodeRoot;}
        }

        [SerializeField]
        public Transform m_retentionItemsRoot;
        public Transform RetentionRoot
        {
            get{return m_retentionItemsRoot;}
        }

        [SerializeField]
        private StateParamManager m_stateParameter;
        public StateParamManager StateParameter
        {
            get { return m_stateParameter; }
        }
    }
}