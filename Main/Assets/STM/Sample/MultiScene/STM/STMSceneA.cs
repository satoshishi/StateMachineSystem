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

    public class STMSceneA : StateMachineBase<Samples.MulScene.SceneA>
    {
        [SerializeField]
        private GenericStateParameter m_genericStateParameter;

        // Start is called before the first frame update
        void Start()
        {
            Initialize<SceneAState>(GetComponent<StateMachineDomainModel>());
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
                DomainModel.StateParameter.Initialize(paramManagerInstances);
            }

            void fnInitSTM()
            {
                STMCore = new StateMachineCore<SceneA>(UpdateStateNodeStatus, false);

                var stateNodes = DomainModel.STMSettings.StateNode;

                if (stateNodes.Find(target => !(target is SceneA)) != null)
                {
                    Debug.LogError("このStateMachineで管理対象でないクラスを継承したStateNodeが存在します");
                    return;
                }

                foreach (SceneA stn in stateNodes)
                {
                    var instance = Instantiate(stn.transform, Vector3.zero, Quaternion.identity, DomainModel.STNRoot);
                    StateNodes.Add(instance.gameObject);
                    STMCore.RegisterStateNode(instance.GetComponent<SceneA>(), new StateNodeCore<SceneA>(stn, STMCore));
                }

                if (TryGetStateNode<FIRST_STATE>(out FIRST_STATE first_state))
                {
                    StartCoroutine(STMCore.StartStateMachine(first_state));
                }
            }

            IEnumerator fnInitializeEvent()
            {
                DomainModel = domainModel;
                fnInitParamManager();

                m_genericStateParameter.Instance = DomainModel.StateParameter;

                yield return SceneManager.LoadSceneAsync("SceneB", LoadSceneMode.Additive);
                var stmB = GameObject.Find("STMSceneB").GetComponent<STMSceneB>();
                DomainModel.StateParameter.Register<STMSceneB>(stmB);
                stmB.BindGenericSTM(m_genericStateParameter);

                yield return SceneManager.LoadSceneAsync("SceneC", LoadSceneMode.Additive);
                var stmC = GameObject.Find("STMSceneC").GetComponent<STMSceneC>();
                DomainModel.StateParameter.Register<STMSceneC>(stmC);
                stmC.BindGenericSTM(m_genericStateParameter);

                fnInitSTM();
            }

            StartCoroutine(fnInitializeEvent());
        }
    }
}