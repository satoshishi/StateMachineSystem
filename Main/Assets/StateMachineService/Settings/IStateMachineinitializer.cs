using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateParameterRepository;
using StateMachineService.StateNode;

namespace StateMachineService.Settings
{
    public interface IStateMachineIntializer
    {
        IStateParameterRepository Get_StateParameterRepository();

        List<IStateNodeService> Get_StateNodeServices();
    }
}