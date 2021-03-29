using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPrefabRepository : PrefabRepositroy
{
    public override void Print()
    {
        Debug.Log("Dummy");
    }
}
