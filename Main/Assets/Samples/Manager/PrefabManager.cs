using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.Param;

namespace Prefab
{

    public class PrefabObj : MonoBehaviour { }

    public class PrefabManager : StateMachineParamManager
    {
        public class StateParamContainer : ParamManagerBase_Prefab<PrefabObj>
        {
            public override void Initialize(List<PrefabObj> parameters, Transform root)
            {
                PrefabParameters = new List<PrefabObj>(parameters);
                ParameterRoot = root;
            }
        }

        public StateParamContainer Collections
        {
            get;
            private set;
        } = null;

        public readonly string ResourcesFolderPath = "PrefabObj";

        [SerializeField]
        private List<PrefabObj> m_prefabs;

        [SerializeField]
        private Transform m_root;

        private void Awake()
        {
            var resoruces = Resources.LoadAll(ResourcesFolderPath,typeof(PrefabObj));
            foreach(var resoruce in resoruces)
                m_prefabs.Add((PrefabObj)resoruce);

            Collections = new StateParamContainer();
            Collections.Initialize(m_prefabs, m_root);
        }
    }
}