using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM.Param;

[CreateAssetMenu(menuName = "STM_Editor/MakeStateNodeBaseSettings")]
public class MakeStateNodeBaseSettings : ScriptableObject
{
    public TextAsset SourceCode;

    public string NameSpace;

    public string StateNodeBaseName;
}
