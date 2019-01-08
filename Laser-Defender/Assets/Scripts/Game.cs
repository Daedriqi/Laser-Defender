using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class Game : MonoBehaviour {
    //configuration parameters
    [Header("Music")]
    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip gameOverMusic;
    [SerializeField] AudioClip gameMusic;
    [SerializeField] AudioClip bossMusic;
    [SerializeField] AudioClip victoryMusic;

    [Header("UI")]
    [SerializeField] GameObject healthBarUIObject;
    [SerializeField] GameObject playButton;
    [SerializeField] TextMeshProUGUI playButtonText;
    [SerializeField] GameObject settingsButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] TextMeshProUGUI scoreboard;
    [SerializeField] TextMeshProUGUI statusText;

    [Header("Other")]
    [SerializeField] GameState state = GameState.Playing;
    [Range(0f, 25f)] [SerializeField] float gameSpeed = 1;
    [SerializeField] int enemyHealthScaling = 1;

    //cache references
    int startLevelIndex = 0;
    int score;
    int levelIndex;
    int maxHealthScaler = 25;
    int currentHealthScaling = 0;
    bool bossFight = false;
    PlayerShip playerShip;
    BigBombUI bigBombUI;
    HealthBarUI healthBarUI;

    //state variables
    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake() {
        levelIndex = startLevelIndex;
        audioSource = gameObject.GetComponent<AudioSource>();
        healthBarUI = FindObjectOfType<HealthBarUI>();
        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
        audioSource = gameObject.GetComponent<AudioSource>();
        SetGameState(GameState.Menu);
        playerShip = FindObjectOfType<PlayerShip>();
    }

    // Update is called once per frame
    void Update() {
        PauseGame();
        Time.timeScale = gameSpeed;
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

    public void PlayBossMusic() {
        bossFight = true;
        audioSource.Stop();
        audioSource.clip = bossMusic;
        audioSource.Play();
    }

    public void PlayGameMusic() {
        bossFight = false;
        audioSource.Stop();
        audioSource.clip = gameMusic;
        audioSource.Play();
    }

    public void SetGameState(GameState newState) {
        state = newState;
        if (newState == GameState.Playing) {
            healthBarUIObject.SetActive(true);
            scoreboard.text = "Score: " + score;
            gameSpeed = 1;
            statusText.text = "";
            ButtonsShowHide(false);
        }
        if (newState == GameState.Paused) {
            ButtonsShowHide(true);
            gameSpeed = 0;
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

    public void ResetHealthScaling() {
        currentHealthScaling = 0;
    }

    public int GetHealthScaling() {
        return currentHealthScaling;
    }

    public void IncreaseLevelIndex() {
        levelIndex++;
        playerShip = FindObjectOfType<PlayerShip>();
        playerShip.IncreasePlayerStats();
    }

    private void GameOver() {
        SetTimeScale(0);
        quitButton.SetActive(true);
        playButton.SetActive(true);
        settingsButton.SetActive(true);
        playButtonText.text = "Try Again";
        statusText.text = "Game Over";
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

    public int GetLevelIndex() {
        return levelIndex;
    }

    public void SetLevelIndex(int level) {
        levelIndex = level;
    }

    public void RestartGame() {
        StopAllCoroutines();
        levelIndex = startLevelIndex;
        score = 0;
        SetGameState(GameState.Playing);
        healthBarUIObject.SetActive(true);
        healthBarUI.UpdateHealthBar(300);
        healthBarUI.UpdateShieldBar(300);
        quitButton.SetActive(false);
        playButton.SetActive(false);
        settingsButton.SetActive(false);
        audioSource.Stop();
        audioSource.clip = gameMusic;
        audioSource.Play();
        currentHealthScaling = 0;
        gameSpeed = 1;
        statusText.text = "";
        scoreboard.text = "Score: 0";
        SceneManager.LoadScene("Game");
    }

    public GameState GetGameState() {
        return state;
    }

    public void SetTimeScale(float scale) {
        gameSpeed = scale;
    }

    public enum GameState {
        Playing,
        Paused,
        GameOver,
        Victory,
        Menu
    }
}
