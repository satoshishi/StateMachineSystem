using System.Collections;
using System.Collections.Generic;
using StateMachineService.StateNode;
using StateMachineService.Locator;

namespace StateMachineService.Settings
{
    public interface IStateMachineIntializer
    {
        IServiceLocator Get_ServiceLocator();

        List<IStateNodeService> Get_StateNodeServices();
    }
}