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
using System.Linq;

// node名
// 継承元名
// 追加するscriptable object参照
// 保存先

public class MakeStateNode : STMEditor
{
    public string StateNodeName
    {
        get;
        set;
    } = "";

    public string FilePath
    {
        get;
        set;
    } = "";

    MakeStateNodeSettings NodeFormat
    {
        get;
        set;
    } = null;

    StateMachineSettings Settings
    {
        get;
        set;
    } = null;

    [MenuItem("Tools/STM Editor/Make StateNode")]
    private static void Create()
    {
        GetWindow<MakeStateNode>("Make StateNode");
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
                var obj = Resources.Load<GameObject>("StateNodeTemp");
                var game = PrefabUtility.InstantiateAttachedAsset(obj) as GameObject;
                game.AddComponent(type);

                var prefab = PrefabUtility.SaveAsPrefabAsset(game, path + "/" + prefabName + ".prefab");

                if (Settings != null)
                {
                    if(Settings.StateNode == null)
                       Settings.StateNode = new List<StateNodeBase>();

                    Settings.StateNode.Add(prefab.GetComponent<StateNodeBase>());
                    EditorUtility.SetDirty(Settings);
                    AssetDatabase.SaveAssets();
                }


                UnityEngine.Object.DestroyImmediate(game);
                AssetDatabase.SaveAssets();
            }
        }

        EditorGUILayout.LabelField("Make Prefab", style);

        Settings = EditorGUILayout.ObjectField("Target STM Settings", Settings, typeof(StateMachineSettings), true) as StateMachineSettings;
        StateNodeName = EditorGUILayout.TextField("Prefab Name", StateNodeName);

        if (GUILayout.Button("Path"))
            FilePath = EditorUtility.OpenFolderPanel("Choice Prefab Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(FilePath);

        if (GUILayout.Button("Make"))
        {
            if (!string.IsNullOrEmpty(FilePath))
                fnMake(FilePath,StateNodeName);
        }
    }

    public override void MakeScriptEdit(GUIStyle style)
    {
        void fnMake(string path, string nodeName, string nodeBaseName, string sourceCode, string nameSpace)
        {
            #region make node script
            var nodeFilePath = path + "/" + nodeName + ".cs";
            var nodeCode = sourceCode.Replace(@"#NODE_NAME#", nodeName).Replace(@"#NODE_BASE#", nodeBaseName).Replace(@"#NAME_SPACE#", nameSpace);
            File.WriteAllText(nodeFilePath, nodeCode);
            #endregion

            AssetDatabase.Refresh();
        }

        EditorGUILayout.LabelField("Make Script", style);

        NodeFormat = EditorGUILayout.ObjectField("StateNodeSettings", NodeFormat, typeof(MakeStateNodeSettings), true) as MakeStateNodeSettings;

        StateNodeName = EditorGUILayout.TextField("StateNode Name", StateNodeName);

        if (GUILayout.Button("Path"))
            FilePath = EditorUtility.OpenFolderPanel("Choice StateNode Script Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(FilePath);

        if (GUILayout.Button("Make"))
        {
            if (!string.IsNullOrEmpty(FilePath))
            {
                fnMake(
                    FilePath,
                    StateNodeName,
                    NodeFormat.StateNodeBaseSettings.StateNodeBaseName, NodeFormat.SourceCode.text,
                    NodeFormat.StateNodeBaseSettings.NameSpace);
            }
        }
    }
}