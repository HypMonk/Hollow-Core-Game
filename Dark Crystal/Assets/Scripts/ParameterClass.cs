using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterClass
{
    string _parameterName;
    float _defaultValue, _testValue;

    public ParameterClass(string parameterName, float defaultValue, float testValue)
    {
        _parameterName = parameterName;
        _defaultValue = defaultValue;
        _testValue = testValue;
    }

    public string ParameterName { get { return _parameterName; } }
    public float DefaultValue { get { return _defaultValue; } }
    public float TestValue { get { return _testValue; } set { _testValue = value; } }
}
