using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSettings : MonoBehaviour
{
    UniversalGameSettings _uniGameSettings;
    [SerializeField] GameBrightness _gameBrightness;
    [SerializeField] WorldSpeaker _worldSpeaker;

    private void Awake()
    {
        _uniGameSettings = GameObject.FindGameObjectWithTag("GameSettings").GetComponent<UniversalGameSettings>();

        if (_uniGameSettings == null) { Debug.LogError("Failed to Find Game Settings"); return; }

        _gameBrightness.SetLightValue(_uniGameSettings.Brightness);
        _worldSpeaker.SetVolumeValue(_uniGameSettings.Volume);
    }

    private void Update()
    {
        _gameBrightness.SetLightValue(_uniGameSettings.Brightness);
        _worldSpeaker.SetVolumeValue(_uniGameSettings.Volume);
    }
}
