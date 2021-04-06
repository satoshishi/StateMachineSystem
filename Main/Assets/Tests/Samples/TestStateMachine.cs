using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateMachine;
using Tests.StateNode;

namespace Tests.StateMachine
{
    public class TestStateMachine : PaupawsanStateMachineBase
    {
        // Start is called before the first frame update
        void Start()
        {
            Initialize(GetComponent<IStateMachineParameter>());
        }
    }
}