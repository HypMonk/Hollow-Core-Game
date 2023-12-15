using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameParameters : MonoBehaviour
{
    public Dictionary<string, ParameterClass> playerParameters = new Dictionary<string, ParameterClass>();
    public Dictionary<string, ParameterClass> crystalParameters = new Dictionary<string, ParameterClass>();
    public Dictionary<string, ParameterClass> baseEnemyParameters = new Dictionary<string, ParameterClass>();
    public Dictionary<string, ParameterClass> lightSuckerParameters = new Dictionary<string, ParameterClass>();
    public Dictionary<string, ParameterClass> tankParameters = new Dictionary<string, ParameterClass>();
    public Dictionary<string, ParameterClass> flyerParameters = new Dictionary<string, ParameterClass>();

    private void Awake()
    {
        string playerParameterSheet = Application.persistentDataPath + "/Parameter Sheets/Dark Crystal Player Parameter Sheet.txt";
        playerParameters = GameObject.FindGameObjectWithTag("ParameterReader").GetComponent<ParameterReader>().ParseFile(playerParameterSheet);

        string crystalParameterSheet = Application.persistentDataPath + "/Parameter Sheets/Dark Crystal Crystal Parameter Sheet.txt";
        crystalParameters = GameObject.FindGameObjectWithTag("ParameterReader").GetComponent<ParameterReader>().ParseFile(crystalParameterSheet);

        string baseEnemyParameterSheet = Application.persistentDataPath + "/Parameter Sheets/Dark Crystal Base Enemy Parameter Sheet.txt";
        baseEnemyParameters = GameObject.FindGameObjectWithTag("ParameterReader").GetComponent<ParameterReader>().ParseFile(baseEnemyParameterSheet);

        string lightSuckerParameterSheet = Application.persistentDataPath + "/Parameter Sheets/Dark Crystal Light Sucker Parameter Sheet.txt";
        lightSuckerParameters = GameObject.FindGameObjectWithTag("ParameterReader").GetComponent<ParameterReader>().ParseFile(lightSuckerParameterSheet);

        string tankParameterSheet = Application.persistentDataPath + "/Parameter Sheets/Dark Crystal Tank Parameter Sheet.txt";
        tankParameters = GameObject.FindGameObjectWithTag("ParameterReader").GetComponent<ParameterReader>().ParseFile(tankParameterSheet);

        string flyerParameterSheet = Application.persistentDataPath + "/Parameter Sheets/Dark Crystal Flyer Parameter Sheet.txt";
        flyerParameters = GameObject.FindGameObjectWithTag("ParameterReader").GetComponent<ParameterReader>().ParseFile(flyerParameterSheet);

    }

    public void UpdateAllParameterSheets()
    {
        string playerParameterSheet = Application.persistentDataPath + "/Parameter Sheets/Dark Crystal Player Parameter Sheet.txt";
        playerParameters = GameObject.FindGameObjectWithTag("ParameterReader").GetComponent<ParameterReader>().ParseFile(playerParameterSheet);

        string crystalParameterSheet = Application.persistentDataPath + "/Parameter Sheets/Dark Crystal Crystal Parameter Sheet.txt";
        crystalParameters = GameObject.FindGameObjectWithTag("ParameterReader").GetComponent<ParameterReader>().ParseFile(crystalParameterSheet);

        string baseEnemyParameterSheet = Application.persistentDataPath + "/Parameter Sheets/Dark Crystal Base Enemy Parameter Sheet.txt";
        baseEnemyParameters = GameObject.FindGameObjectWithTag("ParameterReader").GetComponent<ParameterReader>().ParseFile(baseEnemyParameterSheet);

        string lightSuckerParameterSheet = Application.persistentDataPath + "/Parameter Sheets/Dark Crystal Light Sucker Parameter Sheet.txt";
        lightSuckerParameters = GameObject.FindGameObjectWithTag("ParameterReader").GetComponent<ParameterReader>().ParseFile(lightSuckerParameterSheet);

        string tankParameterSheet = Application.persistentDataPath + "/Parameter Sheets/Dark Crystal Tank Parameter Sheet.txt";
        tankParameters = GameObject.FindGameObjectWithTag("ParameterReader").GetComponent<ParameterReader>().ParseFile(tankParameterSheet);

        string flyerParameterSheet = Application.persistentDataPath + "/Parameter Sheets/Dark Crystal Flyer Parameter Sheet.txt";
        flyerParameters = GameObject.FindGameObjectWithTag("ParameterReader").GetComponent<ParameterReader>().ParseFile(flyerParameterSheet);
    }
}
