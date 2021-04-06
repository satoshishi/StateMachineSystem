using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.Locator;
using System;

[RequireComponent(typeof(AnyService))]
public class AnyServiceInstaller : MonoBehaviour,IPrefabServiceInstaller
{
    public KeyValuePair<Type,object> Install()
    {
        return new KeyValuePair<Type, object>(
            typeof(AnyService),
            GetComponent<AnyService>()
        );
    }
}
