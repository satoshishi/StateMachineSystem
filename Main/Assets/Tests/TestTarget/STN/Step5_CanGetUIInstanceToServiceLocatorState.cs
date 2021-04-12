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
            ServiceLocator.Get<IPattern2>().Print();
            ServiceLocator.Get<Pattern3>().Print();
            ServiceLocator.Get<Pattern4>().Print();
            ServiceLocator.Get<Pattern1>().Value = 1f;
        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}
