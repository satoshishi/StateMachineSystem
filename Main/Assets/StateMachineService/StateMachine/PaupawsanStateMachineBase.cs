using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateParameterRepository;
using StateMachineService.StateNode;
using StateMachineService.Settings;
using PaupawsanSTM.Core;

namespace StateMachineService.StateMachine
{
    [RequireComponent(typeof(InitializeStateMachineServices))]
    public abstract class PaupawsanStateMachineBase : MonoBehaviour, IStateMachineService
    {
        public StateMachineCore<IStateNodeService> STMCore { get { return stateMachineCore; } }
        private StateMachineCore<IStateNodeService> stateMachineCore = null;

        public List<IStateNodeService> StateNodes { get { return stateNodes; } }
        private List<IStateNodeService> stateNodes = new List<IStateNodeService>();

        public IStateParameterRepository StateParameterRepository { get { return stateParameterRepository; } }
        private IStateParameterRepository stateParameterRepository = null;

        public IStateNodeService CurrentState {get;set;} = null;

        public IStateNodeService PreviousState{get;set;} = null;

        public virtual void Initialize<FIRST_STATE>(IStateMachineIntializer initService) where FIRST_STATE : IStateNodeService
        {
            stateParameterRepository = initService.Get_StateParameterRepository();

            stateNodes = initService.Get_StateNodeServices();

            InitializeStateMachineCore<FIRST_STATE>();
        }

        public virtual void InitializeStateMachineCore<FIRST_STATE>() where FIRST_STATE : IStateNodeService
        {
            stateMachineCore = new StateMachineCore<IStateNodeService>(UpdateStateNodeStatus, false);
            stateNodes.ForEach(node => stateMachineCore.RegisterStateNode(node, new StateNodeCore<IStateNodeService>(node, stateMachineCore)));
            if (TryGetStateNode<FIRST_STATE>(out IStateNodeService first_state))
            {
                CurrentState = first_state;
                PreviousState = first_state;
                StartCoroutine(STMCore.StartStateMachine(first_state));
            }
        }

        public virtual bool TryGetStateNode<NODE_TYPE>(out IStateNodeService service) where NODE_TYPE : IStateNodeService
        {
            var res = StateNodes.Find(statenode => statenode is NODE_TYPE);

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
            var res = StateNodes.Find(statenode => statenode.GetType().Equals(stateType.GetType()));

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
                    node?.Initialize(StateParameterRepository, this);
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
            if (TryGetStateNode<NODE_TYPE>(out IStateNodeService service)){

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