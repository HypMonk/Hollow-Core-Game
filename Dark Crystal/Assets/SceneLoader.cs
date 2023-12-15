using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

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

    public void LoadScene(string SceneName)
    {
        if(SceneManager.GetSceneByName(SceneName) == null)
        {
            Debug.Log("\"" + SceneName + "\" Does not exist in scenes.");
            return;
        }

        SceneManager.LoadScene(SceneName);
    }
}
