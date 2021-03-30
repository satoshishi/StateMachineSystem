using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.Locator;

namespace StateMachineService.StateMachine
{
    public class StateMachineParameter : MonoBehaviour, IStateMachineParameter
    {
        [SerializeField]
        private GameObject m_serviceLocatorGameObject;

        [SerializeField]
        private GameObject m_stateMachineGameObject;

        [SerializeField]
        private StateMachineParameterSettings m_serviceSettings;
        public StateMachineParameterSettings ServiceSettings { set { m_serviceSettings = value; } }

        [SerializeField]
        private Transform m_stateNodeRoot;
        public Transform StateNodeRoot { set { m_stateNodeRoot = value; } }

        public IServiceLocator ServiceLocator { get { return serviceLocator; } }
        private IServiceLocator serviceLocator = null;

        public List<IStateNodeService> StateNodes { get { return stateNodes; } }
        private List<IStateNodeService> stateNodes = null;

        public IStateNodeService FirstState { get { return firstState; } }
        private IStateNodeService firstState = null;

        public void Initialize()
        {
            serviceLocator = Get_ServiceLocator();
            stateNodes = Get_StateNodeServices();
            firstState = Get_FirstStateNodeServices();
            RegisterStateMachineToSerViceLocator();
        }

        public void RegisterStateMachineToSerViceLocator()
        {
            var stateMachine = m_stateMachineGameObject.GetComponent<IStateMachineService>();
            if (stateMachine == null) Debug.LogError($"{m_stateMachineGameObject.name} is not attach IStateMachineService");

            serviceLocator.Register(typeof(IStateMachineService), stateMachine);
        }

        public IServiceLocator Get_ServiceLocator()
        {
            var serviceLocator = m_serviceLocatorGameObject.GetComponent<IServiceLocator>();

            if (serviceLocator == null)
                Debug.LogError($"{m_serviceLocatorGameObject.name} is not attach IServiceLocator");

            foreach (GameObject target in m_serviceSettings.StateParametersGameObject)
            {
                var intaller = GameObject.Instantiate(target, m_serviceLocatorGameObject.transform).GetComponent<IPrefabServiceInstaller>();
                if (intaller == null)
                    Debug.LogError($"{target.name} is not attach IPrefabServiceInstaller");

                var parameter = intaller.Install();
                serviceLocator.Register(parameter.Key, parameter.Value);
            }

            return serviceLocator;
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
            IStateNodeService _firstState = default;

            var nodeObject = GameObject.Instantiate(m_serviceSettings.FirstStateNodeGameObject, m_stateNodeRoot);
            _firstState = nodeObject.GetComponent<IStateNodeService>();

            if (_firstState == null)
                Debug.LogError($"{m_serviceSettings.FirstStateNodeGameObject.name} is not attach IStateNodeService");

            return _firstState;
        }
    }
}