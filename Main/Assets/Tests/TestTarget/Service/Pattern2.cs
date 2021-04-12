using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.Locator;

public interface IPattern2
{
    void Print();
}

[AutoRegistOnPrefabScript(typeof(IPattern2))]
public class Pattern2 : MonoBehaviour,IPattern2
{
    public void Print()
    {
        Debug.Log( $"{this.name} is patter2 service");
    }
}
