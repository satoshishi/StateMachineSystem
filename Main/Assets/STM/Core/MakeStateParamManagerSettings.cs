using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM.Param;

[CreateAssetMenu(menuName = "STM_Editor/MakeStateParamManagerSettings")]
public class MakeStateParamManagerSettings : ScriptableObject
{
    public TextAsset SourceCode;

    public string ParamManagerName;

    public string ParamTypeName;

    public string NameSpace;

    public string ResourcesFolderName;
}
