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
    [CreateAssetMenu(menuName = "StateMachineService/Editor/StateParameterSettings")]
    public class StateParameterEditorSettings : ScriptableObject, IStateMachineServiceEditor
    {
        public string ScriptPath;

        public string PrefabPath;

        public TextAsset RepositoryScriptTemp;

        public TextAsset EntityBaseScriptTemp;

        public string ServiceName;

        public string EntityName;

        public void Handle(GUIStyle style)
        {
            EditorGUILayout.LabelField("3. サービス名を入力する", style);
            ServiceName = EditorGUILayout.TextField("StateNode Name", ServiceName);

            EditorGUILayout.LabelField("4. レポジトリスクリプトのテンプレートを選択する", style);
            RepositoryScriptTemp = EditorGUILayout.ObjectField("Repository Scirpt Template", RepositoryScriptTemp, typeof(TextAsset), true) as TextAsset;

            EditorGUILayout.LabelField("5. エンティティ名を入力する", style);
            EntityName = EditorGUILayout.TextField("Entity Name", EntityName);

            EditorGUILayout.LabelField("6. エンティティスクリプトのテンプレートを選択する", style);
            EntityBaseScriptTemp = EditorGUILayout.ObjectField("Entity Scirpt Template", EntityBaseScriptTemp, typeof(TextAsset), true) as TextAsset;

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

    public class StateParameterScriptCreateService : IScriptCreateService
    {
        public class Command : FileCreateServiceCommand
        {
            public string FilePath;

            public TextAsset RepositoryScriptTemp;

            public TextAsset EntityBaseScriptTemp;

            public string ServiceName;

            public string EntityName;
        }

        public FileCreateServiceCommand ThisCommand;

        public void MakeScript()
        {
            void fnMake(string filePath, string script) => File.WriteAllText(filePath, script);

            var scriptCommand = ThisCommand as Command;

            fnMake(
                scriptCommand.FilePath + "/" + scriptCommand.EntityName + ".cs",
                scriptCommand.EntityBaseScriptTemp.text.Replace(@"#SERVICE_NAME#", scriptCommand.ServiceName).Replace(@"#ENTITY_NAME#", scriptCommand.EntityName));

            fnMake(
                scriptCommand.FilePath + "/" + scriptCommand.EntityName + "Repository.cs",
                scriptCommand.RepositoryScriptTemp.text.Replace(@"#SERVICE_NAME#", scriptCommand.ServiceName).Replace(@"#ENTITY_NAME#", scriptCommand.EntityName));


            AssetDatabase.Refresh();
        }
    }

    public class StateParameterPrefabCreateService : IPrefabCreateService
    {
        public class Command : FileCreateServiceCommand
        {
            public string FilePath;

            public string EntityName;

            public string ServiceName;

            public StateMachineServiceSettings STMSettings;
        }

        public FileCreateServiceCommand ThisCommand;

        public void MakePrefab()
        {
            var prefabCommand = ThisCommand as Command;

            if (MakeStateMachineService.ExistTypeInThisProject(prefabCommand.EntityName + "Repository", out Type type))
            {
                var obj = Resources.Load<GameObject>("StateParameterRepositoryTemp");
                var game = PrefabUtility.InstantiateAttachedAsset(obj) as GameObject;
                game.AddComponent(type);

                var prefab = PrefabUtility.SaveAsPrefabAsset(game, prefabCommand.FilePath + "/" + prefabCommand.EntityName + "Repository.prefab");

                if (prefabCommand.STMSettings != null)
                {
                    if(prefabCommand.STMSettings.StateParameters_GameObject == null)
                        prefabCommand.STMSettings.StateParameters_GameObject = new List<GameObject>();

                    prefabCommand.STMSettings.StateParameters_GameObject.Add(prefab);
                    EditorUtility.SetDirty(prefabCommand.STMSettings);
                    AssetDatabase.SaveAssets();
                }


                UnityEngine.Object.DestroyImmediate(game);
                AssetDatabase.SaveAssets();
            }
        }
    }
}