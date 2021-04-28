using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineService.Locator
{

    [CreateAssetMenu(menuName = "StateMachineService/PrefabInstallSettings")]
    public class AutoInstallPrefabSettings : ScriptableObject
    {
        [SerializeField]
        private List<GameObject> intallTargetPrefabs;
        public List<GameObject> IntallTargetPrefabs { get { return intallTargetPrefabs; } }        
    }
}