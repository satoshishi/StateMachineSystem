using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.Locator;
using StateMachineService.StateMachine;

namespace StateMachineService.Parameter
{
    public class StateNodeParamter
    {
        public class Command
        {
            public IServiceLocator ServiceLocator;

            public IStateMachineService StateMachine;
        }

        public IServiceLocator ServiceLocator
        {
            get;
            private set;
        } = null;

        public IStateMachineService StateMachie
        {
            get;
            private set;
        } = null;

        public StateNodeParamter(Command command)
        {
            ServiceLocator = command.ServiceLocator;
            StateMachie = command.StateMachine;
        }
    }
}