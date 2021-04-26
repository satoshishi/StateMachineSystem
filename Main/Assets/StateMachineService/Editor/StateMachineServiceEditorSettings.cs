using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEditor.Events;
using System.IO;
using UnityEditor.Callbacks;
using System;
using System.Reflection;
using StateMachineService.StateNode;
using StateMachineService.Locator;
using StateMachineService.StateMachine.Paupawsan;

namespace StateMachineService.Editor
{
    [CreateAssetMenu(menuName = "StateMachineService/Editor/StateMachineServiceSettings")]
    public class StateMachineServiceEditorSettings : ScriptableObject, IStateMachineServiceEditor
    {
        public string ScriptPath;

        public string PrefabPath;

        public TextAsset STMScriptTemp;

        public TextAsset InstallerScriptTemp;

        public string ServiceName;

        public GameObject FirstStateGameObject;

        public void Handle(GUIStyle style)
        {
            EditorGUILayout.LabelField("3. スクリプト名を入力する(Namespace名にも使われます(入力名.StateMachine))", style);
            ServiceName = EditorGUILayout.TextField("Script and Namespace Name", ServiceName);

            EditorGUILayout.LabelField("4. StateMachineのスクリプトテンプレートを選択する", style);
            STMScriptTemp = EditorGUILayout.ObjectField("Scirpt Template(STM)", STMScriptTemp, typeof(TextAsset), true) as TextAsset;

            EditorGUILayout.LabelField("5. Installerのスクリプトテンプレートを選択する", style);
            InstallerScriptTemp = EditorGUILayout.ObjectField("Scirpt Template(Installer)", InstallerScriptTemp, typeof(TextAsset), true) as TextAsset;

            EditorGUILayout.LabelField("6. 一番最初に遷移するStateを選択する", style);
            FirstStateGameObject = EditorGUILayout.ObjectField("First State", FirstStateGameObject, typeof(GameObject), true) as GameObject;

            GUI.backgroundColor = Color.green;
            OnGUI_SelectScirptPath(style);

            GUI.backgroundColor = Color.yellow;
            OnGUI_SelectPrefabPath(style);
        }

        public void OnGUI_SelectScirptPath(GUIStyle style)
        {
            EditorGUILayout.LabelField("7. スクリプトを作成するディレクトリを選択する", style);

            if (GUILayout.Button("Path"))
                ScriptPath = EditorUtility.OpenFolderPanel("Choice Path", Application.dataPath, string.Empty);

            EditorGUILayout.LabelField(ScriptPath);
        }

        public void OnGUI_SelectPrefabPath(GUIStyle style)
        {
            EditorGUILayout.LabelField("8. プレファブを作成するディレクトリを選択する", style);

            if (GUILayout.Button("Path"))
                PrefabPath = EditorUtility.OpenFolderPanel("Choice Path", Application.dataPath, string.Empty);

            EditorGUILayout.LabelField(PrefabPath);
        }
    }

    public class StateMachineServiceScriptCreateService : IScriptCreateService
    {
        public class Command : FileCreateServiceCommand
        {
            public string FilePath;

            public TextAsset STMScriptTemp;

            public TextAsset InstallerScriptTemp;

            public string ServiceName;

            public GameObject FirstStateGameObject;
        }

        public FileCreateServiceCommand ThisCommand;

        public void MakeScript()
        {
            var scriptCommand = ThisCommand as Command;

            var stmFilePath = scriptCommand.FilePath + "/" + scriptCommand.ServiceName + "StateMachine.cs";
            var stmCode = scriptCommand.STMScriptTemp.text.Replace(@"#SERVICE_NAME#", scriptCommand.ServiceName).Replace(@"#FIRST_STATE_NODE#", scriptCommand.FirstStateGameObject.name);
            File.WriteAllText(stmFilePath, stmCode);

            var installerFilePath = scriptCommand.FilePath + "/" + scriptCommand.ServiceName + "Installer.cs";
            var intallerCode = scriptCommand.InstallerScriptTemp.text.Replace(@"#SERVICE_NAME#", scriptCommand.ServiceName);
            File.WriteAllText(installerFilePath, intallerCode);            

            AssetDatabase.Refresh();
        }
    }

    public class StateMachineServicePrefabCreateService : IPrefabCreateService
    {
        public class Command : FileCreateServiceCommand
        {

            public string FilePath;

            public string ServiceName;

            public StateNodeSettings Settings;

            public GameObject FirstStateGameObject;
        }

        public FileCreateServiceCommand ThisCommand;

        public void MakePrefab()
        {
            var prefabCommand = ThisCommand as Command;

            if (MakeStateMachineService.ExistTypeInThisProject(prefabCommand.ServiceName + "StateMachine", out Type stmType) &&
                MakeStateMachineService.ExistTypeInThisProject(prefabCommand.ServiceName + "Installer", out Type installerType))
            {
                var obj = Resources.Load<GameObject>("StateMachineServiceTemp");
                var game = PrefabUtility.InstantiateAttachedAsset(obj) as GameObject;
                game.AddComponent(stmType);
                game.AddComponent(installerType);

                prefabCommand.Settings.SetFirstStateNodeGameObject(prefabCommand.FirstStateGameObject);

                var nodeList = game.GetComponent<StateNodeList>();
                var intaller = game.GetComponent<Installer>();
                var stm = game.GetComponent<PaupawsanStateMachine>();

                nodeList.SetStateNodeSettings(prefabCommand.Settings);
                UnityEventTools.AddObjectPersistentListener(intaller.m_onInstalled,new UnityAction<GameObject>(nodeList.Initialize),game);                
                UnityEventTools.AddObjectPersistentListener(intaller.m_onInstalled,new UnityAction<GameObject>(stm.Initialize),game);

                intaller.m_autoInstallHierarchyObject.Add(game);
                intaller.m_PrefabRoot = game.transform.GetChild(1);

                EditorUtility.SetDirty(prefabCommand.Settings);
                PrefabUtility.SaveAsPrefabAsset(game, prefabCommand.FilePath + "/" + prefabCommand.ServiceName + "StateMachine.prefab");
                UnityEngine.Object.DestroyImmediate(game);
                AssetDatabase.SaveAssets();
            }
        }
    }

}