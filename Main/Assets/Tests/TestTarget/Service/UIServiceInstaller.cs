using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StateMachineService.Locator;

[RequireComponent(typeof(UIService))]
public class UIServiceInstaller : MonoBehaviour,IPrefabServiceInstaller
{
    public KeyValuePair<Type,object> Install()
    {
        return new KeyValuePair<Type, object>(
            typeof(UIService),
            GetComponent<UIService>()
        );
    }
}
