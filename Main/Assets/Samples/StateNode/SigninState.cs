using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.StateMachine;
using StateMachineService.Locator;
using StateMachineService.Parameter;

namespace Sample.StateNode
{
    public class SignInState : MonoBehaviour, IStateNodeService
    {
        public StateNodeParamter Parameter{get{return parameter;}}
        private StateNodeParamter parameter = null;

        public void Initialize(StateNodeParamter _paramter)
        {
            this.parameter = _paramter;
        }

        public void OnEnter(IStateNodeService from)
        {

        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}
