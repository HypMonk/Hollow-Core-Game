using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.Log("No Audio Manager");
            return;
        }

        audioManager.Play("Exploring Music");
    }

    public void SetVolumeValue(float value)
    {
        AudioListener.volume = value;
    }
}
