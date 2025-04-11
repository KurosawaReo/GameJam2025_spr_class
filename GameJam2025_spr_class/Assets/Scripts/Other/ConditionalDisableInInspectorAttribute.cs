using System;
using UnityEngine;

public partial class ConditionalDisableInInspectorAttribute : PropertyAttribute
{
    public readonly string VariableName;
    public readonly Type VariableType;
    public readonly bool TrueThenDisable;
    public readonly bool ConditionalInvisible;

    public readonly string ComparedStr;
    public readonly int ComparedInt;
    public readonly float ComparedFloat;

    private ConditionalDisableInInspectorAttribute(string variableName, Type variableType, bool trueThenDisable = false, bool conditionalInvisible = false)
    {
        this.VariableName = variableName;
        this.VariableType = variableType;
        this.TrueThenDisable = trueThenDisable;
        this.ConditionalInvisible = conditionalInvisible;
    }

    public ConditionalDisableInInspectorAttribute(string boolVariableName, bool trueThenDisable = false, bool conditionalInvisible = false)
    : this(boolVariableName, typeof(bool), trueThenDisable, conditionalInvisible) { }

    public ConditionalDisableInInspectorAttribute(string strVariableName, string comparedStr, bool notEqualThenEnable = false, bool conditionalInvisible = false)
    : this(strVariableName, comparedStr.GetType(), notEqualThenEnable, conditionalInvisible)
    {
        this.ComparedStr = comparedStr;
    }

    public ConditionalDisableInInspectorAttribute(string intVariableName, int comparedInt, bool notEqualThenEnable = false, bool conditionalInvisible = false)
    : this(intVariableName, comparedInt.GetType(), notEqualThenEnable, conditionalInvisible)
    {
        this.ComparedInt = comparedInt;
    }

    public ConditionalDisableInInspectorAttribute(string floatVariableName, float comparedFloat, bool greaterThanComparedThenEnable = true, bool conditionalInvisible = false)
    : this(floatVariableName, comparedFloat.GetType(), greaterThanComparedThenEnable, conditionalInvisible)
    {
        this.ComparedFloat = comparedFloat;
    }
}

//gist77dd59ad1c764387ec5fca3c185e04c2
//https://mu-777.hatenablog.com/entry/2022/09/04/113850#Attribute%E3%81%A8Editor%E6%8B%A1%E5%BC%B5%E3%81%AE%E3%82%AF%E3%83%A9%E3%82%B9%E3%81%AE%E4%BD%9C%E6%88%90
//‚Ï‚­‚è