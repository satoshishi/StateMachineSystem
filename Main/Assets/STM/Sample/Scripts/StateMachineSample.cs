using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM;
using STM.DomainModel;

/// <summary>
/// state machineで管理するクラスを定義する
/// </summary>
public class A : StateNodeBase { }
public class B : StateNodeBase { }

/// <summary>
/// StateMachineBaseにgenericで持たせるclassを管理するstate nodeとする
/// </summary>
public class StateMachineSample : StateMachineBase<A>
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize<A1>(GetComponent<StateMachineDomainModel>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}