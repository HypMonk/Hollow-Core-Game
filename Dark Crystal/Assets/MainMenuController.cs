using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    SceneLoader sceneLoader;
    UniversalGameSettings universalGameSettings;
    
    [SerializeField] MainMenuAudio mainMenuAudio;

    List<GameObject> _controlSchemes = new List<GameObject>();
    List<GameObject> _controlSchemesMenuToggleControl = new List<GameObject>();

    int _currentSchemeMenuValue = 1;

    PlayerControlsTest _pControls;

    InputAction back;

    GameObject _currentMenu, _rootMenu;

    GameObject _mainLastSelectedButton, _lastSelectedButton;
    [SerializeField]
    GameObject _mainMenu, _mainMenuFirstButton, _settingsMenu, _settingsFirstButton, _controlsMenu, _controlsFirstButton, _firstRebindButtonGamepad, _firstRebindButtonKeyboard,
        _switchControlSchemeMinusButton, _switchControlSchemePositiveButton;
    [SerializeField]
    Slider _brightnessSlider, _volumeSlider;
    [SerializeField]
    TMPro.TMP_Text _versionNumber;
    [SerializeField]
    GameObject _keyboardSchemeMenu, _gamepadSchemeMenu;

    private void Awake()
    {
        _pControls = new PlayerControlsTest();
    }

    private void OnEnable()
    {
        back = _pControls.UI.Back;
        back.Enable();
        back.performed += BackMenu;
    }

    private void OnDisable()
    {
        back.Disable();

    }

    private void Start()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneLoader>();
        if (sceneLoader == null)
        {
            Debug.Log("No Scene Loader");
        }

        universalGameSettings = GameObject.FindGameObjectWithTag("GameSettings").GetComponent<UniversalGameSettings>();

        //Update Sliders
        _brightnessSlider.value = universalGameSettings.Brightness / universalGameSettings.MaxBrightness;
        _volumeSlider.value = universalGameSettings.Volume / 1;

        mainMenuAudio.SetVolumeValue(universalGameSettings.Volume);

        _versionNumber.text = Application.version;

        _currentMenu = _mainMenu;
        _rootMenu = null;

        _controlSchemes.Add(_keyboardSchemeMenu);
        _controlSchemes.Add(_gamepadSchemeMenu);

        _controlSchemesMenuToggleControl.Add(_firstRebindButtonKeyboard);
        _controlSchemesMenuToggleControl.Add(_firstRebindButtonGamepad);
    }

    private void Update()
    {
        mainMenuAudio.SetVolumeValue(universalGameSettings.Volume);
    }

    public void SwitchControlSchemesMinus()
    {
        _controlSchemes[_currentSchemeMenuValue].SetActive(false);
        _currentSchemeMenuValue--;
        if (_currentSchemeMenuValue < 0)
        {
            _currentSchemeMenuValue = _controlSchemes.Count - 1;
        }
        _controlSchemes[_currentSchemeMenuValue].SetActive(true);

        UpdateButtonNav();
    }

    public void SwitchControlSchemesPlus()
    {
        _controlSchemes[_currentSchemeMenuValue].SetActive(false);
        _currentSchemeMenuValue++;
        if (_currentSchemeMenuValue > _controlSchemes.Count - 1)
        {
            _currentSchemeMenuValue = 0;
        }
        _controlSchemes[_currentSchemeMenuValue].SetActive(true);

        UpdateButtonNav();
    }

    void UpdateButtonNav()
    {
        Navigation minusButtonNav = _switchControlSchemeMinusButton.GetComponent<Button>().navigation;
        minusButtonNav.selectOnDown = _controlSchemesMenuToggleControl[_currentSchemeMenuValue].GetComponent<Button>();
        _switchControlSchemeMinusButton.GetComponent<Button>().navigation = minusButtonNav;

        Navigation posButtonNav = _switchControlSchemePositiveButton.GetComponent<Button>().navigation;
        posButtonNav.selectOnDown = _controlSchemesMenuToggleControl[_currentSchemeMenuValue].GetComponent<Button>();
        _switchControlSchemePositiveButton.GetComponent<Button>().navigation = posButtonNav;
    }

    void BackMenu(InputAction.CallbackContext context)
    {
        if(_rootMenu == null) { return; }

        if (_rootMenu == _mainMenu)
        {
            ReturnToMainMenu();
        }

        if (_rootMenu == _settingsMenu)
        {
            ReturnToSettings();
        }
    }

    //Settings Menu
    public void SettingsMenuOpen()
    {
        _mainLastSelectedButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_settingsFirstButton);

        _currentMenu = _settingsMenu;
        _rootMenu = _mainMenu;
    }

    //Controls Menu
    public void ControlsMenuOpen()
    {
        _lastSelectedButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_controlsFirstButton);

        _currentMenu = _controlsMenu;
        _rootMenu = _settingsMenu;
    }

    public void ReturnToSettings()
    {
        _currentMenu.SetActive(false);
        _rootMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_lastSelectedButton);

        _currentMenu = _settingsMenu;
        _rootMenu = _mainMenu;
    }

    public void ReturnToMainMenu()
    {
        _currentMenu.SetActive(false);
        _rootMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_mainLastSelectedButton);

        _currentMenu = _mainMenu;
        _rootMenu = null;
    }

    public void UpdateGameBrightness()
    {
        universalGameSettings.Brightness = _brightnessSlider.value;
        universalGameSettings.WriteSettings();
    }

    public void UpdateGameVolume()
    {
        universalGameSettings.Volume = _volumeSlider.value;
        universalGameSettings.WriteSettings();
    }

    public void OpenForm()
    {
        Application.OpenURL("https://forms.gle/FJCVyqNuY8oThVd47");
    }

    public void CallSceneLoad(string SceneName)
    {
        if(sceneLoader == null)
        {
            Debug.Log("No Scene Loader");
            return;
        }

        sceneLoader.LoadScene(SceneName);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
