using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.Locator;

namespace StateMachineService.StateMachine
{
    public class StateMachineParameter : MonoBehaviour,IStateMachineParameter
    {
        [SerializeField]
        private StateMachineParameterSettings m_serviceSettings;
        public StateMachineParameterSettings ServiceSettings { set { m_serviceSettings = value; } }

        [SerializeField]
        private GameObject m_serviceLocator;

        [SerializeField]
        private GameObject m_stateMachine;

        [SerializeField]
        private Transform m_stateNodeRoot;
        public Transform StateNodeRoot { set { m_stateNodeRoot = value; } }

        public IServiceLocator ServiceLocator{get{return serviceLocator;}}
        private IServiceLocator serviceLocator = null;

        public List<IStateNodeService> StateNodes{get{return stateNodes;}}
        private List<IStateNodeService> stateNodes = null;        

        public void Initialize()
        {
            serviceLocator = Get_ServiceLocator();
            stateNodes = Get_StateNodeServices();
            RegisterStateMachineToSerViceLocator();
        }

        public void RegisterStateMachineToSerViceLocator()
        {
            var stateMachine = m_stateMachine.GetComponent<IStateMachineService>();
            if(stateMachine == null) Debug.LogError($"{m_stateMachine.name} is not attach IStateMachineService");

            serviceLocator.Register(typeof(IStateMachineService),stateMachine);
        }

        public IServiceLocator Get_ServiceLocator()
        {
            var serviceLocator = m_serviceLocator.GetComponent<IServiceLocator>();

            if (serviceLocator == null)
                Debug.LogError($"{m_serviceLocator.name} is not attach IServiceLocator");

            foreach (GameObject target in m_serviceSettings.StateParameters_GameObject)
            {
                var intaller = GameObject.Instantiate(target, m_serviceLocator.transform).GetComponent<IPrefabServiceInstaller>();
                if (intaller == null)
                    Debug.LogError($"{target.name} is not attach IPrefabServiceInstaller");

                var parameter = intaller.Install();
                serviceLocator.Register(parameter.Key,parameter.Value);
            }      

            return serviceLocator;      
        }

        public List<IStateNodeService> Get_StateNodeServices()
        {
            var stateNodes = new List<IStateNodeService>();
            foreach (GameObject target in m_serviceSettings.StateNodes_GameObject)
            {
                var nodeObject = GameObject.Instantiate(target,m_stateNodeRoot);
                var nodeInstance = nodeObject.GetComponent<IStateNodeService>();

                if (nodeInstance == null)
                    Debug.LogError($"{target.name} is not attach IStateNodeService");

                stateNodes.Add(nodeInstance);
            }

            return stateNodes;
        }
    }
}