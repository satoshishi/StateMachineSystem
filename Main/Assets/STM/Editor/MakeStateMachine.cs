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
    public class Info
    {
        public string StateMachineName
        {
            get;
            set;
        } = "";

        public int NodeBaseNameIndex
        {
            get;
            set;
        } = 0;

        public int FirstNodeNameIndex
        {
            get;
            set;
        } = 0;

        public string FilePath
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

    TemplateSetting Format
    {
        get;
        set;
    } = null;

    StateMachineSettings Settings
    {
        get;
        set;
    } = null;

    [MenuItem("STM Editor/Make StateMachine")]
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
        PrefabInfo.NodeBaseNameIndex = EditorGUILayout.Popup("Prefab Name", PrefabInfo.NodeBaseNameIndex, ScriptTypes);

        if (GUILayout.Button("Path"))
            PrefabInfo.FilePath = EditorUtility.OpenFolderPanel("Choice Prefab Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(PrefabInfo.FilePath);

        if (GUILayout.Button("Make"))
        {
            if (!string.IsNullOrEmpty(PrefabInfo.FilePath))
                fnMake(PrefabInfo.FilePath, ScriptTypes[PrefabInfo.NodeBaseNameIndex]);
        }
    }

    public override void MakeScriptEdit(GUIStyle style)
    {
        void fnMake(string path, string nodeName, string nodeBaseName, string firstNodeName, string template)
        {
            var filePath = path + "/" + nodeName + ".cs";
            var code = template.Replace(@"#STM_NAME#", nodeName).Replace(@"#NODE_BASE#", nodeBaseName).Replace(@"#FIRST_NODE#", firstNodeName);
            File.WriteAllText(filePath, code);
            AssetDatabase.Refresh();
        }

        EditorGUILayout.LabelField("Make Script", style);

        Format = EditorGUILayout.ObjectField("TemplateSettings", Format, typeof(TemplateSetting), true) as TemplateSetting;

        ScriptInfo.StateMachineName = EditorGUILayout.TextField("StateMachine Name", ScriptInfo.StateMachineName);
        ScriptInfo.NodeBaseNameIndex = EditorGUILayout.Popup("NodeBase Name", ScriptInfo.NodeBaseNameIndex, ScriptTypes);
        ScriptInfo.FirstNodeNameIndex = EditorGUILayout.Popup("FristNode Name", ScriptInfo.FirstNodeNameIndex, ScriptTypes);

        if (GUILayout.Button("Path"))
            ScriptInfo.FilePath = EditorUtility.OpenFolderPanel("Choice StateNode Script Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(ScriptInfo.FilePath);

        if (GUILayout.Button("Make"))
        {
            if (!string.IsNullOrEmpty(ScriptInfo.FilePath) && !ScriptInfo.StateMachineName.Equals("") && Format != null && !Format.Script.Equals(""))
                fnMake(ScriptInfo.FilePath, ScriptInfo.StateMachineName, ScriptTypes[ScriptInfo.NodeBaseNameIndex] , ScriptTypes[ScriptInfo.FirstNodeNameIndex], Format.Script);
        }
    }

}
