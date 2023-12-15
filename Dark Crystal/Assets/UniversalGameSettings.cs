using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UniversalGameSettings : MonoBehaviour
{
    public static UniversalGameSettings Instance;

    string writtenSettingsPath;

    const float _MAXBRIGHTNESS = .5f;

    float _brightness = .1f;
    float _volume = 1;

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

        //Update Settings with current settings
        writtenSettingsPath = Application.persistentDataPath + "/SavedData/";

        if (!System.IO.Directory.Exists(writtenSettingsPath))
        {
            Directory.CreateDirectory(writtenSettingsPath);
        }

        writtenSettingsPath = writtenSettingsPath + "Settings.txt";

        if (!System.IO.File.Exists(writtenSettingsPath))
        {
            StreamWriter writer = new StreamWriter(writtenSettingsPath);

            writer.Write(_brightness + "\n");
            writer.Write(_volume + "\n");

            writer.Close();
        } else
        {
            StreamReader reader = new StreamReader(writtenSettingsPath);
            _brightness = float.Parse(reader.ReadLine());
            _volume = float.Parse(reader.ReadLine());
            reader.Close();
        }
    }

    public float Brightness { get { return _brightness; } set { _brightness = value * _MAXBRIGHTNESS; } }
    public float MaxBrightness { get { return _MAXBRIGHTNESS; } }


    public float Volume { get { return _volume; } set { _volume = value * 1; } }

    //Write settings to disk
    public void WriteSettings()
    {
        StreamWriter writer = new StreamWriter(writtenSettingsPath);

        writer.Write(_brightness + "\n");
        writer.Write(_volume + "\n");

        writer.Close();
    }
}
