using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM.Param
{
    public class ParamManagerBase_Prefab<MANAGE_PARAMETER_TYPE> where MANAGE_PARAMETER_TYPE : MonoBehaviour
    {
        protected virtual List<MANAGE_PARAMETER_TYPE> PrefabParameters
        {
            get;
            set;
        } = new List<MANAGE_PARAMETER_TYPE>();

        public List<MANAGE_PARAMETER_TYPE> CreatedParameters
        {
            get;
            set;
        } = new List<MANAGE_PARAMETER_TYPE>();

        public Transform ParameterRoot
        {
            get;
            set;
        }

        public virtual void Initialize(List<MANAGE_PARAMETER_TYPE> parameter, Transform root) { }

        public virtual void Remove<PARAMETER>() where PARAMETER : MANAGE_PARAMETER_TYPE
        {
            int index_typeof = (CreatedParameters.FindIndex(param => typeof(PARAMETER) == param.GetType()));
            int index_is = (CreatedParameters.FindIndex(param => param is PARAMETER));
            int index = index_typeof >= 0 ? index_typeof : index_is;

            if (index >= 0)
            {
                var target = CreatedParameters[index].gameObject;
                GameObject.Destroy(target);
                CreatedParameters.RemoveAt(index);
            }
        }

        public PARAMETER GetOrCreate<PARAMETER>() where PARAMETER : MANAGE_PARAMETER_TYPE
        {
            var parameter_typeof = (PARAMETER)(PrefabParameters.Find(param => typeof(PARAMETER) == param.GetType()));
            var parameter_is = (PARAMETER)(PrefabParameters.Find(param => param is PARAMETER));
            var parameter_res = parameter_typeof != null ? parameter_typeof : parameter_is;


            var created_typeof = (PARAMETER)(CreatedParameters.Find(param => typeof(PARAMETER) == param.GetType()));
            var created_is = (PARAMETER)(CreatedParameters.Find(param => param is PARAMETER));
            var created_res = created_typeof != null ? created_typeof : created_is;

            if (created_res != null)
                return (PARAMETER)created_res;
            else if (parameter_res != null)
            {
                var newParameter = GameObject.Instantiate(
                    parameter_res.gameObject,
                    ParameterRoot).GetComponent<PARAMETER>();

                CreatedParameters.Add(newParameter);

                return (PARAMETER)newParameter;
            }

            return default;
        }
    }
}