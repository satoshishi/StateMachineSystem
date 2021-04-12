using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StateMachineService.Locator;

[AutoRegistOnPrefabScript(typeof(Pattern1))]
public class Pattern1 : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    public float Value
    {
        get => slider.value;
        set => slider.value = value;
    }
}
