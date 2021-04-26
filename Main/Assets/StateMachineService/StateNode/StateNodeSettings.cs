using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineService.StateNode
{
    [CreateAssetMenu(menuName = "StateMachineService/StateNodeSettings")]
    public class StateNodeSettings : ScriptableObject
    {
        [Header("StateNodes")]
        [SerializeField]
        private List<GameObject> stateNodesGameObject;
        public List<GameObject> StateNodesGameObject { get { return stateNodesGameObject; } }

        public void SetStateNodesGameObject(List<GameObject> stateNodesGameObject)
        {
            this.stateNodesGameObject = stateNodesGameObject;
        }

        [Header("First StateNode")]
        [SerializeField]
        private GameObject firstStateNodeGameObject;
        public GameObject FirstStateNodeGameObject { get { return firstStateNodeGameObject; } }

        public void SetFirstStateNodeGameObject(GameObject firstStateNodeGameObject)
        {
            this.firstStateNodeGameObject = firstStateNodeGameObject;
        }

    }

}