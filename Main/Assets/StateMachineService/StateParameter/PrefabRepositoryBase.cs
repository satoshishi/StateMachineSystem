using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineService.StateParameter
{
    public class PrefabRepositoryBase<ENTITY> : MonoBehaviour, IRepository<ENTITY> where ENTITY : MonoBehaviour
    {
        public List<ENTITY> Entities { get { return entities; } }
        protected List<ENTITY> entities = new List<ENTITY>();

        public List<ENTITY> Entities_Prefab {get{return entities_prefab;}}
        protected List<ENTITY> entities_prefab = new List<ENTITY>();

        public virtual TYPE Get<TYPE>() where TYPE : ENTITY
        {
            var entity_typeof = (TYPE)(Entities.Find(param => typeof(TYPE) == param.GetType()));
            var entity_is = (TYPE)(Entities.Find(param => param is TYPE));
            var entity_res = entity_typeof != null ? entity_typeof : entity_is;

            return entity_res == null ? default : (TYPE)entity_res;
        }

        public void Create<TYPE>() where TYPE : ENTITY
        {
            var cretedEntity = Get<TYPE>();
            if(cretedEntity != null) return; //既に作られていたら、作らない

            var entity_typeof = (TYPE)(Entities_Prefab.Find(param => typeof(TYPE) == param.GetType()));
            var entity_is = (TYPE)(Entities_Prefab.Find(param => param is TYPE));
            var entity_res = entity_typeof != null ? entity_typeof : entity_is;

            if(entity_res != null)
            {
                var new_entity = GameObject.Instantiate(
                    entity_res.gameObject,
                    this.transform).GetComponent<TYPE>();

                entities.Add(new_entity);               
            }
        }

        public void Remove<TYPE>() where TYPE : ENTITY
        {
            int index_typeof = (Entities_Prefab.FindIndex(param => typeof(TYPE) == param.GetType()));
            int index_is = (Entities_Prefab.FindIndex(param => param is TYPE));
            int index = index_typeof >= 0 ? index_typeof : index_is;

            if (index >= 0)
            {
                var target = entities_prefab[index].gameObject;
                GameObject.Destroy(target);
                entities_prefab.RemoveAt(index);
            }
        }                
    }
}