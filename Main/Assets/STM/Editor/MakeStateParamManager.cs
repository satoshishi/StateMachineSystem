﻿using System.Collections;
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
using STM.Param;

public class MakeStateParamManager : STMEditor
{
    public class Info
    {
        public string ParamManagerName
        {
            get;
            set;
        } = "";

        public string ParamTypeName
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

        public string ResourcesFolderName
        {
            get;
            set;
        } = "";

        public string ResourcesFolderPath
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

    [MenuItem("Tools/STM Editor/Make StateParamManager")]
    private static void Create()
    {
        GetWindow<MakeStateParamManager>("Make StateParamManager");
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
                var obj = Resources.Load<GameObject>("ParamManagerTemp");
                var game = PrefabUtility.InstantiateAttachedAsset(obj) as GameObject;
                game.AddComponent(type);

                var prefab = PrefabUtility.SaveAsPrefabAsset(game, path + "/" + prefabName + ".prefab");

                if (Settings != null)
                {
                    if(Settings.ParamManagers == null)
                        Settings.ParamManagers = new List<StateParameter>();
                        
                    Settings.ParamManagers.Add(prefab.GetComponent<StateParameter>());
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
                fnMake(PrefabInfo.FilePath, ScriptTypes[PrefabInfo.TypeIndex]);
            }
        }
    }

    public override void MakeScriptEdit(GUIStyle style)
    {
        void fnMake(string path, string paramName, string paramTypeName, string template, string nameSpace, string resoucesFolderName, string resourcesFolderPath)
        {
            var filePath = path + "/" + paramName + ".cs";
            var code = template.Replace(@"#PARAM_NAME#", paramName).Replace(@"#PARAM_TYPE#", paramTypeName).Replace(@"#NAME_SPACE#", nameSpace).Replace(@"#RESOURCES_PATH#", resoucesFolderName);
            File.WriteAllText(filePath, code);

            if (!string.IsNullOrEmpty(resourcesFolderPath) && Directory.Exists(resourcesFolderPath + "/" + resoucesFolderName))
            {
                AssetDatabase.Refresh();
                return;
            }
            Directory.CreateDirectory(resourcesFolderPath + "/" + resoucesFolderName);
            AssetDatabase.Refresh();

        }

        EditorGUILayout.LabelField("Make Script", style);

        Format = EditorGUILayout.ObjectField("TemplateSettings", Format, typeof(TemplateSetting), true) as TemplateSetting;

        ScriptInfo.ParamManagerName = EditorGUILayout.TextField("ParamManager Name", ScriptInfo.ParamManagerName);
        ScriptInfo.ParamTypeName = EditorGUILayout.TextField("ParamType Name", ScriptInfo.ParamTypeName);
        ScriptInfo.NameSpace = EditorGUILayout.TextField("Name Space", ScriptInfo.NameSpace);

        if (GUILayout.Button("Script Path"))
            ScriptInfo.FilePath = EditorUtility.OpenFolderPanel("Choice StateNode Script Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(ScriptInfo.FilePath);

        ScriptInfo.ResourcesFolderName = EditorGUILayout.TextField("Resources Folder Name", ScriptInfo.ResourcesFolderName);
        if (GUILayout.Button("Folder Path"))
            ScriptInfo.ResourcesFolderPath = EditorUtility.OpenFolderPanel("Choice Resources Folder Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(ScriptInfo.ResourcesFolderPath);

        if (GUILayout.Button("Make"))
        {
            if (!string.IsNullOrEmpty(ScriptInfo.FilePath) && !ScriptInfo.ParamManagerName.Equals("") && !ScriptInfo.ParamTypeName.Equals("") && Format != null && !Format.Script.Equals("")
            && !string.IsNullOrEmpty(ScriptInfo.ResourcesFolderName))
            {
                fnMake(ScriptInfo.FilePath, ScriptInfo.ParamManagerName, ScriptInfo.ParamTypeName, Format.Script, ScriptInfo.NameSpace, ScriptInfo.ResourcesFolderName, string.IsNullOrEmpty(ScriptInfo.ResourcesFolderPath) ? "" : ScriptInfo.ResourcesFolderPath);
            }
        }
    }
}
