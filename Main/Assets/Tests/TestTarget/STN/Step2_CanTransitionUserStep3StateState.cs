using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.StateMachine;
using StateMachineService.Locator;

namespace Test.StateNode
{
    public class Step2_CanTransitionUserStep3StateState : MonoBehaviour, IStateNodeService
    {
        public void Initialize()
        {

        }

        public void OnEnter(IStateNodeService from)
        {
            var type = typeof(Step3_CanRegisterAnyInstanceToServiceLocatorState);
            ServiceLocator.Get<IStateMachineService>().UpdateState(type);
        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}
