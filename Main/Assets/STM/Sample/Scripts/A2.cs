using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM.Param;

public class A2 : A
{
    public override void Initialize(Transform retentionItemsRoot, StateParamManager stateParameter)
    {
        base.Initialize(retentionItemsRoot, stateParameter);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log(StateParameter.GetParameter<SampleParamManager>() // parameterのmanagerをとって
            .Parameter.GetParameter<A1Parameter>().text); // そこから登録したパラメータ取得   
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
