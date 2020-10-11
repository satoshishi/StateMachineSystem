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

public abstract class STMEditor : EditorWindow
{
    private string[] scriptTypes = null;
    public string[] ScriptTypes
    {
        get
        {
            if(scriptTypes == null || scriptTypes.Length <= 0)
                scriptTypes = GetAllMyTypes();

            return scriptTypes;
        }
    }

    public virtual void RefreshScirptTypes()
    {
        if (GUILayout.Button("RefreshScirptTypes"))
        {
            scriptTypes = null;
            var dummy = ScriptTypes;
        }
    }

    public virtual void MakePrefabEdit(GUIStyle style)
    {

    }

    public virtual void MakeScriptEdit(GUIStyle style)
    {

    }

    public virtual bool IsContainsType(string targetTypeStr, out Type res)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Name == targetTypeStr)
                {
                    res = type;
                    return true;
                }
            }
        }

        res = default;
        return false;
    }

    //https://qiita.com/r-ngtm/items/1d341fa9551ecf6b4c24
    public virtual string[] GetAllMyTypes()
    {
        var MonoScripts = (Resources.FindObjectsOfTypeAll<MonoScript>().ToArray());

        var myTypes = MonoScripts
        .Where(script => script != null)
        .Select(script => script.GetClass())
        .Where(classType => classType != null)
        .Where(classType => classType.Module.Name == "Assembly-CSharp.dll").ToArray();

        return Array.ConvertAll(myTypes,mt=>mt.ToString()); 
    }     


}
