using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Callbacks;
using System;
using System.Reflection;
using StateMachineService.Parameter;
using StateMachineService.StateMachine;


namespace StateMachineService.Editor
{
    [CreateAssetMenu(menuName = "StateMachineService/Editor/StateMachineServiceSettings")]
    public class StateMachineServiceEditorSettings : ScriptableObject, IStateMachineServiceEditor
    {
        public string ScriptPath;

        public string PrefabPath;

        public TextAsset ScriptTemp;

        public string ServiceName;

        public GameObject FirstStateGameObject;

        public void Handle(GUIStyle style)
        {
            EditorGUILayout.LabelField("3. サービス名を入力する", style);
            ServiceName = EditorGUILayout.TextField("StateNode Name", ServiceName);

            EditorGUILayout.LabelField("4. スクリプトのテンプレートを選択する", style);
            ScriptTemp = EditorGUILayout.ObjectField("Scirpt Template", ScriptTemp, typeof(TextAsset), true) as TextAsset;

            EditorGUILayout.LabelField("5. 一番最初に遷移するStateを選択する", style);
            FirstStateGameObject = EditorGUILayout.ObjectField("First State", FirstStateGameObject, typeof(GameObject), true) as GameObject;

            GUI.backgroundColor = Color.green;
            OnGUI_SelectScirptPath(style);

            GUI.backgroundColor = Color.yellow;
            OnGUI_SelectPrefabPath(style);
        }

        public void OnGUI_SelectScirptPath(GUIStyle style)
        {
            EditorGUILayout.LabelField("6. スクリプトを作成するディレクトリを選択する", style);

            if (GUILayout.Button("Path"))
                ScriptPath = EditorUtility.OpenFolderPanel("Choice Path", Application.dataPath, string.Empty);

            EditorGUILayout.LabelField(ScriptPath);
        }

        public void OnGUI_SelectPrefabPath(GUIStyle style)
        {
            EditorGUILayout.LabelField("7. プレファブを作成するディレクトリを選択する", style);

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

            public TextAsset ScriptTemp;

            public string ServiceName;

            public GameObject FirstStateGameObject;
        }

        public FileCreateServiceCommand ThisCommand;

        public void MakeScript()
        {
            var scriptCommand = ThisCommand as Command;

            var nodeFilePath = scriptCommand.FilePath + "/" + scriptCommand.ServiceName + "StateMachine.cs";
            var nodeCode = scriptCommand.ScriptTemp.text.Replace(@"#SERVICE_NAME#", scriptCommand.ServiceName).Replace(@"#FIRST_STATE_NODE#", scriptCommand.FirstStateGameObject.name);
            File.WriteAllText(nodeFilePath, nodeCode);

            AssetDatabase.Refresh();
        }
    }

    public class StateMachineServicePrefabCreateService : IPrefabCreateService
    {
        public class Command : FileCreateServiceCommand
        {
            public string FilePath;

            public string ServiceName;

            public StateMachineParameterSettings STMSettings;
        }

        public FileCreateServiceCommand ThisCommand;

        public void MakePrefab()
        {
            var prefabCommand = ThisCommand as Command;

            if (MakeStateMachineService.ExistTypeInThisProject(prefabCommand.ServiceName + "StateMachine", out Type type))
            {
                var obj = Resources.Load<GameObject>("StateMachineServiceTemp");
                var game = PrefabUtility.InstantiateAttachedAsset(obj) as GameObject;
                game.AddComponent(type);

                if (prefabCommand.STMSettings != null)
                {                    
                    var initServices = game.GetComponent<StateMachineParameter>();

                    initServices.ServiceSettings = prefabCommand.STMSettings;
                    initServices.StateNodeRoot = game.transform.GetChild(0).transform;
                 //   initServices.RepositoryRoot = game.transform;
                }

                var prefab = PrefabUtility.SaveAsPrefabAsset(game, prefabCommand.FilePath + "/" + prefabCommand.ServiceName + "StateMachine.prefab");

                UnityEngine.Object.DestroyImmediate(game);
                AssetDatabase.SaveAssets();
            }
        }
    }

}