using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM.Core;
using STM.Param;
using STM.DomainModel;

namespace STM
{
    [RequireComponent(typeof(StateMachineDomainModel))]    
    public class StateMachineBase<STATE_NODE> : MonoBehaviour where STATE_NODE : StateNodeBase
    {
        protected StateMachineDomainModel DomainModel
        {
            get;
            set;
        } = null;

        protected List<GameObject> StateNodes
        {
            get;
            set;
        } = new List<GameObject>();

        protected StateMachineCore<STATE_NODE> STMCore
        {
            get;
            set;
        } = null;

        public virtual void Initialize<FIRST_STATE>(StateMachineDomainModel domainModel) where FIRST_STATE : STATE_NODE
        {
            void fnInitParamManager()
            {
                var paramManager = DomainModel.STMSettings.ParamManagers;
                List<StateMachineParamManager> paramManagerInstances = new List<StateMachineParamManager>();

                foreach (StateMachineParamManager sp in paramManager)
                {
                    var instance = Instantiate(sp.gameObject, Vector3.zero, Quaternion.identity, DomainModel.StateParameter.transform).GetComponent<StateMachineParamManager>();
                    paramManagerInstances.Add(instance);
                }

                paramManagerInstances.Add(this.GetComponent<StateMachineParamManager>());
                DomainModel.StateParameter.Initialize(paramManagerInstances);               
            }

            void fnInitSTM()
            {
                STMCore = new StateMachineCore<STATE_NODE>(UpdateStateNodeStatus, false);

                var stateNodes = DomainModel.STMSettings.StateNode;

                if (stateNodes.Find(target => !(target is STATE_NODE)) != null)
                {
                    Debug.LogError("このStateMachineで管理対象でないクラスを継承したStateNodeが存在します");
                    return;
                }

                foreach (STATE_NODE stn in stateNodes)
                {
                    var instance = Instantiate(stn.transform, Vector3.zero, Quaternion.identity, DomainModel.STNRoot);
                    StateNodes.Add(instance.gameObject);
                    STMCore.RegisterStateNode(instance.GetComponent<STATE_NODE>(), new StateNodeCore<STATE_NODE>(stn, STMCore));
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

        public virtual bool TryGetStateNode<NODE_TYPE>(out NODE_TYPE stn) where NODE_TYPE : STATE_NODE
        {
            var res = StateNodes.Find(statenode => statenode.GetComponent<STATE_NODE>() is NODE_TYPE);

            if (res != null)
            {
                stn = res.GetComponent<NODE_TYPE>();
                return true;
            }

            stn = default;
            return false;
        }

        public virtual bool TryGetStateNode<NODE_TYPE>(NODE_TYPE target, out NODE_TYPE stn) where NODE_TYPE : STATE_NODE
        {
            var res = StateNodes.Find(statenode => statenode.GetComponent<STATE_NODE>().GetType().Equals(target.GetType()));    

            if (res != null)
            {
                stn = res.GetComponent<NODE_TYPE>();
                return true;
            }

            stn = default;
            return false;            

        }

        public virtual IEnumerator UpdateStateNodeStatus(STATE_NODE stateType, eStateNodeStatus stateStatus)
        {      
            if(!TryGetStateNode<STATE_NODE>(stateType,out STATE_NODE node))
                yield break;

            switch (stateStatus)
            {
                case eStateNodeStatus.StateInitialize:
                    Debug.Log($"{node.GetType().BaseType}-{node.GetType()}-{stateStatus}");     
                    node.gameObject.name = $"{node.GetType()}";
                    node?.Initialize(DomainModel.StateParameter);
                    break;

                case eStateNodeStatus.StateEnter:

                    Debug.Log($"{node.GetType().BaseType}-{node.GetType()}-{stateStatus}");
                    node.gameObject.name = $"{node.GetType()} <";
                    node?.OnEnter();
                    break;

                case eStateNodeStatus.StateUpdate:
                  //  Debug.Log($"{stateType.GetType().BaseType}-{stateType.GetType()}-{stateStatus}");       
                    node?.OnUpdate();
                    break;

                case eStateNodeStatus.StateExit:
                    Debug.Log($"{node.GetType().BaseType}-{node.GetType()}-{stateStatus}"); 
                    node.gameObject.name = $"{node.GetType()}";        
                    node?.OnExit();
                    break;

                case eStateNodeStatus.StateFinalize:
                    break;
            }

            yield return null;
        }

        public virtual void UpdateState<NODE_TYPE>() where NODE_TYPE : STATE_NODE
        {
            if (TryGetStateNode<NODE_TYPE>(out NODE_TYPE stn))
                STMCore.MoveState(stn);
        }

        public virtual void ShutdownSTM()
        {
            STMCore.Shutdown();                              
        }
    }
}