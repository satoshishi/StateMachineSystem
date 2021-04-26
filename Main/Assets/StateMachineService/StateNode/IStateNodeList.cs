using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace StateMachineService.StateNode
{
    public interface IStateNodeList
    {
        List<IStateNodeService> StateNodes { get; }

        IStateNodeService GetStateNode<STATE_NODE>() where STATE_NODE : IStateNodeService;

        IStateNodeService GetStateNode(Type stateType);

        IStateNodeService FirstState { get; }

        void Initialize(GameObject obj);
    }
}