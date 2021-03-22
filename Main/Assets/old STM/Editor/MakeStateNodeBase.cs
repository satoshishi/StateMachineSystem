using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using STM.STN;
using UnityEditor.Callbacks;
using System;
using System.Reflection;

public class MakeStateNodeBase : STMEditor
{
    MakeStateNodeBaseSettings Format
    {
        get;
        set;
    } = null;

    public string NodeBaseName
    {
        get;
        set;
    } = "";

    public string NameSpace
    {
        get;
        set;
    } = "";

    public string FilePath
    {
        get;
        set;
    } = "";

    [MenuItem("Tools/STM Editor/Make StateNodeBase")]
    private static void Create()
    {
        GetWindow<MakeStateNodeBase>("Make StateNodeBase");
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        GUIStyleState state = new GUIStyleState();
        state.textColor = Color.white;
        style.normal = state;

        MakeScriptEdit(style);
    }
    public override void MakeScriptEdit(GUIStyle style)
    {
        void fnMake(string path, string nodeBaseName, string template, string nameSpace)
        {
            var filePath = path + "/" + nodeBaseName + ".cs";
            var code = template.Replace(@"#NODE_BASE#", nodeBaseName).Replace(@"#NAME_SPACE#", nameSpace);
            File.WriteAllText(filePath, code);
            AssetDatabase.Refresh();
        }

        Format = EditorGUILayout.ObjectField("TemplateSettings", Format, typeof(MakeStateNodeBaseSettings), true) as MakeStateNodeBaseSettings;

        if (GUILayout.Button("Path"))
            FilePath = EditorUtility.OpenFolderPanel("Choice StateNode Script Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(FilePath);

        if (GUILayout.Button("Make"))
        {
            if (!string.IsNullOrEmpty(FilePath))
                fnMake(FilePath, Format.StateNodeBaseName, Format.SourceCode.text, Format.NameSpace);
        }
    }
}