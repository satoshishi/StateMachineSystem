using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.Locator;
using Paupawsan;

namespace StateMachineService.StateMachine.Paupawsan
{
    public class PaupawsanStateMachine : MonoBehaviour, IStateMachineService
    {
        public StateMachineCore<IStateNodeService> STMCore { get { return stateMachineCore; } }
        private StateMachineCore<IStateNodeService> stateMachineCore = null;

        private IStateNodeList stateNodeList = null;

        public IStateNodeService CurrentState { get; private set; } = null;
        public IStateNodeService PreviousState { get; private set; } = null;

        public virtual void Initialize(GameObject obj)
        {
            stateNodeList = ServiceLocator.Get<IStateNodeList>();
            InitializeStateMachineCore();
        }

        protected virtual void InitializeStateMachineCore()
        {
            ServiceLocator.Register<IStateMachineService>(this);

            stateMachineCore = new StateMachineCore<IStateNodeService>(UpdateStateNodeStatus, false);
            stateNodeList.StateNodes.ForEach(node => stateMachineCore.RegisterStateNode(node, new StateNodeCore<IStateNodeService>(node, stateMachineCore)));

            var stateNode = stateNodeList.GetStateNode(stateNodeList.FirstState.GetType());
            if (stateNode != null)
            {
                CurrentState = stateNode;
                PreviousState = stateNode;
                StartCoroutine(STMCore.StartStateMachine(stateNode));
            }
        }

        protected virtual IEnumerator UpdateStateNodeStatus(IStateNodeService stateType, eStateNodeStatus stateStatus)
        {
            var stateNode = stateNodeList.GetStateNode(stateType.GetType());
            if (stateNode == null)
                yield break;

            yield return null;                

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
        }

        public virtual void UpdateState<NODE_TYPE>() where NODE_TYPE : IStateNodeService
        {
            var stateNode = stateNodeList.GetStateNode<NODE_TYPE>();

            if (stateNode != null)
            {
                bool isSuccess = STMCore.TryMoveState(stateNode);                

                if(isSuccess)
                {
                    PreviousState = CurrentState;
                    CurrentState = stateNode;
                }
            }
        }

        public virtual void UpdateState(Type type)
        {
            var stateNode = stateNodeList.GetStateNode(type);

            if (stateNode != null)
            {
                bool isSuccess = STMCore.TryMoveState(stateNode);                

                if(isSuccess)
                {
                    PreviousState = CurrentState;
                    CurrentState = stateNode;
                }
            }
        }        

        public virtual void ShutdownSTM()
        {
            STMCore.Shutdown();
        }
    }
}