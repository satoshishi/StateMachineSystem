using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateParameter;
using StateMachineService.StateNode;
using StateMachineService.Locator;
using System;

namespace StateMachineService.Settings
{
    public class InitializeStateMachineServices : MonoBehaviour,IStateMachineIntializer
    {
        [SerializeField]
        private StateMachineServiceSettings m_serviceSettings;
        public StateMachineServiceSettings ServiceSettings { set { m_serviceSettings = value; } }

        [SerializeField]
        private GameObject m_serviceLocator;

        [SerializeField]
        private Transform m_stateNodeRoot;
        public Transform StateNodeRoot { set { m_stateNodeRoot = value; } }

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