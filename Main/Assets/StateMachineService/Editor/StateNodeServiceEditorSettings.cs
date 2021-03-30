using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Callbacks;
using System;
using System.Reflection;
using StateMachineService.StateMachine;
using System.Text.RegularExpressions;

namespace StateMachineService.Editor
{

    [CreateAssetMenu(menuName = "StateMachineService/Editor/StateNodeServiceSettings")]
    public class StateNodeServiceEditorSettings : ScriptableObject, IStateMachineServiceEditor
    {
        public string ScriptPath;

        public string PrefabPath;

        public TextAsset ScriptTemp;

        public string ServiceName;

        public List<string> StateNames;

        public void Handle(GUIStyle style)
        {
            EditorGUILayout.LabelField("3. サービス名を入力する", style);
            ServiceName = EditorGUILayout.TextField("StateNode Name", ServiceName);

            EditorGUILayout.LabelField("4. スクリプトのテンプレートを選択する", style);
            ScriptTemp = EditorGUILayout.ObjectField("Scirpt Template", ScriptTemp, typeof(TextAsset), true) as TextAsset;

            OnGUII_SelectStateNames(style);

            GUI.backgroundColor = Color.green;
            OnGUI_SelectScirptPath(style);

            GUI.backgroundColor = Color.yellow;
            OnGUI_SelectPrefabPath(style);
        }

        public void OnGUII_SelectStateNames(GUIStyle style)
        {
            EditorGUILayout.LabelField("5. 作成するState名を入力する", style);

            var so = new SerializedObject(this);
            so.Update();
            EditorGUILayout.PropertyField(so.FindProperty("StateNames"), true);

            so.ApplyModifiedProperties();
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

    public class StateNodeServiceScriptCreateService : IScriptCreateService
    {
        public class Command : FileCreateServiceCommand
        {
            public string FilePath;

            public TextAsset ScriptTemp;

            public string ServiceName;

            public List<string> StateNames;
        }

        public FileCreateServiceCommand ThisCommand;

        public void MakeScript()
        {
            var scriptCommand = ThisCommand as Command;

            foreach (string stateName in scriptCommand.StateNames)
            {
                var nodeFilePath = scriptCommand.FilePath + "/" + stateName + "State.cs";
                var nodeCode = scriptCommand.ScriptTemp.text.Replace(@"#SERVICE_NAME#", scriptCommand.ServiceName).Replace(@"#STATE_NAME#", stateName);
                File.WriteAllText(nodeFilePath, nodeCode);
            }

            AssetDatabase.Refresh();
        }
    }

    public class StateNodeServicePrefabCreateService : IPrefabCreateService
    {
        public class Command : FileCreateServiceCommand
        {
            public string FilePath;

            public List<string> StateNames;

            public StateMachineParameterSettings STMSettings;
        }

        public FileCreateServiceCommand ThisCommand;

        public void MakePrefab()
        {
            var prefabCommand = ThisCommand as Command;

            foreach (string stateName in prefabCommand.StateNames)
            {
                if (MakeStateMachineService.ExistTypeInThisProject(stateName+"State", out Type type))
                {
                    var obj = Resources.Load<GameObject>("StateNodeServiceTemp");
                    var game = PrefabUtility.InstantiateAttachedAsset(obj) as GameObject;
                    game.AddComponent(type);

                    var prefab = PrefabUtility.SaveAsPrefabAsset(game, prefabCommand.FilePath + "/" + stateName + "State.prefab");

                    if (prefabCommand.STMSettings != null)
                    {
                        if(prefabCommand.STMSettings.StateNodesGameObject == null)
                            prefabCommand.STMSettings.StateNodesGameObject = new List<GameObject>();

                        prefabCommand.STMSettings.StateNodesGameObject.Add(prefab);
                        EditorUtility.SetDirty(prefabCommand.STMSettings);
                        AssetDatabase.SaveAssets();
                    }


                    UnityEngine.Object.DestroyImmediate(game);
                    AssetDatabase.SaveAssets();
                }
            }
        }
    }
}