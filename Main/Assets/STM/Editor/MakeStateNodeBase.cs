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
    TemplateSetting Format
    {
        get;
        set;
    } = null;

    public string NodeBaseName
    {
        get;
        set;
    } = "";

    public string FilePath
    {
        get;
        set;
    } = "";    

    [MenuItem("STM Editor/Make StateNodeBase")]
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
        void fnMake(string path, string nodeBaseName, string template)
        {
            var filePath = path + "/" + nodeBaseName + ".cs";
            var code = template.Replace(@"#NODE_BASE#", nodeBaseName);
            File.WriteAllText(filePath, code);
            AssetDatabase.Refresh();
        }

        Format = EditorGUILayout.ObjectField("TemplateSettings", Format, typeof(TemplateSetting), true) as TemplateSetting;
        NodeBaseName = EditorGUILayout.TextField("NodeBase Name", NodeBaseName);

        if (GUILayout.Button("Path"))
            FilePath = EditorUtility.OpenFolderPanel("Choice StateNode Script Path", Application.dataPath, string.Empty);
        EditorGUILayout.LabelField(FilePath);

        if (GUILayout.Button("Make"))
        {
            if (!string.IsNullOrEmpty(FilePath) && !NodeBaseName.Equals("") && Format != null && !Format.Script.Equals(""))
                fnMake(FilePath, NodeBaseName, Format.Script);
        }        
    }

}
