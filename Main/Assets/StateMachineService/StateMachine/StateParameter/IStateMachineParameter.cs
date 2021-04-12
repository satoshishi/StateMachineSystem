using System.Collections;
using System;
using System.Collections.Generic;
using StateMachineService.StateNode;
using StateMachineService.Locator;

namespace StateMachineService.StateMachine.Parameter
{
    public interface IStateMachineParameter
    {
        List<IStateNodeService> StateNodes{get;}

        IStateNodeService GetStateNode<STATE_NODE>() where STATE_NODE : IStateNodeService;

        IStateNodeService GetStateNode(Type stateType);

        IStateNodeService FirstState{get;}

        void Initialize();
    }
}