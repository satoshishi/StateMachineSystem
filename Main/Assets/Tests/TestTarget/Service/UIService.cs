using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIService : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    public float Value
    {
        get => slider.value;
        set => slider.value = value;
    }
}
