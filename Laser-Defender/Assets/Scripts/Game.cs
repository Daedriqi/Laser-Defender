using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    //configuration parameters
    [SerializeField] AudioClip menuMusic;
    [Range(0f,10f)][SerializeField] float gameSpeed = 1;
    [SerializeField] AudioClip gameOverMusic;
    [SerializeField] AudioClip gameMusic;
    [SerializeField] AudioClip victoryMusic;
    [SerializeField] GameState state = GameState.Playing;

    //cache references


    //state variables
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        Game[] games = FindObjectsOfType<Game>();
        if (games.Length > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.GetComponent<AudioSource>();
            SetGameState(GameState.Playing);
        }
    }

    // Update is called once per frame
    void Update() {
        Time.timeScale = gameSpeed;
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
    }

    public void StartGame() {
        SetGameState(GameState.Playing);
    }

    public void PauseGame() {
        SetGameState(GameState.Paused);
    }

    public GameState GetGameState() {
        return state;
    }

    public enum GameState {
        Playing,
        Paused,
        GameOver,
        Victory
    }
}
