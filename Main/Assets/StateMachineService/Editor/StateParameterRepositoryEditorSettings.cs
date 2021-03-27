using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Callbacks;
using System;
using System.Reflection;
using StateMachineService.Settings;
using System.Text.RegularExpressions;

namespace StateMachineService.Editor
{

    [CreateAssetMenu(menuName = "StateMachineService/Editor/StateParameterRepositorySettings")]
    public class StateParameterRepositoryEditorSettings : ScriptableObject, IStateMachineServiceEditor
    {
        public string ScriptPath;

        public string PrefabPath;

        public TextAsset ScriptTemp;

        public string ServiceName;

        public void Handle(GUIStyle style)
        {
            EditorGUILayout.LabelField("3. サービス名を入力する", style);
            ServiceName = EditorGUILayout.TextField("StateNode Name", ServiceName);

            EditorGUILayout.LabelField("4. スクリプトのテンプレートを選択する", style);
            ScriptTemp = EditorGUILayout.ObjectField("Scirpt Template", ScriptTemp, typeof(TextAsset), true) as TextAsset;

            GUI.backgroundColor = Color.green;
            OnGUI_SelectScirptPath(style);

            GUI.backgroundColor = Color.yellow;
            OnGUI_SelectPrefabPath(style);
        }

        public void OnGUI_SelectScirptPath(GUIStyle style)
        {
            EditorGUILayout.LabelField("5. スクリプトを作成するディレクトリを選択する", style);

            if (GUILayout.Button("Path"))
                ScriptPath = EditorUtility.OpenFolderPanel("Choice Path", Application.dataPath, string.Empty);

            EditorGUILayout.LabelField(ScriptPath);
        }

        public void OnGUI_SelectPrefabPath(GUIStyle style)
        {
            EditorGUILayout.LabelField("6. プレファブを作成するディレクトリを選択する", style);

            if (GUILayout.Button("Path"))
                PrefabPath = EditorUtility.OpenFolderPanel("Choice Path", Application.dataPath, string.Empty);

            EditorGUILayout.LabelField(PrefabPath);
        }
    }

    public class StateParameterRepositoryScriptCreateService : IScriptCreateService
    {
        public class Command : FileCreateServiceCommand
        {
            public string FilePath;

            public TextAsset ScriptTemp;

            public string ServiceName;

            public StateMachineServiceSettings STMSettings;
        }

        public FileCreateServiceCommand ThisCommand;

        public void MakeScript()
        {
            var scriptCommand = ThisCommand as Command;

            var nodeFilePath = scriptCommand.FilePath + "/" + scriptCommand.ServiceName + "StateParameterRepository.cs";
            var nodeCode = scriptCommand.ScriptTemp.text.Replace(@"#SERVICE_NAME#", scriptCommand.ServiceName);
            File.WriteAllText(nodeFilePath, nodeCode);

            AssetDatabase.Refresh();
        }
    }

    public class StateParameterRepositoryPrefabCreateService : IPrefabCreateService
    {
        public class Command : FileCreateServiceCommand
        {
            public string FilePath;

            public string ServiceName;

            public StateMachineServiceSettings STMSettings;
        }

        public FileCreateServiceCommand ThisCommand;

        public void MakePrefab()
        {
            var prefabCommand = ThisCommand as Command;

            if (MakeStateMachineService.ExistTypeInThisProject(prefabCommand.ServiceName + "StateParameterRepository", out Type type))
            {
                var obj = Resources.Load<GameObject>("StateParameterRepositoryTemp");
                var game = PrefabUtility.InstantiateAttachedAsset(obj) as GameObject;
                game.AddComponent(type);

                var prefab = PrefabUtility.SaveAsPrefabAsset(game, prefabCommand.FilePath + "/" + prefabCommand.ServiceName + "StateParameterRepository.prefab");

                if (prefabCommand.STMSettings != null)
                {
                    prefabCommand.STMSettings.StateParameterRepository_GameObject = prefab;
                    EditorUtility.SetDirty(prefabCommand.STMSettings);
                    AssetDatabase.SaveAssets();
                }


                UnityEngine.Object.DestroyImmediate(game);
                AssetDatabase.SaveAssets();
            }
        }
    }
}