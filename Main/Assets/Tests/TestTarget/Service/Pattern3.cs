using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineService.Locator;

[AutoInstallAttribute(typeof(Pattern3))]
public class Pattern3 : MonoBehaviour
{
    public void Print()
    {
        Debug.Log( $"{this.name} is patter3 service");
    }
}
