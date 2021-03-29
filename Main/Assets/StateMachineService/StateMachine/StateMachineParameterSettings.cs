using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineService.StateMachine
{
    [CreateAssetMenu(menuName = "StateMachineService/ServiceSettings")]    
    public class StateMachineParameterSettings : ScriptableObject
    {
        [Header("State Node Settings")]
        [SerializeField]
        public List<GameObject> StateNodes_GameObject; 

        [Header("State Parameter Settings")]  
        [SerializeField]
        public List<GameObject> StateParameters_GameObject;
    }
}