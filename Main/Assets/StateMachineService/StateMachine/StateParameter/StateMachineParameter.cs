using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.Locator;

namespace StateMachineService.StateMachine.Parameter
{
    public class StateMachineParameter : MonoBehaviour,IStateMachineParameter
    {
        [System.Serializable]
        public class Command
        {
            public Transform stateNodeRoot;
            public Transform prefabRoot;
            public StateMachineParameterSettings serviceSettings;         
        }

        [SerializeField]
        private Command command;
        public Command GetCommand() => command;

        public IStateNodeService FirstState { get { return firstState; } }
        private IStateNodeService firstState = null;

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

        public void Initialize()
        {
            stateNodes = Get_StateNodeServices(command);
            firstState = Get_FirstStateNodeServices(command);
            InstantiateAndRegistServiceLocator_FromPrefab(command);
            RegistServiceLocator_NotPrefab(command); 

            ServiceLocator.Register<IStateMachineParameter>(this);
        }

        protected virtual void RegistServiceLocator_NotPrefab(Command command)
        {

        }

        protected virtual void InstantiateAndRegistServiceLocator_FromPrefab(Command command)
        {
            foreach (GameObject target in command.serviceSettings.StateParametersGameObject)
            {
                var registTarget = GameObject.Instantiate(target, command.prefabRoot.transform);
                var targetScripts = registTarget.GetComponents<MonoBehaviour>().Select(mono => mono.GetType());
                var attributes = targetScripts
                        .Where(scirpt => scirpt.GetCustomAttributes(typeof(AutoRegistOnPrefabScriptAttribute), false).Any())
                        .Select(scirpt => (scirpt.GetCustomAttributes(typeof(AutoRegistOnPrefabScriptAttribute), false).OfType<AutoRegistOnPrefabScriptAttribute>().First()));

                attributes.ToList().ForEach(
                    attribute =>
                    {
                        var targetType = attribute.RegistTargetType;
                        ServiceLocator.Register(targetType, registTarget.GetComponent(targetType));
                    });
            }
        }

        protected virtual List<IStateNodeService> Get_StateNodeServices(Command command)
        {
            var stateNodes = new List<IStateNodeService>();
            foreach (GameObject target in command.serviceSettings.StateNodesGameObject)
            {
                var nodeObject = GameObject.Instantiate(target, command.stateNodeRoot);
                var nodeInstance = nodeObject.GetComponent<IStateNodeService>();

                if (nodeInstance == null)
                    Debug.LogError($"{target.name} is not attach IStateNodeService");

                stateNodes.Add(nodeInstance);
            }

            return stateNodes;
        }

        protected virtual IStateNodeService Get_FirstStateNodeServices(Command command)
        {
            var _firstState = command.serviceSettings.FirstStateNodeGameObject.GetComponent<IStateNodeService>();
            if (_firstState == null)
                Debug.LogError($"{command.serviceSettings.FirstStateNodeGameObject.name} is not attach IStateNodeService");
            var targetState = StateNodes.Find(node => node.GetType() == _firstState.GetType());

            return targetState;
        }
    }
}