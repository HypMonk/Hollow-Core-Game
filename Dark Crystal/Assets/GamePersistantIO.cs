using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GamePersistantIO : MonoBehaviour
{

    string parameterSheetsPath;
    string versionPath;

    private void Awake()
    {
        parameterSheetsPath = Application.persistentDataPath + "/Parameter Sheets/";
        versionPath = Application.persistentDataPath + "/Version/";

        if (!System.IO.Directory.Exists(versionPath))
        {
            Directory.CreateDirectory(versionPath);
        }

        if (!System.IO.File.Exists(versionPath + "Version.txt"))
        {
            WriteVersionFile();

            if (!System.IO.Directory.Exists(parameterSheetsPath))
            {
                Directory.CreateDirectory(parameterSheetsPath);
            }

            WritePlayerParameterSheet();
            WriteCrystalParameterSheet();
            WriteBaseEnemyParameterSheet();
            WriteLightSuckerParameterSheet();
            WriteTankParameterSheet();
            WriteFlyerParameterSheet();
        } else
        {

            StreamReader reader = new StreamReader(versionPath + "Version.txt");
            string savedVersionNumber = reader.ReadLine();
            if (savedVersionNumber != Application.version)
            {
                WriteVersionFile();

                if (!System.IO.Directory.Exists(parameterSheetsPath))
                {
                    Directory.CreateDirectory(parameterSheetsPath);
                }

                WritePlayerParameterSheet();
                WriteCrystalParameterSheet();
                WriteBaseEnemyParameterSheet();
                WriteLightSuckerParameterSheet();
                WriteTankParameterSheet();
                WriteFlyerParameterSheet();
            } else
            {
                if (!System.IO.Directory.Exists(parameterSheetsPath))
                {
                    Directory.CreateDirectory(parameterSheetsPath);
                }

                if (!System.IO.File.Exists(parameterSheetsPath + "Dark Crystal Player Parameter Sheet.txt"))
                {
                    WritePlayerParameterSheet();
                }

                if (!System.IO.File.Exists(parameterSheetsPath + "Dark Crystal Crystal Parameter Sheet.txt"))
                {
                    WriteCrystalParameterSheet();
                }

                if (!System.IO.File.Exists(parameterSheetsPath + "Dark Crystal Base Enemy Parameter Sheet.txt"))
                {
                    WriteBaseEnemyParameterSheet();
                }

                if (!System.IO.File.Exists(parameterSheetsPath + "Dark Crystal Light Sucker Parameter Sheet.txt"))
                {
                    WriteLightSuckerParameterSheet();
                }

                if (!System.IO.File.Exists(parameterSheetsPath + "Dark Crystal Tank Parameter Sheet.txt"))
                {
                    WriteTankParameterSheet();
                }

                if (!System.IO.File.Exists(parameterSheetsPath + "Dark Crystal Flyer Parameter Sheet.txt"))
                {
                    WriteFlyerParameterSheet();
                }
            }

        }

    }

    void WriteVersionFile()
    {
        StreamWriter writer = new StreamWriter(versionPath + "Version.txt");

        writer.Write(Application.version);

        writer.Close();
    }

    void WritePlayerParameterSheet()
    {
        StreamWriter writer = new StreamWriter(parameterSheetsPath + "Dark Crystal Player Parameter Sheet.txt");

        TextAsset playerParameterSheet = Resources.Load<TextAsset>("Parameter Sheets/Dark Crystal Player Parameter Sheet");

        string[] data = playerParameterSheet.text.Split(new char[] { '\n' });
        for (int i = 0; i < data.Length - 1; i++)
        {
            writer.Write(data[i] + "\n");
        }

        writer.Close();
    }

    void WriteCrystalParameterSheet()
    {
        StreamWriter writer = new StreamWriter(parameterSheetsPath + "Dark Crystal Crystal Parameter Sheet.txt");

        TextAsset crystalParameterSheet = Resources.Load<TextAsset>("Parameter Sheets/Dark Crystal Crystal Parameter Sheet");

        string[] data = crystalParameterSheet.text.Split(new char[] { '\n' });
        for (int i = 0; i < data.Length - 1; i++)
        {
            writer.Write(data[i] + "\n");
        }

        writer.Close();
    }

    void WriteBaseEnemyParameterSheet()
    {
        StreamWriter writer = new StreamWriter(parameterSheetsPath + "Dark Crystal Base Enemy Parameter Sheet.txt");

        TextAsset baseEnemyParameterSheet = Resources.Load<TextAsset>("Parameter Sheets/Dark Crystal Base Enemy Parameter Sheet");

        string[] data = baseEnemyParameterSheet.text.Split(new char[] { '\n' });
        for (int i = 0; i < data.Length - 1; i++)
        {
            writer.Write(data[i] + "\n");
        }

        writer.Close();
    }

    void WriteLightSuckerParameterSheet()
    {
        StreamWriter writer = new StreamWriter(parameterSheetsPath + "Dark Crystal Light Sucker Parameter Sheet.txt");

        TextAsset lightSuckerParameterSheet = Resources.Load<TextAsset>("Parameter Sheets/Dark Crystal Light Sucker Parameter Sheet");

        string[] data = lightSuckerParameterSheet.text.Split(new char[] { '\n' });
        for (int i = 0; i < data.Length - 1; i++)
        {
            writer.Write(data[i] + "\n");
        }

        writer.Close();
    }

    void WriteTankParameterSheet()
    {
        StreamWriter writer = new StreamWriter(parameterSheetsPath + "Dark Crystal Tank Parameter Sheet.txt");

        TextAsset tankParameterSheet = Resources.Load<TextAsset>("Parameter Sheets/Dark Crystal Tank Parameter Sheet");

        string[] data = tankParameterSheet.text.Split(new char[] { '\n' });
        for (int i = 0; i < data.Length - 1; i++)
        {
            writer.Write(data[i] + "\n");
        }

        writer.Close();
    }

    void WriteFlyerParameterSheet()
    {
        StreamWriter writer = new StreamWriter(parameterSheetsPath + "Dark Crystal Flyer Parameter Sheet.txt");

        TextAsset flyerParameterSheet = Resources.Load<TextAsset>("Parameter Sheets/Dark Crystal Flyer Parameter Sheet");

        string[] data = flyerParameterSheet.text.Split(new char[] { '\n' });
        for (int i = 0; i < data.Length - 1; i++)
        {
            writer.Write(data[i] + "\n");
        }

        writer.Close();
    }
}
