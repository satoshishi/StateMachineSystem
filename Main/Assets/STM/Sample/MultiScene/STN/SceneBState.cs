using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM.Param;

namespace Samples.MulScene
{
    public partial class SceneBState : Samples.MulScene.SceneB
    {
        public override void Initialize(Transform retentionItemsRoot, StateParamManager stateParameter)
        {
            base.Initialize(retentionItemsRoot, stateParameter);
        }

        public Transform a;

        public Transform b;

        public Vector3 pos;

        public override void OnEnter()
        {
            base.OnEnter();
            
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