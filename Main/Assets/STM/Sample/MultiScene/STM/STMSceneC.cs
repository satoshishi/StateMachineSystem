using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM;
using STM.Core;
using STM.Param;
using STM.DomainModel;
using UnityEngine.SceneManagement;
using STM.Param.Generic;

namespace Samples.MulScene
{

    public class STMSceneC : StateMachineBase<Samples.MulScene.SceneC>
    {
        private GenericStateParameter m_genericStateParameter;

        public void BindGenericSTM(GenericStateParameter generciStateParam)
        {
            m_genericStateParameter = generciStateParam;
            Initialize<SceneCState>(GetComponent<StateMachineDomainModel>());
        }

        public override void Initialize<FIRST_STATE>(StateMachineDomainModel domainModel)
        {
            void fnInitParamManager()
            {
                var paramManager = DomainModel.STMSettings.ParamManagers;
                List<StateParameter> paramManagerInstances = new List<StateParameter>();

                foreach (StateParameter sp in paramManager)
                {
                    var instance = Instantiate(sp.gameObject, Vector3.zero, Quaternion.identity, DomainModel.StateParameter.transform) as GameObject;
                    paramManagerInstances.Add(instance.GetComponent<StateParameter>());
                }

                paramManagerInstances.Add(this.GetComponent<StateParameter>());
                paramManagerInstances.Add(m_genericStateParameter);
                DomainModel.StateParameter.Initialize(paramManagerInstances);
            }

            void fnInitSTM()
            {
                STMCore = new StateMachineCore<SceneC>(UpdateStateNodeStatus, false);

                var stateNodes = DomainModel.STMSettings.StateNode;

                if (stateNodes.Find(target => !(target is SceneC)) != null)
                {
                    Debug.LogError("このStateMachineで管理対象でないクラスを継承したStateNodeが存在します");
                    return;
                }

                foreach (SceneC stn in stateNodes)
                {
                    var instance = Instantiate(stn.transform, Vector3.zero, Quaternion.identity, DomainModel.STNRoot);
                    StateNodes.Add(instance.gameObject);
                    STMCore.RegisterStateNode(instance.GetComponent<SceneC>(), new StateNodeCore<SceneC>(stn, STMCore));
                }

                if (TryGetStateNode<FIRST_STATE>(out FIRST_STATE first_state))
                {
                    StartCoroutine(STMCore.StartStateMachine(first_state));
                }
            }

            DomainModel = domainModel;
            fnInitParamManager();
            fnInitSTM();
        }
    }
}