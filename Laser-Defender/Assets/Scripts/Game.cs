using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
    //configuration parameters
    [SerializeField] AudioClip menuMusic;
    [Range(0f,10f)][SerializeField] float gameSpeed = 1;
    [SerializeField] AudioClip gameOverMusic;
    [SerializeField] AudioClip gameMusic;
    [SerializeField] AudioClip victoryMusic;
    [SerializeField] GameState state = GameState.Playing;
    [SerializeField] GameObject restartButton;
    [SerializeField] int enemyHealthScaling = 1;

    //cache references
    int score;
    Text scoreboard;
    Text gameOverText;
    int maxHealthScaler = 75;
    int currentHealthScaling = 0;
    PlayerShip playerShip;
    BigBombUI bigBombUI;

    //state variables
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
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
        Text[] texts = FindObjectsOfType<Text>();
        for (int textIndex = 0; textIndex < texts.Length; textIndex++) {
            if (texts[textIndex].gameObject.tag == "GameOverText") {
                gameOverText = texts[textIndex];
            }
            if (texts[textIndex].gameObject.tag == "Scoreboard") {
                scoreboard = texts[textIndex];
            }
        }

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetGameState(GameState newState) {
        state = newState;
        if (newState == GameState.Playing) {
            audioSource.Stop();
            audioSource.clip = gameMusic;
            audioSource.Play();
        }
        if (newState == GameState.Paused) {
            audioSource.Stop();
        }
        if (newState == GameState.GameOver) {
            GameOver();
        }
    }

    public void RoundComplete() {
        if (enemyHealthScaling < maxHealthScaler) {
            currentHealthScaling += enemyHealthScaling;
        }
    }

    public int GetHealthScaling() {
        return currentHealthScaling;
    }

    private void GameOver() {
        GameObject[] everything = FindObjectsOfType<GameObject>();
        foreach (GameObject thing in everything) {
            if (thing.GetComponent<Rigidbody2D>() != null) {
                thing.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }
        gameOverText.text = "Game Over";
        restartButton.SetActive(true);
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
        Time.timeScale = 1;
        gameOverText.text = "";
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
        Victory
    }
}
