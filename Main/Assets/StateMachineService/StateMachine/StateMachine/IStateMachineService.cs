using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using StateMachineService.StateNode;

namespace StateMachineService.StateMachine
{
    public interface IStateMachineService
    {     
        void UpdateState<STATE_NODE>() where STATE_NODE : IStateNodeService;

        void UpdateState(Type type);
    }
}