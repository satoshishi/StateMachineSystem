using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateMachine.Paupawsan;
using Test.StateNode;

namespace Test.StateMachine
{
    public class TestStateMachine : PaupawsanStateMachineBase
    {
        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }
    }
}