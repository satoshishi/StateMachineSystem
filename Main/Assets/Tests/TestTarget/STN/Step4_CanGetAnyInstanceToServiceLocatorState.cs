using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.StateMachine;
using StateMachineService.Locator;

namespace Test.StateNode
{
    public class Step4_CanGetAnyInstanceToServiceLocatorState : MonoBehaviour, IStateNodeService
    {
        public Step3_CanRegisterAnyInstanceToServiceLocatorState.IAnyParameter anyParameter;

        public void Initialize()
        {

        }

        public void OnEnter(IStateNodeService from)
        {
            anyParameter = ServiceLocator.Get<Step3_CanRegisterAnyInstanceToServiceLocatorState.IAnyParameter>();
        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}
