using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif 

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
public sealed class G20_EnumFlagsAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(G20_EnumFlagsAttribute))]
public sealed class G20_EnumFlagsAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(
        Rect position,
        SerializedProperty prop,
        GUIContent label
    )
    {
        prop.intValue = EditorGUI.MaskField(
            position,
            label,
            prop.intValue,
            prop.enumNames
        );
    }
}
#endif