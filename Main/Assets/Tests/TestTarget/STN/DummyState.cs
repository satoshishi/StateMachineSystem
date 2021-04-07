using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.StateMachine;
using StateMachineService.Locator;

namespace Test.StateNode
{
    public class DummyState : MonoBehaviour,IStateNodeService
    {
        public void Initialize()
        {

        }

        public void OnEnter(IStateNodeService from)
        {
            
        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}