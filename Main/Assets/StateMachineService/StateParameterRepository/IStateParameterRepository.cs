using System.Collections;
using System.Collections.Generic;
using StateMachineService.StateParameter;

namespace StateMachineService.StateParameterRepository
{
    public interface IStateParameterRepository
    {
        List<IStateParameter> Parameters{get;}

        STATE_PARAMETER Get<STATE_PARAMETER>();
    }
}