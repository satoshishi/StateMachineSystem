using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.Param;

namespace Samples.Parameters.Model
{

    public class ModelSample : IMangeParameter{}

    public class ModelManagerSample : StateParameter
    {
        public Manager Instance
        {
            get;
            private set;
        } = null;

        private void Awake()
        {
            Instance =  transform.gameObject.AddComponent<Manager>();
        }

        public class Manager : ParamManagerBase<ModelSample>
        {

        }
    }
}