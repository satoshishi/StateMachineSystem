using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.Locator;
using System;

[RequireComponent(typeof(PrefabRepository))]
public class PrefabRepositoryInstaller : MonoBehaviour,IPrefabServiceInstaller
{
    public KeyValuePair<Type,object> Install()
    {
        return new KeyValuePair<Type, object>(
            typeof(PrefabRepository),
            GetComponent<PrefabRepository>()
        );
    }
}
