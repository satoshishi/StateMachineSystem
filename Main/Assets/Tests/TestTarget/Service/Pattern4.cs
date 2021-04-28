using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.Locator;


[AutoInstallAttribute(typeof(Pattern4))]
public class Pattern4 : MonoBehaviour
{
    public void Print()
    {
        Debug.Log( $"{this.name} is patter4 service");
    }
}