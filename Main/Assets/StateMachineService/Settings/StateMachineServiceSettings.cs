using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateParameterRepository;
using StateMachineService.StateNode;
using StateMachineService.StateParameter;

namespace StateMachineService.Settings
{
    [CreateAssetMenu(menuName = "StateMachineService/ServiceSettings")]    
    public class StateMachineServiceSettings : ScriptableObject
    {
        [Header("State Node Settings")]
        [SerializeField]
        public List<GameObject> StateNodes_GameObject; 

        [Header("State Parameter Repository Settings")]        
        [SerializeField]
        public GameObject StateParameterRepository_GameObject;


        [Header("State Parameter Settings")]  
        [SerializeField]
        public List<GameObject> StateParameters_GameObject;
    }
}