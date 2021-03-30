using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.Locator;
using System;

[RequireComponent(typeof(PefabRepository))]
public class PefabRepositoryInstaller : MonoBehaviour,IPrefabServiceInstaller
{
    public KeyValuePair<Type,object> Install()
    {
        return new KeyValuePair<Type, object>(
            typeof(PefabRepository),
            GetComponent<PefabRepository>()
        );
    }
}
