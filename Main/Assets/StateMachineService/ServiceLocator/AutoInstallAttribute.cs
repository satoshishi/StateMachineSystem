using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineService.Locator
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AutoInstallAttribute : Attribute
    {
        public Type RegistTargetType;

        public AutoInstallAttribute(Type registTargetType)
        {
            RegistTargetType = registTargetType;
        }
    }
}