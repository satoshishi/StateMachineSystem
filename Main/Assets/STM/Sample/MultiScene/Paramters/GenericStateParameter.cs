using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM.Param.Generic
{
    public class GenericStateParameter : StateParameter
    {
        public StateParamManager Instance
        {
            get;
            set;
        } = null;
    }
}