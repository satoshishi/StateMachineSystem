using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using StateMachineService.Locator;

namespace StateMachineService.StateNode
{
    [AutoInstallAttribute(typeof(IStateNodeList))]
    public class StateNodeList : MonoBehaviour, IStateNodeList
    {
        public List<IStateNodeService> StateNodes { get => stateNodes; }
        private List<IStateNodeService> stateNodes;

        public IStateNodeService FirstState { get => firstState; }
        private IStateNodeService firstState;

        [SerializeField]
        private StateNodeSettings m_settings;
        public void SetStateNodeSettings(StateNodeSettings settings) => m_settings = settings;

        [SerializeField]
        private Transform m_stateNodeRoot;


        public IStateNodeService GetStateNode<STATE_NODE>() where STATE_NODE : IStateNodeService
        {
            var res = StateNodes.Find(statenode => statenode.GetType() == typeof(STATE_NODE));

            return res == null ? null : res;
        }

        public IStateNodeService GetStateNode(Type type)
        {
            var res = StateNodes.Find(statenode => statenode.GetType() == type);

            return res == null ? null : res;
        }

        public virtual void Initialize(GameObject obj)
        {
            stateNodes = Get_StateNodeServices();
            firstState = Get_FirstStateNodeServices();
        }

        protected virtual List<IStateNodeService> Get_StateNodeServices()
        {
            var stateNodes = new List<IStateNodeService>();
            foreach (GameObject target in m_settings.StateNodesGameObject)
            {
                var nodeObject = GameObject.Instantiate(target, m_stateNodeRoot);
                var nodeInstance = nodeObject.GetComponent<IStateNodeService>();

                if (nodeInstance == null)
                    Debug.LogError($"{target.name} is not attach IStateNodeService");

                stateNodes.Add(nodeInstance);
            }

            return stateNodes;
        }

        protected virtual IStateNodeService Get_FirstStateNodeServices()
        {
            var _firstState = m_settings.FirstStateNodeGameObject.GetComponent<IStateNodeService>();
            if (_firstState == null)
                Debug.LogError($"{m_settings.FirstStateNodeGameObject.name} is not attach IStateNodeService");
            var targetState = StateNodes.Find(node => node.GetType() == _firstState.GetType());

            return targetState;
        }
    }
}