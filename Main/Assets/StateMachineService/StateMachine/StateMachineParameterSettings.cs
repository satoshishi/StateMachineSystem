using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineService.StateMachine
{
    [CreateAssetMenu(menuName = "StateMachineService/StateMachineParameterSettings")]    
    public class StateMachineParameterSettings : ScriptableObject
    {
        [Header("State Node Settings")]
        [SerializeField]
        public List<GameObject> StateNodesGameObject; 

        [Header("First State Node Settings")]  
        [SerializeField]
        public GameObject FirstStateNodeGameObject;                

        [Header("Service Settings")]  
        [SerializeField]
        public List<GameObject> StateParametersGameObject;
    }
}