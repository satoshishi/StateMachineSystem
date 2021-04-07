using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.StateMachine;
using StateMachineService.Locator;

namespace Test.StateNode
{
    public class Step5_CanGetUIInstanceToServiceLocatorState : MonoBehaviour, IStateNodeService
    {
        public void Initialize()
        {

        }

        public void OnEnter(IStateNodeService from)
        {
            ServiceLocator.Get<UIService>().Value = 1f;
        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}
