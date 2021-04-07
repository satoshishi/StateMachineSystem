using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineService.StateMachine.Parameter
{
    [CreateAssetMenu(menuName = "StateMachineService/StateMachineParameterSettings")]
    public class StateMachineParameterSettings : ScriptableObject
    {
        [Header("State Node Settings")]
        [SerializeField]
        private List<GameObject> stateNodesGameObject;
        public List<GameObject> StateNodesGameObject { get { return stateNodesGameObject; } }

        public void SetStateNodesGameObject(List<GameObject> stateNodesGameObject)
        {
            this.stateNodesGameObject = stateNodesGameObject;
        }

        [Header("First State Node Settings")]
        [SerializeField]
        private GameObject firstStateNodeGameObject;
        public GameObject FirstStateNodeGameObject { get { return firstStateNodeGameObject; } }

        public void SetFirstStateNodeGameObject(GameObject firstStateNodeGameObject)
        {
            this.firstStateNodeGameObject = firstStateNodeGameObject;
        }

        [Header("Service Settings")]
        [SerializeField]
        private List<GameObject> stateParametersGameObject;
        public List<GameObject> StateParametersGameObject { get { return stateParametersGameObject; } }

        public void SetStateParametersGameObject(List<GameObject> stateParametersGameObject)
        {
            this.stateParametersGameObject = stateParametersGameObject;
        }
    }
}