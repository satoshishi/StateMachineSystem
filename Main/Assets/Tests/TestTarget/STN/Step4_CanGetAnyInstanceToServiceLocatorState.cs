using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.StateMachine;
using StateMachineService.Locator;

namespace Test
{

    public interface IAnyParameter
    {

    }
    public class AnyParameter : IAnyParameter
    {

    }
}

namespace Test.StateNode
{
    public class Step4_CanGetAnyInstanceToServiceLocatorState : MonoBehaviour, IStateNodeService
    {
        public IAnyParameter anyParameter;

        public void Initialize()
        {

        }

        public void OnEnter(IStateNodeService from)
        {
            anyParameter = ServiceLocator.Get<IAnyParameter>();
        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}
