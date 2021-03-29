using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.Settings;
using StateMachineService.Locator;

namespace StateMachineService.StateMachine
{
    public interface IStateMachineService
    {
        List<IStateNodeService> StateNodes { get; }        

        IServiceLocator Services { get; }

        void Initialize<FIRST_STATE>(IStateMachineIntializer initService) where FIRST_STATE : IStateNodeService;        

        void UpdateState<STATE_NODE>() where STATE_NODE : IStateNodeService;
    }
}