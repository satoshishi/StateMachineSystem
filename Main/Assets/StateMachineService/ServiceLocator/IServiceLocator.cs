using System;

namespace StateMachineService.Locator
{
    public interface IServiceLocator
    {
        void Register(Type type, object instance);

        void Register<T>(object instance);        

        void Register<TContract, TConcrete>() where TContract : class;

        T Get<T>() where T : class;        
    }
}