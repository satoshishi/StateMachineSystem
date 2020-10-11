using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TemplateSettings")]
public class TemplateSetting : ScriptableObject
{
    [TextArea(1,500)]
    public string Script;
}
