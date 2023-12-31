using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Debug World Variables
    public static bool usingTestVariables = false;

    GameObject _player;

    float _gameTimer;
    [HideInInspector]
    public int gameHour, gameMinute, gameSecond;

    public enum GameState { Exploring, Danger, Combat};
    GameState _currentState;

    public static bool isPaused;

    [SerializeField] WorldSpeaker worldSpeaker;

    SceneLoader sceneLoader;

    GameObject[] crystalSpawnLocations;
    [SerializeField] GameObject darkCrystalPrefab;
    bool crystalSpawnInvoked = false;

    public GameState CurrentState{ get { return _currentState; } }

    public void ToggleGameState(GameState state)
    {
        if (state != _currentState)
        {
            _currentState = state;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameTimer = 0;
        _currentState = GameState.Exploring;

        _player = GameObject.FindGameObjectWithTag("Player");
        sceneLoader = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneLoader>();

        crystalSpawnLocations = GameObject.FindGameObjectsWithTag("CrystalSpawnPoint");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePauseGame();
        }

        _gameTimer += Time.deltaTime;
        TimeConversion();
        PlayerStatus();

        if (!crystalSpawnInvoked && Mathf.RoundToInt(_gameTimer) % 50 == 0)
        {
            crystalSpawnInvoked = true;
            StartCoroutine(SpawnCrystal());
        }
    }

    public void TogglePauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
            worldSpeaker.StartPauseMusic();
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
            worldSpeaker.StopPauseMusic();
        }
    }

    void TimeConversion()
    {
        gameHour = System.TimeSpan.FromSeconds(_gameTimer).Hours;
        gameMinute = System.TimeSpan.FromSeconds(_gameTimer).Minutes;
        gameSecond = System.TimeSpan.FromSeconds(_gameTimer).Seconds;
    }

    IEnumerator SpawnCrystal()
    {
        bool filled = true;
        foreach (GameObject spawnLocation in crystalSpawnLocations)
        {
            Collider2D hit = Physics2D.OverlapCircle((Vector2)spawnLocation.transform.position, 3, 1 << 9);
            if (hit == null)
            {
                filled = false;
                break;
            }
        }
        if (!filled)
        {
            bool spawned = false;
            while (spawned == false)
            {
                int randomLocationIndex = Mathf.RoundToInt(Random.Range(0, crystalSpawnLocations.Length - 1));
                Collider2D hit = Physics2D.OverlapCircle((Vector2)crystalSpawnLocations[randomLocationIndex].transform.position, 3, 1 << 9);
                if (hit == null)
                {
                    Instantiate(darkCrystalPrefab, crystalSpawnLocations[randomLocationIndex].transform.position, Quaternion.identity);
                    spawned = true;
                }
            }
        }
        yield return new WaitForSeconds(2);
        crystalSpawnInvoked = false;
    }

    void PlayerStatus()
    {
        if (_player == null) return;
        if (_player.GetComponent<PlayerStats>().Health <= 0)
        {
            Debug.Log("Player Died");
            PlayerDeath();
        }

        Collider2D nearbyEnemies = Physics2D.OverlapCircle(_player.transform.position, 10, LayerMask.GetMask("Enemy"));
        if (nearbyEnemies != null)
        {
            ToggleGameState(GameState.Combat);
        }

        Collider2D nearbyCrystal = Physics2D.OverlapCircle(_player.transform.position, 100, LayerMask.GetMask("Crystal"));
        if(nearbyCrystal != null)
        {
            ToggleGameState(GameState.Danger);
        }
        else
        {
            ToggleGameState(GameState.Exploring);
        }

    }

    public void PlayerDeath()
    {
        //Collect stats for record
        Debug.Log("Player Death [GameManager]");
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(.5f);
        sceneLoader.LoadScene("MainMenu");
    }
}
