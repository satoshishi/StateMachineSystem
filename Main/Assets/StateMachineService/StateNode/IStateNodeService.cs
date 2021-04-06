using StateMachineService.StateMachine;
using StateMachineService.Locator;

namespace StateMachineService.StateNode
{
    public interface IStateNodeService
    {
        IStateMachineService StateMachine{get;}

        void Initialize(IStateMachineService stateMachine);

        void OnEnter(IStateNodeService from);

        void OnExit(IStateNodeService to);
    }
}
