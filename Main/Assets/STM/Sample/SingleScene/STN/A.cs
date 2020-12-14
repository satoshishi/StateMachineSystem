using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM.Param;
using Samples.Parameters.Prefabs;

namespace Samples.SingleScene
{
    public partial class A : Samples.SingleScene.Sample1
    {
        public override void Initialize(Transform retentionItemsRoot, StateParamManager stateParameter)
        {
            base.Initialize(retentionItemsRoot, stateParameter);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            StateParameter.GetParameter<PrefabMaangerSample>().Instance.Create<PrefaberSample>();
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