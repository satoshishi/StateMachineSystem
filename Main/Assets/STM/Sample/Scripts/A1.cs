using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM.Param;

public class A1Parameter : SampleParam
{
    public string text;
}

public class A1 : A
{
    public override void Initialize(Transform retentionItemsRoot, StateParamManager stateParameter)
    {
        base.Initialize(retentionItemsRoot, stateParameter);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        A1Parameter test = new A1Parameter(){ text = this.transform.parent.name };

        StateParameter.GetParameter<SampleParamManager>() // parameterのmanagerをとって
            .Parameter.Register<A1Parameter>(test); // そこに対応したパラメータを登録
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(Input.GetKeyUp(KeyCode.A))
            StateParameter.GetParameter<StateMachineSample>().UpdateState<A2>();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
