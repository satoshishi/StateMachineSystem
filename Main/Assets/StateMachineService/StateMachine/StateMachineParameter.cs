using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.Locator;

namespace StateMachineService.StateMachine
{
    public class StateMachineParameter : MonoBehaviour, IStateMachineParameter
    {
        [SerializeField]
        private GameObject m_stateMachineGameObject;

        [SerializeField]
        private Transform m_stateNodeRoot;     

        [SerializeField]
        private Transform m_prefabRoot;   

        [SerializeField]
        private StateMachineParameterSettings m_serviceSettings;
        public StateMachineParameterSettings ServiceSettings { set { m_serviceSettings = value; } }

        public List<IStateNodeService> StateNodes { get { return stateNodes; } }
        private List<IStateNodeService> stateNodes = null;

        public IStateNodeService FirstState { get { return firstState; } }
        private IStateNodeService firstState = null;

        public void Initialize()
        {
            stateNodes = Get_StateNodeServices();
            firstState = Get_FirstStateNodeServices();

            InstantiateAndRegisterServiceLocator_FromPrefab();
        }

        public void InstantiateAndRegisterServiceLocator_FromPrefab()
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

        public List<IStateNodeService> Get_StateNodeServices()
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

        public IStateNodeService Get_FirstStateNodeServices()
        {
            var _firstState = m_serviceSettings.FirstStateNodeGameObject.GetComponent<IStateNodeService>();
            if (_firstState == null)
                Debug.LogError($"{m_serviceSettings.FirstStateNodeGameObject.name} is not attach IStateNodeService");
            var targetState = StateNodes.Find(node => node.GetType() == _firstState.GetType());

            return targetState;
        }
    }
}