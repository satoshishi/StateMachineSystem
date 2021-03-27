using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Callbacks;
using System;
using System.Reflection;
using StateMachineService.Settings;

namespace StateMachineService.Editor
{
    public interface IStateMachineServiceEditor
    {
        void Handle(GUIStyle style);
    }

    public class FileCreateServiceCommand
    {

    }

    public interface IScriptCreateService
    {
        void MakeScript();
    }

    public interface IPrefabCreateService
    {
        void MakePrefab();
    }

    public partial class MakeStateMachineService : EditorWindow
    {
        [MenuItem("Tools/StateMachineService/Make StateMachine")]
        private static void Create()
        {
            GetWindow<MakeStateMachineService>("Make StateMachine");
        }

        public StateMachineServiceSettings STMSettings
        {
            get;
            set;
        }

        public IStateMachineServiceEditor Editor
        {
            get;
            set;
        }

        public enum MakeType
        {
            StateMachine,
            StateNode,
            Repository,
            StateParameter,
            None
        }

        public MakeType MType
        {
            get;
            set;
        } = MakeType.None;

        void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            GUIStyleState state = new GUIStyleState();
            state.textColor = Color.white;
            style.normal = state;

            SelectSettingsFiles(style);
            SelectMakeType(style, (type) => { Editor = CreateEditor(MType); });

            if (Editor == null && Event.current.type == EventType.Repaint)
                Editor = MType != MakeType.None ? CreateEditor(MType) : null;
            else if(Editor != null)
            {
                
                Editor.Handle(style);

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("スクリプトを生成する"))
                    CreateScriptService(MType)?.MakeScript();

                GUI.backgroundColor = Color.yellow;
                if (GUILayout.Button("プレファブを生成する"))
                    CreatePrefabService(MType)?.MakePrefab();
            }
        }

        public void SelectSettingsFiles(GUIStyle style)
        {
            EditorGUILayout.LabelField("1. ScrptableObjectを選択する", style);
            STMSettings = EditorGUILayout.ObjectField("STM Settings", STMSettings, typeof(StateMachineServiceSettings), true) as StateMachineServiceSettings;
        }

        public void SelectMakeType(GUIStyle style, Action<MakeType> onChangeMakeType)
        {
            EditorGUILayout.LabelField("2. 作成する内容を選択する", style);
            var newMakeType = (MakeType)EditorGUILayout.EnumPopup(MType);

            if (newMakeType != MType)
            {
                MType = newMakeType;
                onChangeMakeType(MType);
            }
        }

        public static bool ExistTypeInThisProject(string targetTypeStr, out Type res)
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
    }
}