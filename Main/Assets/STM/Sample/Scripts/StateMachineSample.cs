using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM;

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
        Initialize<A1>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
            UpdateState<A1>();
        if (Input.GetKeyUp(KeyCode.B))
            UpdateState<A2>();
    }
}