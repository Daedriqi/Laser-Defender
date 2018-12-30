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
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject quitButton;

    //cache references
    int score;
    Text scoreboard;
    Text statusText;
    int maxHealthScaler = 150;
    int currentHealthScaling = 0;
    PlayerShip playerShip;
    BigBombUI bigBombUI;

    //state variables
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        Screen.SetResolution(450, 800, false);
        playerShip = FindObjectOfType<PlayerShip>();
        score = 0;
        Game[] games = FindObjectsOfType<Game>();
        if (games.Length > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.GetComponent<AudioSource>();
            SetGameState(GameState.Playing);
        }
        GameObject statusTextObject = GameObject.FindGameObjectWithTag("StatusText");
        statusText = statusTextObject.GetComponent<Text>();
        GameObject scoreboardObject = GameObject.FindGameObjectWithTag("Scoreboard");
        scoreboard = scoreboardObject.GetComponent<Text>();
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

    public void SetGameState(GameState newState) {
        GameObject statusTextObject = GameObject.FindGameObjectWithTag("StatusText");
        statusText = statusTextObject.GetComponent<Text>();
        GameObject scoreboardObject = GameObject.FindGameObjectWithTag("Scoreboard");
        scoreboard = scoreboardObject.GetComponent<Text>();
        state = newState;
        if (newState == GameState.Playing) {
            if (audioSource.clip != gameMusic) {
                audioSource.Stop();
                audioSource.clip = gameMusic;
                audioSource.Play();
            }
            scoreboard.text = "Score: " + score;
            healthBarUIObject.SetActive(true);
            Time.timeScale = 1;
            statusText.GetComponent<Text>().text = "";
        }
        if (newState == GameState.Paused) {
            Time.timeScale = 0;
            statusText.GetComponent<Text>().text = "Paused";
        }
        if (newState == GameState.GameOver) {
            GameOver();
            healthBarUIObject.SetActive(false);
            ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();
            foreach (ParticleSystem particleSystem in particleSystems) {
                particleSystem.Pause();
            }
        }
        if (newState == GameState.Menu) {
            scoreboard.text = "";
            healthBarUIObject.SetActive(false);
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
        statusText.text = "Game Over";
        restartButton.SetActive(true);
        quitButton.SetActive(true);
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
        restartButton.SetActive(false);
        quitButton.SetActive(false);
        currentHealthScaling = 0;
        Time.timeScale = 1;
        statusText.text = "";
        scoreboard.text = "Score: 0";
        SceneManager.LoadScene(0);
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
