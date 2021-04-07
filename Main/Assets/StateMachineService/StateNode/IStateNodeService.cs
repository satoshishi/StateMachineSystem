using StateMachineService.Locator;

namespace StateMachineService.StateNode
{
    public interface IStateNodeService
    {
        void Initialize();

        void OnEnter(IStateNodeService from);

        void OnExit(IStateNodeService to);
    }
}
