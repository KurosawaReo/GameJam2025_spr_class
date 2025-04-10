using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using GetCondFunc = System.Func<UnityEditor.SerializedProperty, ConditionalDisableInInspectorAttribute, bool>;

[CustomPropertyDrawer(typeof(ConditionalDisableInInspectorAttribute))]
internal sealed class ConditionalDisableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = base.attribute as ConditionalDisableInInspectorAttribute;
        var condProp = property.serializedObject.FindProperty(attr.VariableName);
        if (condProp == null)
        {
            Debug.LogError($"Not found '{attr.VariableName}' property");
            EditorGUI.PropertyField(position, property, label, true);
        }

        var isDisable = IsDisable(attr, condProp);
        if (attr.ConditionalInvisible && isDisable)
        {
            return;
        }
        EditorGUI.BeginDisabledGroup(isDisable);
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.EndDisabledGroup();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attr = base.attribute as ConditionalDisableInInspectorAttribute;
        var prop = property.serializedObject.FindProperty(attr.VariableName);
        if (attr.ConditionalInvisible && IsDisable(attr, prop))
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
        return EditorGUI.GetPropertyHeight(property, true);
    }

    private bool IsDisable(ConditionalDisableInInspectorAttribute attr, SerializedProperty prop)
    {
        GetCondFunc disableCondFunc;
        if (!DisableCondFuncMap.TryGetValue(attr.VariableType, out disableCondFunc))
        {
            Debug.LogError($"{attr.VariableType} type is not supported");
            return false;
        }
        return disableCondFunc(prop, attr);
    }

    private Dictionary<Type, GetCondFunc> DisableCondFuncMap = new Dictionary<Type, GetCondFunc>() {
        { typeof(bool), (prop, attr) => {return attr.TrueThenDisable ? !prop.boolValue : prop.boolValue;} },
        { typeof(string), (prop, attr) => {return attr.TrueThenDisable ? prop.stringValue == attr.ComparedStr : prop.stringValue != attr.ComparedStr;} },
        { typeof(int), (prop, attr) => {return attr.TrueThenDisable ? prop.intValue == attr.ComparedInt : prop.intValue != attr.ComparedInt;} },
        { typeof(float), (prop, attr) => {return attr.TrueThenDisable ? prop.floatValue <= attr.ComparedFloat : prop.floatValue > attr.ComparedFloat;} }
    };
}

//gist77dd59ad1c764387ec5fca3c185e04c2
//https://mu-777.hatenablog.com/entry/2022/09/04/113850#Attribute%E3%81%A8Editor%E6%8B%A1%E5%BC%B5%E3%81%AE%E3%82%AF%E3%83%A9%E3%82%B9%E3%81%AE%E4%BD%9C%E6%88%90
//‚Ï‚­‚è
