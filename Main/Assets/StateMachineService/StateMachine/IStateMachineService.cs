using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateParameterRepository;
using StateMachineService.StateNode;
using StateMachineService.Settings;

namespace StateMachineService.StateMachine
{
    public interface IStateMachineService
    {
        List<IStateNodeService> StateNodes { get; }

        IStateParameterRepository StateParameterRepository { get; }

        IStateNodeService CurrentState {get;set;}

        IStateNodeService PreviousState{get;set;}        

        void Initialize<FIRST_STATE>(IStateMachineIntializer initService) where FIRST_STATE : IStateNodeService;        

        void UpdateState<STATE_NODE>() where STATE_NODE : IStateNodeService;
    }
}