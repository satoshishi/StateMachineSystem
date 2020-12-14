using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.Param;

namespace Samples.Parameters.Prefabs
{

    public class Prefaber : MonoBehaviour,IMangeParameter { }

    public class PrefabMaangerSample : StateParameter
    {
        public Manager Instance
        {
            get;
            private set;
        } = null;

        [SerializeField]
        private List<Prefaber> m_prefabs;

        [SerializeField]
        private Transform m_root;

        private void Awake()
        {
            Instance = transform.gameObject.AddComponent<Manager>();
            Instance.Initialize(m_prefabs, m_root);
        }

        public class Manager : ParamManagerBase<Prefaber>
        {
            public List<Prefaber> CreatedParams 
            {
                get;
                set;
            } = new List<Prefaber>();

            public Transform ParamsRoot
            {
                get;
                set;
            } = null;

            public void Initialize(List<Prefaber> prefabs,Transform root) 
            {
                Parameters = prefabs;
                ParamsRoot = root;
            }

            public override void UnRegister<PARAMETER>()
            {
                int index = CreatedParams.FindIndex(param => typeof(PARAMETER) == param.GetType());

                if (index >= 0)
                {
                    var target = CreatedParams[index].gameObject;
                    GameObject.Destroy(target);
                    CreatedParams.RemoveAt(index);
                }
            }

            public PARAMETER Create<PARAMETER>() where PARAMETER : Prefaber
            {

                var paramter_child = (PARAMETER)(Parameters.Find(param => param is PARAMETER));
                var parameter_parent = (PARAMETER)(Parameters.Find(param => typeof(PARAMETER) == param.GetType()));
                var parameter_res = parameter_parent != null ? parameter_parent : paramter_child;

                var created_child = (PARAMETER)(CreatedParams.Find(param => param is PARAMETER));
                var created_parent = (PARAMETER)(CreatedParams.Find(param => typeof(PARAMETER) == param.GetType()));
                var created_res = created_child != null ? created_child : created_parent;

                if (created_res != null)
                    return (PARAMETER)created_res;
                else if (parameter_res != null)
                {
                    var newParameter = Instantiate(parameter_res.gameObject, ParamsRoot).GetComponent <Prefaber>();
                    CreatedParams.Add(newParameter);

                    return (PARAMETER)newParameter;
                }

                return default;
            }
        }
    }
}