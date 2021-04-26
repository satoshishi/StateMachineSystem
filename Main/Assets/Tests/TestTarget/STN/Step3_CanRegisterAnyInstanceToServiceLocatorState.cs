using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.StateMachine;
using StateMachineService.Locator;

namespace Test.StateNode
{
    public class Step3_CanRegisterAnyInstanceToServiceLocatorState : MonoBehaviour, IStateNodeService
    {
        public bool IsTransition = false;

        public void Initialize()
        {

        }

        public void OnEnter(IStateNodeService from)
        {
            IsTransition = true;
        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}
