using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.Locator;
using PaupawsanSTM.Core;

namespace StateMachineService.StateMachine
{
    [RequireComponent(typeof(IStateMachineParameter))]
    public abstract class PaupawsanStateMachineBase : MonoBehaviour, IStateMachineService
    {
        public StateMachineCore<IStateNodeService> STMCore { get { return stateMachineCore; } }
        private StateMachineCore<IStateNodeService> stateMachineCore = null;

        public IStateMachineParameter StateMachineParameter { get { return stateMachineParameter; } }
        private IStateMachineParameter stateMachineParameter = null;

        public IStateNodeService CurrentState { get; private set; } = null;

        public IStateNodeService PreviousState { get; private set; } = null;

        public virtual void Initialize(IStateMachineParameter stmParamter)
        {
            stmParamter.Initialize();
            stateMachineParameter = stmParamter;

            InitializeStateMachineCore(stateMachineParameter.FirstState);
        }

        public virtual void InitializeStateMachineCore(IStateNodeService firstState)
        {
            stateMachineCore = new StateMachineCore<IStateNodeService>(UpdateStateNodeStatus, false);
            stateMachineParameter.StateNodes.ForEach(node => stateMachineCore.RegisterStateNode(node, new StateNodeCore<IStateNodeService>(node, stateMachineCore)));
            if (TryGetStateNode(firstState, out IStateNodeService first_state))
            {
                CurrentState = first_state;
                PreviousState = first_state;
                StartCoroutine(STMCore.StartStateMachine(first_state));
            }
        }

        public virtual bool TryGetStateNode<NODE_TYPE>(out IStateNodeService service) where NODE_TYPE : IStateNodeService
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

        public virtual bool TryGetStateNode(IStateNodeService stateType, out IStateNodeService service)
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

        public virtual IEnumerator UpdateStateNodeStatus(IStateNodeService stateType, eStateNodeStatus stateStatus)
        {
            if (!TryGetStateNode(stateType, out IStateNodeService node))
                yield break;

            switch (stateStatus)
            {
                case eStateNodeStatus.StateInitialize:
                    Debug.Log($"{node.GetType().BaseType}-{node.GetType()}-{stateStatus}");
                    node?.Initialize(this);
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