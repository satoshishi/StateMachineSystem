using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM.Param;

[CreateAssetMenu(menuName = "STM_Editor/MakeStateNodeSettings")]
public class MakeStateNodeSettings : ScriptableObject
{
    public TextAsset SourceCode;

    public MakeStateNodeBaseSettings StateNodeBaseSettings;
}
