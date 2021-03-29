using System.Collections;
using System.Collections.Generic;
using StateMachineService.StateNode;
using StateMachineService.Locator;

namespace StateMachineService.Parameter
{
    public interface IStateMachineParameter
    {
        IServiceLocator ServiceLocator{get;}

        List<IStateNodeService> StateNodes{get;}

        void Initialize();
    }
}