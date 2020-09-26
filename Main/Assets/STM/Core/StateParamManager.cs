using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace STM.Param
{
    /// <summary>
    /// StateParameterを継承するParameterManagerを管理する
    /// </summary>
    public class StateParamManager : ParamManagerBase<StateParameter>
    {
        public override void Initialize(List<StateParameter> parameters)
        {
            Parameters = new List<StateParameter>(parameters);
        }
    }
    
    public class StateParameter : MonoBehaviour,IMangeParameter
    {

    }
}