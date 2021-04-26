using UnityEditor;

namespace StateMachineService.Editor
{
    public partial class MakeStateMachineService : EditorWindow
    {
        IScriptCreateService CreateScriptService(MakeType type)
        {
            IScriptCreateService scriptService = null;

            switch (type)
            {
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
                            STMScriptTemp = stateMachineEditor.STMScriptTemp,
                            InstallerScriptTemp = stateMachineEditor.InstallerScriptTemp,
                            ServiceName = stateMachineEditor.ServiceName,
                            FirstStateGameObject = stateMachineEditor.FirstStateGameObject
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
                case MakeType.StateNode:
                    var stateNodeEditor = Editor as StateNodeServiceEditorSettings;
                    prefabService = new StateNodeServicePrefabCreateService()
                    {
                        ThisCommand = new StateNodeServicePrefabCreateService.Command()
                        {
                            FilePath = stateNodeEditor.PrefabPath,
                            Settings = this.Settings,
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
                            Settings = Settings,
                            ServiceName = stateMachineEditor.ServiceName,
                            FirstStateGameObject = stateMachineEditor.FirstStateGameObject,
                            prefabInstallSettings = stateMachineEditor.prefabInstallSettings
                        }
                    };
                    break;                    
            }

            return prefabService;
        }
    }
}
