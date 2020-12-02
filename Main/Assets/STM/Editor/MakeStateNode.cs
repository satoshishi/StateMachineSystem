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
    public class Info
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

        public int TypeIndex
        {
            get;
            set;
        } = 0;

        public string NameSpace
        {
            get;
            set;
        } = "";
    }

    Info ScriptInfo
    {
        get;
        set;
    } = new Info();

    Info PrefabInfo
    {
        get;
        set;
    } = new Info();

    TemplateSetting NodeFormat
    {
        get;
        set;
    } = null;

    TemplateSetting ControllerFormat
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

        RefreshScirptTypes();

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
        PrefabInfo.TypeIndex = EditorGUILayout.Popup("Prefab Name", PrefabInfo.TypeIndex, ScriptTypes);

        if (GUILayout.Button("Path"))
            PrefabInfo.FilePath = EditorUtility.OpenFolderPanel("Choice Prefab Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(PrefabInfo.FilePath);

        if (GUILayout.Button("Make"))
        {
            if (!string.IsNullOrEmpty(PrefabInfo.FilePath))
            {
                PrefabInfo.StateNodeName = ScriptTypes[PrefabInfo.TypeIndex];
                fnMake(PrefabInfo.FilePath, PrefabInfo.StateNodeName);
            }
        }
    }

    public override void MakeScriptEdit(GUIStyle style)
    {
        void fnMake(string path, string nodeName, string nodeBaseName, string nodeTemplate, string controllerTemplate, string nameSpace)
        {
            #region make node script
            var nodeFilePath = path + "/" + nodeName + ".cs";
            var nodeCode = nodeTemplate.Replace(@"#NODE_NAME#", nodeName).Replace(@"#NODE_BASE#", nodeBaseName).Replace(@"#NAME_SPACE#", nameSpace);
            File.WriteAllText(nodeFilePath, nodeCode);
            #endregion

            #region make controller script

            if (!controllerTemplate.Equals(""))
            {
                var controllerFilePath = path + "/" + nodeName + ".Controller" + ".cs";
                var controllerCode = controllerTemplate.Replace(@"#NODE_NAME#", nodeName).Replace(@"#NODE_BASE#", nodeBaseName).Replace(@"#NAME_SPACE#", nameSpace);
                File.WriteAllText(controllerFilePath, controllerCode);
            }
            #endregion

            AssetDatabase.Refresh();
        }

        EditorGUILayout.LabelField("Make Script", style);

        NodeFormat = EditorGUILayout.ObjectField("NodeTemplateSettings", NodeFormat, typeof(TemplateSetting), true) as TemplateSetting;
        ControllerFormat = EditorGUILayout.ObjectField("ControllerTemplateSettings", ControllerFormat, typeof(TemplateSetting), true) as TemplateSetting;

        ScriptInfo.StateNodeName = EditorGUILayout.TextField("StateNode Name", ScriptInfo.StateNodeName);
        ScriptInfo.NameSpace = EditorGUILayout.TextField("Name Space", ScriptInfo.NameSpace);
        ScriptInfo.TypeIndex = EditorGUILayout.Popup("NodeBase Name", ScriptInfo.TypeIndex, ScriptTypes);

        if (GUILayout.Button("Path"))
            ScriptInfo.FilePath = EditorUtility.OpenFolderPanel("Choice StateNode Script Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(ScriptInfo.FilePath);

        if (GUILayout.Button("Make"))
        {
            if (!string.IsNullOrEmpty(ScriptInfo.FilePath) && !ScriptInfo.StateNodeName.Equals("") && NodeFormat != null && !NodeFormat.Script.Equals(""))
            {
                var nodeBaseName = ScriptTypes[ScriptInfo.TypeIndex];
                fnMake(
                    ScriptInfo.FilePath,
                    ScriptInfo.StateNodeName,
                    nodeBaseName, NodeFormat.Script,
                    ControllerFormat != null ? ControllerFormat.Script : "",
                    ScriptInfo.NameSpace);
            }
        }
    }
}