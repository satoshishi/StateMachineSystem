using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.StateMachine.Parameter;

namespace StateMachineService.StateMachine
{
    public interface IStateMachineService
    {     
        void UpdateState<STATE_NODE>() where STATE_NODE : IStateNodeService;
    }
}