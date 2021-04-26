using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StateMachineService.Locator
{

    [DefaultExecutionOrder(-9999)]
    public class Installer : MonoBehaviour
    {
        public List<GameObject> m_autoInstallHierarchyObject = new List<GameObject>();

        public UnityEngine.Events.UnityEvent m_onInstalled = new UnityEngine.Events.UnityEvent();

        public PrefabInstallSettings m_prefabSettins;

        public Transform m_PrefabRoot;

        private void Awake()
        {
            Install_FromHierarchyObject();
            Install_FromPrefabObject();
            Install_Instance();

            m_onInstalled.Invoke();
        }

        protected virtual void Install_FromPrefabObject()
        {
            if(m_prefabSettins == null)
                return;

            foreach (GameObject prefab in m_prefabSettins.IntallTargetPrefabs)
            {
                var target = Instantiate(prefab,m_PrefabRoot);
                var attributes = Get_AutoRegistOnPrefabScriptAttribute_FromGameObject(target);
                Regist_ServiceLocator_FromGameObjectAndAttribute(target,attributes);
            }            
        }

        protected virtual void Install_Instance()
        {

        }

        protected virtual void Install_FromHierarchyObject()
        {
            foreach (GameObject target in m_autoInstallHierarchyObject)
            {
                var attributes = Get_AutoRegistOnPrefabScriptAttribute_FromGameObject(target);
                Regist_ServiceLocator_FromGameObjectAndAttribute(target,attributes);
            }
        }

        protected IEnumerable<AutoRegistOnPrefabScriptAttribute> Get_AutoRegistOnPrefabScriptAttribute_FromGameObject(GameObject target)
        {
            var targetScripts = target.GetComponents<MonoBehaviour>().Select(mono => mono.GetType());
            var attributes = targetScripts
                    .Where(scirpt => scirpt.GetCustomAttributes(typeof(AutoRegistOnPrefabScriptAttribute), false).Any())
                    .Select(scirpt => (scirpt.GetCustomAttributes(typeof(AutoRegistOnPrefabScriptAttribute), false).OfType<AutoRegistOnPrefabScriptAttribute>().First()));

            return attributes;
        }

        protected void Regist_ServiceLocator_FromGameObjectAndAttribute(GameObject target, IEnumerable<AutoRegistOnPrefabScriptAttribute> attributes)
        {
            attributes.ToList().ForEach(
                attribute =>
                {
                    var targetType = attribute.RegistTargetType;
                    ServiceLocator.Register(targetType, target.GetComponent(targetType));
                });
        }
    }
}