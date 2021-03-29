using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateMachine;
using StateMachineService.Locator;

namespace StateMachineService.StateNode
{
    public interface IStateNodeService
    {
        IServiceLocator Services { get; }

        IStateMachineService StateMachine{get;}

        void Initialize(IServiceLocator services , IStateMachineService stateMachine);

        void OnEnter(IStateNodeService from);

        void OnExit(IStateNodeService to);
    }
}