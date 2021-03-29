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

    public partial class MakeStateMachineService : EditorWindow
    {
        IStateMachineServiceEditor CreateEditor(MakeType type)
        {
            IStateMachineServiceEditor editor = null;

            switch(type)
            {
                case MakeType.StateNode:
                    editor = Resources.Load<StateNodeServiceEditorSettings>("StateNodeServiceEditorSettings");
                break;                

                case MakeType.StateMachine:
                    editor = Resources.Load<StateMachineServiceEditorSettings>("StateMachineServiceEditorSettings");
                break;                      

                case MakeType.Service:
                    editor = Resources.Load<ServiceEditorSettings>("ServiceEditorSettings");
                break;                                                
            }

            return editor;
        }
    }
}
