using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ParameterReader : MonoBehaviour
{
    public static ParameterReader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    public Dictionary<string,ParameterClass> ParseFile(string parameterFilePath)
    {
        Dictionary<string, ParameterClass> parameterList = new Dictionary<string, ParameterClass>();
        StreamReader reader = new StreamReader(parameterFilePath);
        string[] data = reader.ReadToEnd().Split(new char[] { '\n' });
        for (int i = 2; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            string tag = row[0];
            string parameterName = row[2];
            float defaultValue = float.Parse(row[3]);
            float testValue = float.Parse(row[4]);
            parameterList.Add(tag, new ParameterClass(parameterName, defaultValue, testValue));
        }
        reader.Close();
        return parameterList;
    }
}
