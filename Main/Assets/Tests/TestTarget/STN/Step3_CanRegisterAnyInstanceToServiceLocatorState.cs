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

        public interface IAnyParameter
        {
            
        }
        public class AnyParameter : IAnyParameter
        {

        }

        public void OnEnter(IStateNodeService from)
        {
            IsTransition = true;
            ServiceLocator.Register<IAnyParameter>(new AnyParameter());
        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}
