using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.Locator;

namespace StateMachineService.StateMachine
{
    public interface IStateMachineService
    {
        IStateMachineParameter StateMachineParameter{get;}

        void Initialize(IStateMachineParameter stmParamter);        
        
        void UpdateState<STATE_NODE>() where STATE_NODE : IStateNodeService;
    }
}