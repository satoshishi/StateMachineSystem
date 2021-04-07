using System.Collections;
using System.Collections.Generic;
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
            ServiceLocator.Get<IStateMachineService>().UpdateState<Step3_CanRegisterAnyInstanceToServiceLocatorState>();
        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}
