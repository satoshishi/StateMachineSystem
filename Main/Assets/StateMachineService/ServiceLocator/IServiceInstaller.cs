using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace StateMachineService.Locator
{

    public interface IPrefabServiceInstaller
    {
        KeyValuePair<Type,object> Install();
    }
}