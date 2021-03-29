using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.Locator;
using System;

[RequireComponent(typeof(DummyPrefabRepository))]
public class DummyPrefabRepositoryInstaller : MonoBehaviour,IPrefabServiceInstaller
{
    public KeyValuePair<Type,object> Install()
    {
        return new KeyValuePair<Type, object>(
            typeof(PrefabRepositroy),
            GetComponent<PrefabRepositroy>()
        );
    }
}
