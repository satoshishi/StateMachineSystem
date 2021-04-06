using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.StateMachine;
using StateMachineService.Locator;
using UnityEngine.Assertions;

namespace Tests.StateNode
{
    public class Step5_CanGetPrefabParamter_FromServiceLocatorState : MonoBehaviour, IStateNodeService
    {
        public IStateMachineService StateMachine{get;}
        public IStateMachineService stateMachine = null;

        public void Initialize(IStateMachineService stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public void Initialize()
        {

        }

        public void OnEnter(IStateNodeService from)
        {
            Assert.IsNotNull(ServiceLocator.Get<AnyService>());
        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}
