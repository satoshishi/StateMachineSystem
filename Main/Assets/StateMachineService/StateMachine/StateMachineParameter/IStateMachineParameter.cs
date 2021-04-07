using System.Collections;
using System.Collections.Generic;
using StateMachineService.StateNode;
using StateMachineService.Locator;

namespace StateMachineService.StateMachine.Parameter
{
    public interface IStateMachineParameter
    {
        List<IStateNodeService> StateNodes{get;}

        IStateNodeService FirstState{get;}
    }
}