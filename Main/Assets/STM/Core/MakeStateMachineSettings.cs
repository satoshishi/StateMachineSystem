using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM.Param;

[CreateAssetMenu(menuName = "STM_Editor/MakeStateMachineSettings")]
public class MakeStateMachineSettings : ScriptableObject
{
    public TextAsset SourceCode;

    public MakeStateNodeBaseSettings StateNodeBaseSettings;

    public GameObject FirstTransitionStateNode;
}
