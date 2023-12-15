using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpeaker : MonoBehaviour
{
    AudioManager audioManager;
    GameManager gameManager;

    GameManager.GameState currentState;
    GameManager.GameState nextState;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //Debug.Log(gameManager.CurrentState);
        nextState = gameManager.CurrentState;
    }

    // Update is called once per frame
    void Update()
    {

        nextState = gameManager.CurrentState;

        //Debug.Log("World Speaker Next State: " + nextState);
        if (nextState != currentState)
        {
            //Debug.Log("World Speaker: Changing States");
            StopCurrentMusic();
            currentState = nextState;
            SetAmbientSound(currentState);
            SetGameMusic(currentState);
        }

    }

    public void StartPauseMusic()
    {
        StopCurrentMusic();
        audioManager.Play("Ambient Cave", .5f, 0);
    }

    public void StopPauseMusic()
    {
        SetAmbientSound(currentState);
        SetGameMusic(currentState);
    }

    void StopCurrentMusic()
    {
        if (currentState == GameManager.GameState.Exploring)
        {
            //Debug.Log("Playing Exploring Music");
            audioManager.Stop("Exploring Music");
            return;
        }
        if (currentState == GameManager.GameState.Danger)
        {
            audioManager.Stop("Tension Music");
            return;
        }
    }

    void SetAmbientSound(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Exploring)
        {
            //Debug.Log("Playing Exploring Ambience ");
            audioManager.Play("Ambient Cave", .8f, 0);
            return;
        }

        audioManager.Stop("Ambient Cave");
    }

    void SetGameMusic(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Exploring)
        {
            //Debug.Log("Playing Exploring Music");
            audioManager.Play("Exploring Music");
            return;
        }
        if (state == GameManager.GameState.Danger)
        {
            audioManager.Play("Tension Music");
            return;
        }
    }

    public void SetVolumeValue(float value)
    {
        AudioListener.volume = value;
    }
}
