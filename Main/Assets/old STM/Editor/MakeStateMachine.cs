using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using STM.STN;
using STM;
using UnityEditor.Callbacks;
using System;
using System.Reflection;
using STM.DomainModel;

public class MakeStateMachine : STMEditor
{
    public string StateMachineName
    {
        get;
        set;
    } = "";

    public string FilePath
    {
        get;
        set;
    } = "";

    MakeStateMachineSettings Format
    {
        get;
        set;
    } = null;

    StateMachineSettings Settings
    {
        get;
        set;
    } = null;

    [MenuItem("Tools/STM Editor/Make StateMachine")]
    private static void Create()
    {
        GetWindow<MakeStateMachine>("Make StateMachine");
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        GUIStyleState state = new GUIStyleState();
        state.textColor = Color.white;
        style.normal = state;

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        MakeScriptEdit(style);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        MakePrefabEdit(style);
    }

    public override void MakePrefabEdit(GUIStyle style)
    {
        //https://qiita.com/kyanberu/items/fb6e1f5adc7de491d852
        void fnMake(string path, string prefabName)
        {
            var splitedName = prefabName.Split('.');
            prefabName = splitedName.Length <= 1 ? prefabName : splitedName[splitedName.Length - 1];

            if (IsContainsType(prefabName, out Type type))
            {
                var obj = Resources.Load<GameObject>("StateMachineTemp");
                var game = PrefabUtility.InstantiateAttachedAsset(obj) as GameObject;
                game.AddComponent(type);

                if (Settings != null)
                {
                    var model = game.GetComponent<StateMachineDomainModel>();
                    model.STMSettings = Settings;
                }

                var prefab = PrefabUtility.SaveAsPrefabAsset(game, path + "/" + prefabName + ".prefab");

                UnityEngine.Object.DestroyImmediate(game);
                AssetDatabase.SaveAssets();
            }
        }

        EditorGUILayout.LabelField("Make Prefab", style);

        Settings = EditorGUILayout.ObjectField("Target STM Settings", Settings, typeof(StateMachineSettings), true) as StateMachineSettings;
        StateMachineName = EditorGUILayout.TextField("StateMachine Name",StateMachineName);

        if (GUILayout.Button("Path"))
            FilePath = EditorUtility.OpenFolderPanel("Choice Prefab Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(FilePath);

        if (GUILayout.Button("Make"))
        {
            if (!string.IsNullOrEmpty(FilePath))
                fnMake(FilePath,StateMachineName);
        }
    }

    public override void MakeScriptEdit(GUIStyle style)
    {
        void fnMake(string path, string nodeName, string nodeBaseName, string firstNodeName, string template, string nameSpace)
        {
            var filePath = path + "/" + nodeName + ".cs";
            var code = template.Replace(@"#STM_NAME#", nodeName).Replace(@"#NODE_BASE#", nodeBaseName).Replace(@"#FIRST_NODE#", firstNodeName).Replace(@"#NAME_SPACE#", nameSpace);
            File.WriteAllText(filePath, code);
            AssetDatabase.Refresh();
        }

        EditorGUILayout.LabelField("Make Script", style);

        Format = EditorGUILayout.ObjectField("TemplateSettings", Format, typeof(MakeStateMachineSettings), true) as MakeStateMachineSettings;

        StateMachineName = EditorGUILayout.TextField("StateMachine Name",StateMachineName);

        if (GUILayout.Button("Path"))
            FilePath = EditorUtility.OpenFolderPanel("Choice StateNode Script Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(FilePath);

        if (GUILayout.Button("Make"))
        {
            if (!string.IsNullOrEmpty(FilePath))
                fnMake(FilePath,StateMachineName, Format.StateNodeBaseSettings.StateNodeBaseName, Format.FirstTransitionStateNode.name, Format.SourceCode.text,Format.StateNodeBaseSettings.NameSpace);
        }
    }

}