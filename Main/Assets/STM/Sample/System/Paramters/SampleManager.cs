using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.Param;

namespace Sample
{

    public class SampleParameter : MonoBehaviour,IMangeParameter { }

    public class SampleManager : StateParameter
    {
        public readonly string ResourcesFolderPath = "SampleParameter";

        public Manager Instance
        {
            get;
            private set;
        } = null;

        [SerializeField]
        private List<SampleParameter> m_prefabs;

        [SerializeField]
        private Transform m_root;

        private void Awake()
        {
            Instance = transform.gameObject.AddComponent<Manager>();

            var resoruces = Resources.LoadAll(ResourcesFolderPath,typeof(SampleParameter));
            foreach(var resoruce in resoruces)
                m_prefabs.Add((SampleParameter)resoruce);

            Instance.Initialize(m_prefabs, m_root);
        }

        public class Manager : ParamManagerBase<SampleParameter>
        {
            public List<SampleParameter> CreatedParams 
            {
                get;
                set;
            } = new List<SampleParameter>();

            public Transform ParamsRoot
            {
                get;
                set;
            } = null;

            public void Initialize(List<SampleParameter> prefabs,Transform root) 
            {
                Parameters = prefabs;
                ParamsRoot = root;
            }

            public override void UnRegister<PARAMETER>()
            {
                int index_typeof = (CreatedParams.FindIndex(param => typeof(PARAMETER) == param.GetType()));
                int index_is = (CreatedParams.FindIndex(param => param is PARAMETER));
                int index = index_typeof >= 0 ? index_typeof : index_is;

                if (index >= 0)
                {
                    var target = CreatedParams[index].gameObject;
                    GameObject.Destroy(target);
                    CreatedParams.RemoveAt(index);
                }
            }

            public PARAMETER Create<PARAMETER>() where PARAMETER : SampleParameter
            {
                var parameter_typeof = (PARAMETER)(Parameters.Find(param => typeof(PARAMETER) == param.GetType()));
                var parameter_is = (PARAMETER)(Parameters.Find(param => param is PARAMETER));
                var parameter_res = parameter_typeof != null ? parameter_typeof : parameter_is;


                var created_typeof = (PARAMETER)(CreatedParams.Find(param => typeof(PARAMETER) == param.GetType()));
                var created_is = (PARAMETER)(CreatedParams.Find(param => param is PARAMETER));
                var created_res = created_typeof != null ? created_typeof : created_is;

                if (created_res != null)
                    return (PARAMETER)created_res;
                else if (parameter_res != null)
                {
                    var newParameter = Instantiate(parameter_res.gameObject, ParamsRoot).GetComponent <SampleParameter>();
                    CreatedParams.Add(newParameter);

                    return (PARAMETER)newParameter;
                }

                return default;
            }
        }
    }
}