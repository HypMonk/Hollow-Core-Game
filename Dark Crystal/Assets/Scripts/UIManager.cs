using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;
    PlayerStats playerStats;
    PlayerInventory playerInventory;
    SceneLoader sceneLoader;
    UniversalGameSettings universalGameSettings;

    List<Dictionary<string, ParameterClass>> _parameterSheets = new List<Dictionary<string, ParameterClass>>();

    GameObject _currentMenu, _rootMenu;

    GameObject _pauseLastSelectedButton, _lastSelectedButton;

    PlayerControlsTest _pControls;

    InputAction back;

    [SerializeField]
    TMP_Text _uiTimer, _shardCount;
    [SerializeField]
    Slider _staminaBar, _powerBar, _healthBar, _brightnessSlider, _volumeSlider;
    [SerializeField]
    Image _HUDOverlay;
    [SerializeField]
    GameObject _pauseMenu, _pauseFirstButton, _debugControlMenu, 
        _debugFirstButton, _settingsMenu, _settingsFirstButton, _controlsMenu, _controlsFirstButton;
    [SerializeField]
    TMP_Dropdown _parameterSheetDropDown;
    [SerializeField]
    TMP_Text _parameterReader;

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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();

        universalGameSettings = GameObject.FindGameObjectWithTag("GameSettings").GetComponent<UniversalGameSettings>();

        sceneLoader = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneLoader>();
        if (sceneLoader == null)
        {
            Debug.Log("No Scene Loader");
        }

        //create list of parameter sheets (probably can be done better)
        GameParameters gameParameters = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameParameters>();
        _parameterSheets.Add(gameParameters.playerParameters);
        _parameterSheets.Add(gameParameters.crystalParameters);
        _parameterSheets.Add(gameParameters.baseEnemyParameters);
        _parameterSheets.Add(gameParameters.lightSuckerParameters);
        _parameterSheets.Add(gameParameters.tankParameters);
        _parameterSheets.Add(gameParameters.flyerParameters);

        //Update Sliders
        _brightnessSlider.value = universalGameSettings.Brightness/universalGameSettings.MaxBrightness;
        _volumeSlider.value = universalGameSettings.Volume/1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isPaused && !_HUDOverlay.enabled)
        {
            _HUDOverlay.enabled = true;
            _pauseMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_pauseFirstButton);
        } else if (!GameManager.isPaused && _HUDOverlay.enabled)
        {
            _HUDOverlay.enabled = false;
            _pauseMenu.SetActive(false);
            if (_debugControlMenu.activeInHierarchy) {  _debugControlMenu.SetActive(false); }
            if (_settingsMenu.activeInHierarchy) {  _settingsMenu.SetActive(false); }
            if (_controlsMenu.activeInHierarchy) {  _controlsMenu.SetActive(false); }

        }

        _uiTimer.text = BuildTimer();
        _healthBar.value = playerStats.Health;
        _staminaBar.value = playerStats.Stamina;
        _powerBar.value = playerStats.LightLevel;
        _shardCount.text = "Shards: " + playerInventory.Shards;
    }


    string BuildTimer()
    {
        string _hours = gameManager.gameHour.ToString();
        if (gameManager.gameHour < 10) { _hours = "0" + gameManager.gameHour.ToString(); }

        string _minutes = gameManager.gameMinute.ToString();
        if (gameManager.gameMinute < 10) { _minutes = "0" + gameManager.gameMinute.ToString(); }

        string _seconds = gameManager.gameSecond.ToString();
        if (gameManager.gameSecond < 10) { _seconds = "0" + gameManager.gameSecond.ToString(); }

        return _hours + ":" + _minutes + ":" + _seconds;
    }

    void BackMenu(InputAction.CallbackContext context)
    {
        if (!GameManager.isPaused) return;
        if (_rootMenu == null) 
        {
            gameManager.TogglePauseGame();
            return; 
        }

        if (_rootMenu == _pauseMenu)
        {
            ReturnToPause();
        }

        if (_rootMenu == _settingsMenu)
        {
            ReturnToSettings();
        }
    }



    //Debug Menu
    public void DebugMenuOpen()
    {
        _pauseLastSelectedButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_debugFirstButton);
        UpdateParameterReader();

        _currentMenu = _debugControlMenu;
        _rootMenu = _pauseMenu;
    }

    //Settings Menu
    public void SettingsMenuOpen()
    {
        _pauseLastSelectedButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_settingsFirstButton);

        _currentMenu = _settingsMenu;
        _rootMenu = _pauseMenu;
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
        _rootMenu = _pauseMenu;
    }

    public void ReturnToPause()
    {
        _currentMenu.SetActive(false);
        _rootMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_pauseLastSelectedButton);

        _currentMenu = _pauseMenu;
        _rootMenu = null;
    }

    public void LoadDefaultValues()
    {
        gameManager.GetComponent<GameParameters>().UpdateAllParameterSheets();

        GameManager.usingTestVariables = false;

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().UpdateStats();

        GameObject[] crystals = GameObject.FindGameObjectsWithTag("DarkCrystal");
        foreach (GameObject crystal in crystals)
        {
            crystal.GetComponent<DarkCrystal>().UpdateStats();
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyClass>().UpdateParameters();
        }

        UpdateParameterReader();
    } 

    public void LoadTestValues()
    {
        gameManager.GetComponent<GameParameters>().UpdateAllParameterSheets();

        GameManager.usingTestVariables = true;

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().UpdateStats();

        GameObject[] crystals = (GameObject.FindGameObjectsWithTag("DarkCrystal"));
        foreach (GameObject crystal in crystals)
        {
            crystal.GetComponent<DarkCrystal>().UpdateStats();
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyClass>().UpdateParameters();
        }

        UpdateParameterReader();
    }

    public void OpenFileExplorerParameterFolder()
    {
        Application.OpenURL(Application.persistentDataPath + "/Parameter Sheets/");
    }

    public void UpdateParameterReader()
    {
        int parameterSheetIndex;
        parameterSheetIndex = _parameterSheetDropDown.value;
        string parameterReaderText = "Parameter	|	Default Value	|	Test Value\n";
        for (int i = 0; i < _parameterSheets[parameterSheetIndex].Count; i++)
        {
            parameterReaderText += _parameterSheets[parameterSheetIndex].ElementAt(i).Value.ParameterName + "\t|\t" +
                _parameterSheets[parameterSheetIndex].ElementAt(i).Value.DefaultValue + "\t|\t" +
                _parameterSheets[parameterSheetIndex].ElementAt(i).Value.TestValue + "\n";
        }
        _parameterReader.text = parameterReaderText;
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

    public void CallSceneLoad(string SceneName)
    {
        if (sceneLoader == null)
        {
            Debug.Log("No Scene Loader");
            return;
        }

        if (GameManager.isPaused)
        {
            _HUDOverlay.enabled = false;
            _pauseMenu.SetActive(false);
            gameManager.TogglePauseGame();
        }

        sceneLoader.LoadScene(SceneName);
    }
}
