using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM.Param;

[CreateAssetMenu(menuName = "StateMachineSettings")]
public class StateMachineSettings : ScriptableObject
{
    public List<StateNodeBase> StateNode;

    public List<StateParameter> ParamManagers;
}
