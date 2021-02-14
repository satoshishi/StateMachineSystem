using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace STM.Param
{
    [Serializable]
    public abstract class ParamManagerBase<MANAGE_PARAMETER_TYPE> : MonoBehaviour where MANAGE_PARAMETER_TYPE : IMangeParameter
    {
        protected virtual List<MANAGE_PARAMETER_TYPE> Parameters
        {
            get;
            set;
        } = new List<MANAGE_PARAMETER_TYPE>();

        public virtual void Initialize(List<MANAGE_PARAMETER_TYPE> parameter){}

        public virtual void Register<PARAMETER>(PARAMETER parameter) where PARAMETER : MANAGE_PARAMETER_TYPE
        {
            int index  = Parameters.FindIndex(param => typeof(PARAMETER) == param.GetType());

            if(index >= 0)
                Parameters[index] = parameter;
            else Parameters.Add(parameter);
        }

        public virtual void UnRegister<PARAMETER>() where PARAMETER : MANAGE_PARAMETER_TYPE
        {
            int index  = Parameters.FindIndex(param => typeof(PARAMETER) == param.GetType());

            if(index >= 0)
                Parameters.RemoveAt(index);
        }

        public virtual bool TryGetParameter<PARAMETER>(out PARAMETER parameter) where PARAMETER : MANAGE_PARAMETER_TYPE
        {
            var res = GetParameter<PARAMETER>();

            if(res != null)
            {
                parameter = res;
                return true;
            }

            parameter = default;
            return false;
        }

        public virtual PARAMETER GetParameter<PARAMETER>() where PARAMETER : MANAGE_PARAMETER_TYPE
        {
            return (PARAMETER)(Parameters.Find(param => typeof(PARAMETER) == param.GetType()));
        }
    }

    public interface IMangeParameter
    {

    }    
}