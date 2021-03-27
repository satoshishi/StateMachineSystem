using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateParameter;
using StateMachineService.StateParameterRepository;

namespace Sample.StateParameterRepository
{
    public class SampleStateParameterRepository : MonoBehaviour, IStateParameterRepository
    {
        public List<IStateParameter> Parameters { get { return parameters; } }
        private List<IStateParameter> parameters = new List<IStateParameter>();

        public STATE_PARAMETER Get<STATE_PARAMETER>()
        {
            return (STATE_PARAMETER)(Parameters.Find(param => param is STATE_PARAMETER));
        }

        public bool Contains<STATE_PARAMETER>()
        {
            return Get<STATE_PARAMETER>() != null;    
        }

        public void Register<STATE_PARAMETER>(STATE_PARAMETER newParameter) where STATE_PARAMETER : IStateParameter
        {
            if(Contains<STATE_PARAMETER>())
                return;

            parameters.Add(newParameter);
        }

        public void Initialize(List<IStateParameter> parameters)
        {
            this.parameters = parameters;
        }
    }
}