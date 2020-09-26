using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.Param;

public class SampleParam : IMangeParameter{}

public class SampleParamManager : StateParameter
{
    public Manager Parameter
    {
        get;
        private set;
    } = null;

    private void Awake()
    {
        Parameter =  transform.gameObject.AddComponent<Manager>();
    }

    /// <summary>
    /// パラメータを管理する本体
    /// </summary>
    public class Manager : ParamManagerBase<SampleParam>
    {

    }
}
