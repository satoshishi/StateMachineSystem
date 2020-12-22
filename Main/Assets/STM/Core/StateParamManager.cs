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

        public override PARAMETER GetParameter<PARAMETER>()
        {
            var parameter_typeof = (PARAMETER) Parameters.Find(param => typeof(PARAMETER) == param.GetType());
            var parameter_is = (PARAMETER) Parameters.Find(param => param is PARAMETER);

            return parameter_typeof != null ? parameter_typeof : parameter_is;
        }
    }
    
    public class StateParameter : MonoBehaviour,IMangeParameter
    {

    }
}