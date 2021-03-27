using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.StateParameter;
using Sample.StateParameter.Entity;
using System.Linq;

namespace Sample.StateParameter
{
    public class PrefabRepository : PrefabRepositoryBase<Prefab>,IStateParameter
    {
        [SerializeField]
        private List<GameObject> m_entities_prefab;

        public void Awake()
        {
            m_entities_prefab.ForEach(prefab => entities_prefab.Add(prefab.GetComponent<Prefab>()));
        }
    }
}