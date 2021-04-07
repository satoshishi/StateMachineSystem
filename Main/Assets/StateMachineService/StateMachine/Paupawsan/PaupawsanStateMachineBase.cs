using System;
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

            var stateNode = stateMachineParameter.GetStateNode(firstState.GetType());
            if (stateNode != null)
            {
                CurrentState = stateNode;
                PreviousState = stateNode;
                StartCoroutine(STMCore.StartStateMachine(stateNode));
            }
        }

        protected virtual IEnumerator UpdateStateNodeStatus(IStateNodeService stateType, eStateNodeStatus stateStatus)
        {
            var stateNode = stateMachineParameter.GetStateNode(stateType.GetType());
            if (stateNode == null)
                yield break;

            switch (stateStatus)
            {
                case eStateNodeStatus.StateInitialize:
                    Debug.Log($"{stateNode.GetType().BaseType}-{stateNode.GetType()}-{stateStatus}");
                    stateNode?.Initialize();
                    break;

                case eStateNodeStatus.StateEnter:

                    Debug.Log($"{stateNode.GetType().BaseType}-{stateNode.GetType()}-{stateStatus}");
                    stateNode?.OnEnter(PreviousState);
                    break;

                case eStateNodeStatus.StateUpdate:
                    //  Debug.Log($"{stateType.GetType().BaseType}-{stateType.GetType()}-{stateStatus}");       
                    //node?.OnUpdate();
                    break;

                case eStateNodeStatus.StateExit:
                    Debug.Log($"{stateNode.GetType().BaseType}-{stateNode.GetType()}-{stateStatus}");
                    stateNode?.OnExit(CurrentState);
                    break;

                case eStateNodeStatus.StateFinalize:
                    break;
            }

            yield return null;
        }

        public virtual void UpdateState<NODE_TYPE>() where NODE_TYPE : IStateNodeService
        {
            var stateNode = stateMachineParameter.GetStateNode<NODE_TYPE>();

            if (stateNode != null)
            {

                PreviousState = CurrentState;
                CurrentState = stateNode;

                STMCore.MoveState(stateNode);
            }
        }

        public virtual void UpdateState(Type type)
        {
            var stateNode = stateMachineParameter.GetStateNode(type);

            if (stateNode != null)
            {

                PreviousState = CurrentState;
                CurrentState = stateNode;

                STMCore.MoveState(stateNode);
            }
        }        

        public virtual void ShutdownSTM()
        {
            STMCore.Shutdown();
        }
    }
}