using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.StateMachine;
using StateMachineService.Locator;

namespace Tests.StateNode
{
    public class Step3_CanRegistToServiceLocatorState : MonoBehaviour, IStateNodeService
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

        public interface ITestParameter
        {
            string Get();
        }

        public class TestParameter : ITestParameter
        {
            public string Get()
            {
                return "Parameter";
            }
        }

        public void OnEnter(IStateNodeService from)
        {
            ServiceLocator.Register<ITestParameter>(new TestParameter());
        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}
