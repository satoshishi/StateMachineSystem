using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateParameterRepository;
using StateMachineService.StateMachine;

namespace StateMachineService.StateNode
{
    public interface IStateNodeService
    {
        IStateParameterRepository StateParameterRepository{get;}

        IStateMachineService StateMachineService{get;}

        void Initialize(IStateParameterRepository stateParameterRepository, IStateMachineService stateMachineService);

        void OnEnter(IStateNodeService from);

        void OnExit(IStateNodeService to);
    }
}