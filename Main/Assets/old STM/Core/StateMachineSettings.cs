using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM.Param;

[CreateAssetMenu(menuName = "STM_Editor/StateMachineSettings")]
public class StateMachineSettings : ScriptableObject
{
    public List<StateNodeBase> StateNode;

    public List<StateMachineParamManager> ParamManagers;
}
