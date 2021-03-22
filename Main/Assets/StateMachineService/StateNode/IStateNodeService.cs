using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateParameterRepository;

namespace StateMachineService.StateNode
{
    public interface IStateNodeService
    {
        IStateParameterRepository StateParameterRepository{get;}

        void Initialize(IStateParameterRepository stateParameterRepository);

        void OnEnter();

        void OnExit();
    }
}