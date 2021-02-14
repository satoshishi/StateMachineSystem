using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM.Param;

namespace Sample
{
    public partial class SampleState : Sample.SampleNode
    {
        public override void Initialize(Transform retentionItemsRoot, StateParamManager stateParameter)
        {
            base.Initialize(retentionItemsRoot, stateParameter);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            StateParameter.GetParameter<SampleManager>().Instance.Create<SampleParameterCube>();

            /*InitializeController(
                new ParameterData()
                {

                });
*/
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