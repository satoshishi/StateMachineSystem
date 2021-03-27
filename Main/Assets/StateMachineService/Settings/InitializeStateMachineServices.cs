using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateParameterRepository;
using StateMachineService.StateParameter;
using StateMachineService.StateNode;

namespace StateMachineService.Settings
{
    public class InitializeStateMachineServices : MonoBehaviour,IStateMachineIntializer
    {
        [SerializeField]
        private StateMachineServiceSettings m_serviceSettings;
        public StateMachineServiceSettings ServiceSettings { set { m_serviceSettings = value; } }

        [SerializeField]
        private Transform m_stateNodeRoot;
        public Transform StateNodeRoot { set { m_stateNodeRoot = value; } }

        [SerializeField]
        private Transform m_repositoryRoot;
        public Transform RepositoryRoot { set { m_repositoryRoot = value; } }

        public IStateParameterRepository Get_StateParameterRepository()
        {
            var repositoryTransform = GameObject.Instantiate(m_serviceSettings.StateParameterRepository_GameObject, m_repositoryRoot).transform;
            var stateParameterRepository = repositoryTransform.GetComponent<IStateParameterRepository>();

            if (stateParameterRepository == null)
                Debug.LogError($"{m_serviceSettings.StateParameterRepository_GameObject.name} is not attach IStateParameterRepository");

            var stateParameters = new List<IStateParameter>();
            foreach (GameObject target in m_serviceSettings.StateParameters_GameObject)
            {
                var parameter = GameObject.Instantiate(target, repositoryTransform).GetComponent<IStateParameter>();
                if (parameter == null)
                    Debug.LogError($"{target.name} is not attach IStateParameter");

                stateParameters.Add(parameter);
            }      

            stateParameterRepository.Initialize(stateParameters);
            return stateParameterRepository;      
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