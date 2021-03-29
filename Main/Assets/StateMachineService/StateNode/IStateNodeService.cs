using StateMachineService.StateMachine;
using StateMachineService.Locator;

namespace StateMachineService.StateNode
{
    public interface IStateNodeService
    {
        IServiceLocator Services{get;}

        void Initialize(IServiceLocator services);

        void OnEnter(IStateNodeService from);

        void OnExit(IStateNodeService to);
    }
}