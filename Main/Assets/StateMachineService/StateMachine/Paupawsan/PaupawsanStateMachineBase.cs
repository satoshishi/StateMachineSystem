using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.StateMachine.Parameter;
using StateMachineService.Locator;
using Paupawsan;

namespace StateMachineService.StateMachine.Paupawsan
{
    [RequireComponent(typeof(IStateMachineParameter))]
    public abstract class PaupawsanStateMachineBase : MonoBehaviour, IStateMachineService
    {
        public StateMachineCore<IStateNodeService> STMCore { get { return stateMachineCore; } }
        private StateMachineCore<IStateNodeService> stateMachineCore = null;

        private IStateMachineParameter stateMachineParameter = null;

        public IStateNodeService CurrentState { get; private set; } = null;
        public IStateNodeService PreviousState { get; private set; } = null;

        protected virtual void Initialize()
        {
            stateMachineParameter = ServiceLocator.Get<IStateMachineParameter>();
            InitializeStateMachineCore(stateMachineParameter.FirstState);
        }

        protected virtual void InitializeStateMachineCore(IStateNodeService firstState)
        {
            ServiceLocator.Register<IStateMachineService>(this);

            stateMachineCore = new StateMachineCore<IStateNodeService>(UpdateStateNodeStatus, false);
            stateMachineParameter.StateNodes.ForEach(node => stateMachineCore.RegisterStateNode(node, new StateNodeCore<IStateNodeService>(node, stateMachineCore)));
            if (TryGetStateNode(firstState, out IStateNodeService first_state))
            {
                CurrentState = first_state;
                PreviousState = first_state;
                StartCoroutine(STMCore.StartStateMachine(first_state));
            }
        }

        protected virtual bool TryGetStateNode<NODE_TYPE>(out IStateNodeService service) where NODE_TYPE : IStateNodeService
        {
            var res = stateMachineParameter.StateNodes.Find(statenode => statenode is NODE_TYPE);

            if (res != null)
            {
                service = ((NODE_TYPE)(res));
                return true;
            }

            service = default;
            return false;
        }

        protected virtual bool TryGetStateNode(IStateNodeService stateType, out IStateNodeService service)
        {
            var res = stateMachineParameter.StateNodes.Find(statenode => statenode.GetType().Equals(stateType.GetType()));

            if (res != null)
            {
                service = res;
                return true;
            }

            service = default;
            return false;
        }

        protected virtual IEnumerator UpdateStateNodeStatus(IStateNodeService stateType, eStateNodeStatus stateStatus)
        {
            if (!TryGetStateNode(stateType, out IStateNodeService node))
                yield break;

            switch (stateStatus)
            {
                case eStateNodeStatus.StateInitialize:
                    Debug.Log($"{node.GetType().BaseType}-{node.GetType()}-{stateStatus}");
                    node?.Initialize();
                    break;

                case eStateNodeStatus.StateEnter:

                    Debug.Log($"{node.GetType().BaseType}-{node.GetType()}-{stateStatus}");
                    node?.OnEnter(PreviousState);
                    break;

                case eStateNodeStatus.StateUpdate:
                    //  Debug.Log($"{stateType.GetType().BaseType}-{stateType.GetType()}-{stateStatus}");       
                    //node?.OnUpdate();
                    break;

                case eStateNodeStatus.StateExit:
                    Debug.Log($"{node.GetType().BaseType}-{node.GetType()}-{stateStatus}");
                    node?.OnExit(CurrentState);
                    break;

                case eStateNodeStatus.StateFinalize:
                    break;
            }

            yield return null;
        }

        public virtual void UpdateState<NODE_TYPE>() where NODE_TYPE : IStateNodeService
        {
            if (TryGetStateNode<NODE_TYPE>(out IStateNodeService service))
            {

                PreviousState = CurrentState;
                CurrentState = service;

                STMCore.MoveState(service);
            }
        }

        public virtual void ShutdownSTM()
        {
            STMCore.Shutdown();
        }
    }
}