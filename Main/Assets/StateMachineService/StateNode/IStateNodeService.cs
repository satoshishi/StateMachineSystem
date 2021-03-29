using StateMachineService.StateMachine;
using StateMachineService.Locator;
using StateMachineService.Parameter;

namespace StateMachineService.StateNode
{
    public interface IStateNodeService
    {
        StateNodeParamter Parameter{get;}

        void Initialize(StateNodeParamter _paramter);

        void OnEnter(IStateNodeService from);

        void OnExit(IStateNodeService to);
    }
}