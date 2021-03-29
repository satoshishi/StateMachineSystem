using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.StateMachine;
using StateMachineService.Locator;

namespace Sample.StateNode
{
    public class MainState : MonoBehaviour, IStateNodeService
    {
        public IServiceLocator Services
        {
            get { return services; }
        }
        private IServiceLocator services = null;

        public IStateMachineService StateMachine
        {
            get { return stateMachine; }
        }
        public IStateMachineService stateMachine = null;

        public void Initialize(IServiceLocator services, IStateMachineService stateMachine)
        {
            this.services = services;
            this.stateMachine = stateMachine;
        }

        public void OnEnter(IStateNodeService from)
        {

        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}
