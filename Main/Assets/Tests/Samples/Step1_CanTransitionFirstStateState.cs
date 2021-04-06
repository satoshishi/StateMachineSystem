using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateNode;
using StateMachineService.StateMachine;
using StateMachineService.Locator;

namespace Tests.StateNode
{
    public class Step1_CanTransitionFirstStateState : MonoBehaviour, IStateNodeService
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

        public void TransitionTest()
        {
            stateMachine.UpdateState<Step2_CanTransitionNextStateState>();
        }

        public void OnEnter(IStateNodeService from)
        {

        }

        public void OnExit(IStateNodeService to)
        {

        }
    }
}
