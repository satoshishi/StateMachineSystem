using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace StateMachineService.Locator
{
    //https://qiita.com/ozaki_shinya/items/9eb0f827caa6a4108888
    public class ServiceLocator : MonoBehaviour, IServiceLocator
    {
        private Dictionary<Type, object> instanceDict = new Dictionary<Type, object>();

        private Dictionary<Type, Type> typeDict = new Dictionary<Type, Type>();

        public void Register(Type type, object instance)
        {
            instanceDict[type] = instance;
        }

        public void Register<T>(object instance)
        {
            instanceDict[typeof(T)] = instance;
        }

        public void Register<TContract, TConcrete>() where TContract : class
        {
            typeDict[typeof(TContract)] = typeof(TConcrete);
        }

        public T Get<T>() where T : class
        {
            T instance = default;

            Type type = typeof(T);

            if (instanceDict.ContainsKey(type))
            {
                instance = instanceDict[type] as T;
                return instance;
            }

            if (typeDict.ContainsKey(type))
            {
                instance = Activator.CreateInstance(typeDict[type]) as T;
                return instance;
            }

            if (instance == null) Debug.LogWarning($"Locator: {typeof(T).Name} not found.");
 
            return instance;
        }
    }
}