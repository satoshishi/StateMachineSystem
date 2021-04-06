using System.Collections;
using System.Collections.Generic;
using StateMachineService.StateNode;
using StateMachineService.Locator;

namespace StateMachineService.StateMachine
{
    public interface IStateMachineParameter
    {
        List<IStateNodeService> StateNodes{get;}

        IStateNodeService FirstState{get;}

        void Initialize();
    }
}