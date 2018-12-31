using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Game : MonoBehaviour {
    //configuration parameters
    [SerializeField] AudioClip menuMusic;
    [Range(0f, 10f)] [SerializeField] float gameSpeed = 1;
    [SerializeField] AudioClip gameOverMusic;
    [SerializeField] AudioClip gameMusic;
    [SerializeField] AudioClip victoryMusic;
    [SerializeField] GameState state = GameState.Playing;
    [SerializeField] int enemyHealthScaling = 1;
    [SerializeField] GameObject healthBarUIObject;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject settingsButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] Text scoreboard;
    [SerializeField] Text statusText;

    //cache references
    int score;
    int maxHealthScaler = 150;
    int currentHealthScaling = 0;
    PlayerShip playerShip;
    BigBombUI bigBombUI;

    //state variables
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        Screen.SetResolution(450, 800, false);
        Game[] games = FindObjectsOfType<Game>();
        if (games.Length > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.GetComponent<AudioSource>();
            SetGameState(GameState.Menu);
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (state == GameState.Playing) {
                SetGameState(GameState.Paused);
            }
            else if (state == GameState.Paused) {
                SetGameState(GameState.Playing);
            }
        }
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ResumeGame() {
        SetGameState(GameState.Playing);
    }

    public void Settings() {
        //TODO fill this
    }

    public void SetGameState(GameState newState) {
        state = newState;
        if (newState == GameState.Playing) {
            playerShip = FindObjectOfType<PlayerShip>();
            healthBarUIObject.transform.localScale = new Vector3(2.5f, 2.5f, 1);
            if (audioSource.clip != gameMusic) {
                audioSource.Stop();
                audioSource.clip = gameMusic;
                audioSource.Play();
            }
            scoreboard.transform.localScale = new Vector3(1, 1, 1);
            scoreboard.text = "Score: " + score;
            Time.timeScale = 1;
            statusText.GetComponent<Text>().text = "";
        }
        if (newState == GameState.Paused) {

            Time.timeScale = 0;
            statusText.GetComponent<Text>().text = "Paused";
        }
        if (newState == GameState.GameOver) {
            GameOver();
        }
        if (newState == GameState.Menu) {
            scoreboard.text = "";
            healthBarUIObject.transform.localScale = new Vector3(0, 0, 0);
        }

    }

    public void RoundComplete() {
        if (currentHealthScaling < maxHealthScaler) {
            currentHealthScaling += enemyHealthScaling;
        }
    }

    public int GetHealthScaling() {
        return currentHealthScaling;
    }

    private void GameOver() {
        Time.timeScale = 0;
        quitButton = GameObject.FindGameObjectWithTag("QuitButton");
        playButton = GameObject.FindGameObjectWithTag("PlayButton");
        settingsButton = GameObject.FindGameObjectWithTag("SettingsButton");
        statusText = GameObject.FindGameObjectWithTag("StatusText").GetComponent<Text>();
        scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<Text>();
        quitButton.transform.localScale = new Vector3(1, 1, 1);
        playButton.transform.localScale = new Vector3(1, 1, 1);
        settingsButton.transform.localScale = new Vector3(1, 1, 1);
        statusText.text = "Game Over";
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = gameOverMusic;
        audioSource.Play();
    }

    public void StartGame() {
        SetGameState(GameState.Playing);
    }

    public int AddToScore(int scoreToAdd) {
        score += scoreToAdd;
        return score;
    }

    public void RestartGame() {
        StopAllCoroutines();
        score = 0;
        SetGameState(GameState.Playing);
        HealthBarUI healthBarUI = FindObjectOfType<HealthBarUI>();
        healthBarUI.UpdateHealthBar(300);
        healthBarUI.UpdateShieldBar(300);
        healthBarUI.transform.localScale = new Vector3(2.5f, 2.5f, 1);
        quitButton.transform.localScale = new Vector3(0, 0, 0);
        playButton.transform.localScale = new Vector3(0, 0, 0);
        settingsButton.transform.localScale = new Vector3(0, 0, 0);
        currentHealthScaling = 0;
        Time.timeScale = 1;
        statusText.text = "";
        scoreboard.text = "Score: 0";
        SceneManager.LoadScene("Level 1");
    }

    public void PauseGame() {
        SetGameState(GameState.Paused);
    }

    public GameState GetGameState() {
        return state;
    }

    public void SetTimeScale(float scale) {
        Time.timeScale = scale;
    }

    public enum GameState {
        Playing,
        Paused,
        GameOver,
        Victory,
        Menu
    }
}
