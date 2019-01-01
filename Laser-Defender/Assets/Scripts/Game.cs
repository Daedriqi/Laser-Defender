using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

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
    [SerializeField] TextMeshProUGUI playButtonText;
    [SerializeField] GameObject settingsButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] TextMeshProUGUI scoreboard;
    [SerializeField] TextMeshProUGUI statusText;

    //cache references
    int score;
    int maxHealthScaler = 150;
    int currentHealthScaling = 0;
    PlayerShip playerShip;
    BigBombUI bigBombUI;

    //state variables
    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake() {
        Screen.SetResolution(450, 800, false);
        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.GetComponent<AudioSource>();
            SetGameState(GameState.Menu);
        }
        playerShip = FindObjectOfType<PlayerShip>();
    }

    // Update is called once per frame
    void Update() {
        PauseGame();
    }

    private void PauseGame() {
        if (Input.GetButtonDown("Pause")) {
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
            healthBarUIObject.SetActive(true);
            if (audioSource.clip != gameMusic) {
                audioSource.Stop();
                audioSource.clip = gameMusic;
                audioSource.Play();
            }
            scoreboard.text = "Score: " + score;
            Time.timeScale = 1;
            statusText.text = "";
            ButtonsShowHide(false);
        }
        if (newState == GameState.Paused) {
            ButtonsShowHide(true);
            Time.timeScale = 0;
            statusText.text = "Paused";
            playButtonText.text = "Restart";
        }
        if (newState == GameState.GameOver) {
            GameOver();
        }
        if (newState == GameState.Menu) {
            scoreboard.text = "";
            healthBarUIObject.SetActive(false);
        }
    }

    private void ButtonsShowHide(bool showButtons) {
        if (showButtons) {
            playButton.SetActive(true);
            settingsButton.SetActive(true);
            quitButton.SetActive(true);
        }
        else {
            playButton.SetActive(false);
            settingsButton.SetActive(false);
            quitButton.SetActive(false);
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
        quitButton.SetActive(true);
        playButton.SetActive(true);
        settingsButton.SetActive(true);
        playButtonText.text = "Try Again";
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
        healthBarUIObject.SetActive(true);
        HealthBarUI healthBarUI = FindObjectOfType<HealthBarUI>();
        healthBarUI.UpdateHealthBar(300);
        healthBarUI.UpdateShieldBar(300);
        quitButton.SetActive(false);
        playButton.SetActive(false);
        settingsButton.SetActive(false);
        currentHealthScaling = 0;
        Time.timeScale = 1;
        statusText.text = "";
        scoreboard.text = "Score: 0";
        SceneManager.LoadScene("Level 1");
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
