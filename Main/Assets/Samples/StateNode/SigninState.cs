using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.StateMachine;
using StateMachineService.StateParameterRepository;

namespace Sample.StateNode
{
    public class SigninState : MonoBehaviour, IStateNodeService
    {
        public IStateParameterRepository StateParameterRepository
        {
            get { return stateParameterRepository; }
        }
        private IStateParameterRepository stateParameterRepository = null;

        public IStateMachineService StateMachineService
        {
            get { return stateMachineService; }
        }
        public IStateMachineService stateMachineService = null;

        public void Initialize(IStateParameterRepository stateParameterRepository, IStateMachineService stateMachineService)
        {
            this.stateParameterRepository = stateParameterRepository;
            this.stateMachineService = stateMachineService;
        }

        public void OnEnter(IStateNodeService from)
        {

        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}
