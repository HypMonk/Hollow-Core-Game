using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunOnOpen : MonoBehaviour
{
    public static RunOnOpen Instance;
    [SerializeField] GameObject _warningScreen, _warningScreenButton, _mainMenu, _mainMenuFirstButton;

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

        if (_mainMenu.activeInHierarchy) { _mainMenu.SetActive(false); }
        _warningScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_warningScreenButton);
    }

    public void CloseWarning()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_mainMenuFirstButton);
    }
}
