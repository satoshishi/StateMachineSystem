using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineService.Locator
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AutoRegistOnPrefabScriptAttribute : Attribute
    {
        public Type RegistTargetType;

        public AutoRegistOnPrefabScriptAttribute(Type registTargetType)
        {
            RegistTargetType = registTargetType;
        }
    }
}