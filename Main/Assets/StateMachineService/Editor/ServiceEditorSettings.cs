using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Callbacks;
using System;
using System.Reflection;
using StateMachineService.StateMachine;
using StateMachineService.StateMachine.Parameter;
using System.Text.RegularExpressions;

namespace StateMachineService.Editor
{
    [CreateAssetMenu(menuName = "StateMachineService/Editor/ServiceSettings")]
    public class ServiceEditorSettings : ScriptableObject, IStateMachineServiceEditor
    {
        public string ScriptPath;

        public string PrefabPath;

        public TextAsset ServiceInstallerScriptTemp;

        public TextAsset ServiceScriptTemp;

        public string ServiceName;

        public string ServiceTypeName;

        public void Handle(GUIStyle style)
        {
            EditorGUILayout.LabelField("4. 作成するスクリプト名を入力する", style);
            ServiceName = EditorGUILayout.TextField("Script Name", ServiceName);

            EditorGUILayout.LabelField("5. スクリプトが継承するの基底クラスorそのクラス名を入力する", style);
            ServiceTypeName = EditorGUILayout.TextField("Type Name", ServiceTypeName);

            EditorGUILayout.LabelField("6. 作成するサービスのテンプレートを選択する", style);
            ServiceScriptTemp = EditorGUILayout.ObjectField("Service Scirpt Template", ServiceScriptTemp, typeof(TextAsset), true) as TextAsset;

            EditorGUILayout.LabelField(". 作成するサービススクリプトのテンプレートを選択する", style);
            ServiceInstallerScriptTemp = EditorGUILayout.ObjectField("ServiceInstaller Scirpt Template", ServiceInstallerScriptTemp, typeof(TextAsset), true) as TextAsset;

            GUI.backgroundColor = Color.green;
            OnGUI_SelectScirptPath(style);

            GUI.backgroundColor = Color.yellow;
            OnGUI_SelectPrefabPath(style);
        }

        public void OnGUI_SelectScirptPath(GUIStyle style)
        {
            EditorGUILayout.LabelField("8. スクリプトを作成するディレクトリを選択する", style);

            if (GUILayout.Button("Path"))
                ScriptPath = EditorUtility.OpenFolderPanel("Choice Path", Application.dataPath, string.Empty);

            EditorGUILayout.LabelField(ScriptPath);
        }

        public void OnGUI_SelectPrefabPath(GUIStyle style)
        {
            EditorGUILayout.LabelField("9. プレファブを作成するディレクトリを選択する", style);

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

            public TextAsset ServiceInstallerScriptTemp;

            public TextAsset ServiceScriptTemp;

            public string ServiceName;

            public string ServiceTypeName;
        }

        public FileCreateServiceCommand ThisCommand;

        public void MakeScript()
        {
            void fnMake(string filePath, string script) => File.WriteAllText(filePath, script);

            var scriptCommand = ThisCommand as Command;

            fnMake(
                scriptCommand.FilePath + "/" + scriptCommand.ServiceName + ".cs",
                scriptCommand.ServiceScriptTemp.text.Replace(@"#SERVICE_NAME#", scriptCommand.ServiceName).Replace(@"#SERVICE_TYPE_NAME#", scriptCommand.ServiceTypeName));

            fnMake(
                scriptCommand.FilePath + "/" + scriptCommand.ServiceName + "Installer.cs",
                scriptCommand.ServiceInstallerScriptTemp.text.Replace(@"#SERVICE_NAME#", scriptCommand.ServiceName + "Installer").Replace(@"#SERVICE_INSTALLER_NAME#",scriptCommand.ServiceName).Replace(@"#SERVICE_TYPE_NAME#", scriptCommand.ServiceTypeName));


            AssetDatabase.Refresh();
        }
    }

    public class StateParameterPrefabCreateService : IPrefabCreateService
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

            if (MakeStateMachineService.ExistTypeInThisProject(prefabCommand.ServiceName + "Installer", out Type intallerType) &&
               MakeStateMachineService.ExistTypeInThisProject(prefabCommand.ServiceName, out Type serviceType))
            {
                var obj = Resources.Load<GameObject>("ServiceTemp");
                var game = PrefabUtility.InstantiateAttachedAsset(obj) as GameObject;
                game.AddComponent(serviceType);
                game.AddComponent(intallerType);

                var prefab = PrefabUtility.SaveAsPrefabAsset(game, prefabCommand.FilePath + "/" + prefabCommand.ServiceName + ".prefab");

                if (prefabCommand.STMSettings != null)
                {
                    if (prefabCommand.STMSettings.StateParametersGameObject == null)
                        prefabCommand.STMSettings.SetStateNodesGameObject(new List<GameObject>());

                    prefabCommand.STMSettings.StateParametersGameObject.Add(prefab);
                    EditorUtility.SetDirty(prefabCommand.STMSettings);
                    AssetDatabase.SaveAssets();
                }


                UnityEngine.Object.DestroyImmediate(game);
                AssetDatabase.SaveAssets();
            }
        }
    }
}