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
        IScriptCreateService CreateScriptService(MakeType type)
        {
            IScriptCreateService scriptService = null;

            switch (type)
            {
                case MakeType.Repository:
                    var repositoryEditor = Editor as StateParameterRepositoryEditorSettings;
                    scriptService = new StateParameterRepositoryScriptCreateService()
                    {
                        ThisCommand = new StateParameterRepositoryScriptCreateService.Command()
                        {
                            FilePath = repositoryEditor.ScriptPath,
                            ScriptTemp = repositoryEditor.ScriptTemp,
                            ServiceName = repositoryEditor.ServiceName,
                            STMSettings = this.STMSettings
                        }
                    };
                    break;

                case MakeType.StateNode:
                    var stateNodeEditor = Editor as StateNodeServiceEditorSettings;
                    scriptService = new StateNodeServiceScriptCreateService()
                    {
                        ThisCommand = new StateNodeServiceScriptCreateService.Command()
                        {
                            FilePath = stateNodeEditor.ScriptPath,
                            ScriptTemp = stateNodeEditor.ScriptTemp,
                            ServiceName = stateNodeEditor.ServiceName,
                            StateNames = stateNodeEditor.StateNames
                        }
                    };
                    break;

                case MakeType.StateMachine:
                    var stateMachineEditor = Editor as StateMachineServiceEditorSettings;
                    scriptService = new StateMachineServiceScriptCreateService()
                    {
                        ThisCommand = new StateMachineServiceScriptCreateService.Command()
                        {
                            FilePath = stateMachineEditor.ScriptPath,
                            ScriptTemp = stateMachineEditor.ScriptTemp,
                            ServiceName = stateMachineEditor.ServiceName,
                            FirstStateGameObject = stateMachineEditor.FirstStateGameObject
                        }
                    };
                    break;

                case MakeType.StateParameter:
                    var stateParameterEditor = Editor as StateParameterEditorSettings;
                    scriptService = new StateParameterScriptCreateService()
                    {
                        ThisCommand = new StateParameterScriptCreateService.Command()
                        {
                            EntityBaseScriptTemp = stateParameterEditor.EntityBaseScriptTemp,
                            RepositoryScriptTemp = stateParameterEditor.RepositoryScriptTemp,
                            ServiceName = stateParameterEditor.ServiceName,
                            FilePath = stateParameterEditor.ScriptPath,
                            EntityName = stateParameterEditor.EntityName       
                        }
                    };
                    break;                                        
            }

            return scriptService;
        }

        IPrefabCreateService CreatePrefabService(MakeType type)
        {
            IPrefabCreateService prefabService = null;

            switch (type)
            {
                case MakeType.Repository:
                    var repositoryEditor = Editor as StateParameterRepositoryEditorSettings;
                    prefabService = new StateParameterRepositoryPrefabCreateService()
                    {
                        ThisCommand = new StateParameterRepositoryPrefabCreateService.Command()
                        {
                            FilePath = repositoryEditor.ScriptPath,
                            ServiceName = repositoryEditor.ServiceName,
                            STMSettings = this.STMSettings
                        }
                    };
                    break;

                case MakeType.StateNode:
                    var stateNodeEditor = Editor as StateNodeServiceEditorSettings;
                    prefabService = new StateNodeServicePrefabCreateService()
                    {
                        ThisCommand = new StateNodeServicePrefabCreateService.Command()
                        {
                            FilePath = stateNodeEditor.PrefabPath,
                            STMSettings = this.STMSettings,
                            StateNames = stateNodeEditor.StateNames
                        }
                    };
                    break;

                case MakeType.StateMachine:
                    var stateMachineEditor = Editor as StateMachineServiceEditorSettings;
                    prefabService = new StateMachineServicePrefabCreateService()
                    {
                        ThisCommand = new StateMachineServicePrefabCreateService.Command()
                        {
                            FilePath = stateMachineEditor.PrefabPath,
                            STMSettings = this.STMSettings,
                            ServiceName = stateMachineEditor.ServiceName
                        }
                    };
                    break;                    

                case MakeType.StateParameter:
                    var stateParameterEditor = Editor as StateParameterEditorSettings;
                    prefabService = new StateParameterPrefabCreateService()
                    {
                        ThisCommand = new StateParameterPrefabCreateService.Command()
                        {
                            ServiceName = stateParameterEditor.ServiceName,
                            FilePath = stateParameterEditor.ScriptPath,
                            EntityName = stateParameterEditor.EntityName,
                            STMSettings = this.STMSettings
                        }
                    };
                    break;                                        
            }

            return prefabService;
        }
    }
}
