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
using STM.Param;

public class MakeStateParamManager : STMEditor
{
    public string FilePath
    {
        get;
        set;
    } = "";

    public string ResourcesFolderPath
    {
        get;
        set;
    } = "";

    MakeStateParamManagerSettings Format
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
                    if (Settings.ParamManagers == null)
                        Settings.ParamManagers = new List<StateMachineParamManager>();

                    Settings.ParamManagers.Add(prefab.GetComponent<StateMachineParamManager>());
                    EditorUtility.SetDirty(Settings);
                    AssetDatabase.SaveAssets();
                }

                UnityEngine.Object.DestroyImmediate(game);
                AssetDatabase.SaveAssets();
            }
        }

        EditorGUILayout.LabelField("Make Prefab", style);

        Format = EditorGUILayout.ObjectField("TemplateSettings", Format, typeof(MakeStateParamManagerSettings), true) as MakeStateParamManagerSettings;
        Settings = EditorGUILayout.ObjectField("Target STM Settings", Settings, typeof(StateMachineSettings), true) as StateMachineSettings;

        if (GUILayout.Button("Path"))
            FilePath = EditorUtility.OpenFolderPanel("Choice Prefab Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(FilePath);

        if (GUILayout.Button("Make"))
        {
            if (!string.IsNullOrEmpty(FilePath))
            {
                fnMake(FilePath,Format.ParamManagerName);
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

            if(!string.IsNullOrEmpty(resoucesFolderName)   && 
               !string.IsNullOrEmpty(resourcesFolderPath) &&
               !Directory.Exists(resourcesFolderPath + "/" + resoucesFolderName))
               {
                    Directory.CreateDirectory(resourcesFolderPath + "/" + resoucesFolderName);                   
               }
            AssetDatabase.Refresh();
        }

        EditorGUILayout.LabelField("Make Script", style);

        Format = EditorGUILayout.ObjectField("TemplateSettings", Format, typeof(MakeStateParamManagerSettings), true) as MakeStateParamManagerSettings;

        if (GUILayout.Button("Script Path"))
            FilePath = EditorUtility.OpenFolderPanel("Choice StateNode Script Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(FilePath);

        if (GUILayout.Button("Folder Path"))
            ResourcesFolderPath = EditorUtility.OpenFolderPanel("Choice Resources Folder Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(ResourcesFolderPath);

        if (GUILayout.Button("Make"))
        {
            if (!string.IsNullOrEmpty(FilePath))
            {
                fnMake(FilePath, Format.ParamManagerName, Format.ParamTypeName, Format.SourceCode.text, Format.NameSpace, Format.ResourcesFolderName, ResourcesFolderPath);
            }
        }
    }
}
