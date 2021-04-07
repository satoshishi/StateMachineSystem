using System.Collections.Generic;
using System;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.Locator;

namespace StateMachineService.StateMachine.Parameter
{
    [DefaultExecutionOrder(-9999)]    
    public class StateMachineParameter : MonoBehaviour, IStateMachineParameter
    {
        [SerializeField]
        private Transform m_stateNodeRoot;     

        [SerializeField]
        private Transform m_prefabRoot;   
        
        public IStateNodeService FirstState { get { return firstState; } }
        private IStateNodeService firstState = null;        

        [SerializeField]
        private StateMachineParameterSettings m_serviceSettings;
        public StateMachineParameterSettings ServiceSettings { set { m_serviceSettings = value; } }

        public List<IStateNodeService> StateNodes { get { return stateNodes; } }
        private List<IStateNodeService> stateNodes = null;

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

        private void Awake()
        {
            stateNodes = Get_StateNodeServices();
            firstState = Get_FirstStateNodeServices();
            InstantiateAndRegisterServiceLocator_FromPrefab();

            ServiceLocator.Register<IStateMachineParameter>(this);
        }

        private void InstantiateAndRegisterServiceLocator_FromPrefab()
        {
            foreach (GameObject target in m_serviceSettings.StateParametersGameObject)
            {
                var intaller = GameObject.Instantiate(target, m_prefabRoot.transform).GetComponent<IPrefabServiceInstaller>();
                if (intaller != null)
                {
                    var parameter = intaller.Install();
                    ServiceLocator.Register(parameter.Key, parameter.Value);
                }
            }
        }

        private List<IStateNodeService> Get_StateNodeServices()
        {
            var stateNodes = new List<IStateNodeService>();
            foreach (GameObject target in m_serviceSettings.StateNodesGameObject)
            {
                var nodeObject = GameObject.Instantiate(target, m_stateNodeRoot);
                var nodeInstance = nodeObject.GetComponent<IStateNodeService>();

                if (nodeInstance == null)
                    Debug.LogError($"{target.name} is not attach IStateNodeService");

                stateNodes.Add(nodeInstance);
            }

            return stateNodes;
        }

        private IStateNodeService Get_FirstStateNodeServices()
        {
            var _firstState = m_serviceSettings.FirstStateNodeGameObject.GetComponent<IStateNodeService>();
            if (_firstState == null)
                Debug.LogError($"{m_serviceSettings.FirstStateNodeGameObject.name} is not attach IStateNodeService");
            var targetState = StateNodes.Find(node => node.GetType() == _firstState.GetType());

            return targetState;
        }
    }
}