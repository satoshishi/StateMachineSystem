using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateParameterRepository;
using StateMachineService.StateNode;

public interface IStateMachineService
{
    IStateParameterRepository StateParameterRepository { get; }

    void UpdateState<STATE_NODE>() where STATE_NODE : IStateNodeService;
}
