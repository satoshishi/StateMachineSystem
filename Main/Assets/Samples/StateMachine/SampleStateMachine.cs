using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateMachine;
using Sample.StateNode;

namespace Sample.StateMachine
{
    public class SampleStateMachine : PaupawsanStateMachineBase
    {
        // Start is called before the first frame update
        void Start()
        {
            Initialize<SetUpState>(GetComponent<IStateMachineParameter>());
        }
    }
}