using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM.Param;
using Prefab;
using Runtime;

namespace Sample
{
    public partial class FirstState : SampleSTN
    {
        public override void Initialize(StateParamManager stateParameter)
        {
            base.Initialize(stateParameter);
        }

        public override void OnEnter()
        {
            StateParameter.Collections.GetParameter<PrefabManager>().Collections.GetOrCreate<PrefabSample>();
            StateParameter.Collections.GetParameter<RuntimeManager>().Collections.Register<RuntimeSample>(
                new RuntimeSample()
                {
                    text = "hoge"
                }
            );


            Debug.Log(StateParameter.Collections.GetParameter<RuntimeManager>().Collections.GetParameter<RuntimeSample>().text);
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
}