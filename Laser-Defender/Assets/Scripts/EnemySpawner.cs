using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] List<WaveConfig> waveConfigs;

    int waveIndex = 0;
    bool nextWave = false;
    WaveConfig currentWave;
    Game game;

    // Start is called before the first frame update
    void Start() {
        game = FindObjectOfType<Game>();
        currentWave = waveConfigs[0];
        StartCoroutine(SpawnAllEnemiesInWave(currentWave));
    }

    // Update is called once per frame
    void Update() {
        if (nextWave) {
            nextWave = false;
            StopCoroutine(SpawnAllEnemiesInWave(null));
            StopCoroutine(SpawnAllEnemiesForPath(null, null));
            StartCoroutine(SpawnAllEnemiesInWave(waveConfigs[waveIndex]));
        }
        if (game.GetGameState() == Game.GameState.GameOver) {
            StopAllCoroutines();
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveToSpawn) {
        waveToSpawn.SetCounterZero();
        foreach (List<Transform> path in waveToSpawn.GetWaypoints()) {
            StartCoroutine(SpawnAllEnemiesForPath(path, waveToSpawn));
        }
        if (waveIndex >= waveConfigs.Count - 1) {
            waveIndex = -1;
        }
        waveIndex++;
        yield return new WaitForSeconds((waveToSpawn.GetTimeBetweenSpawns() * waveToSpawn.GetEnemyCount()) + 1);
        nextWave = true;
    }

    private IEnumerator SpawnAllEnemiesForPath(List<Transform> path, WaveConfig waveToSpawn) {
        int enemiesSpawned = 0;
        while (enemiesSpawned < waveToSpawn.GetEnemyCount()) {
            GameObject enemyPrefab = waveToSpawn.GetEnemyPrefab();
            GameObject enemy = Instantiate(enemyPrefab, path[0].position, Quaternion.identity);
            enemy.GetComponent<EnemyPathing>().SetWave(waveToSpawn);
            enemy.GetComponent<EnemyPathing>().SetWaypoints(path);
            enemiesSpawned++;
            yield return new WaitForSeconds(waveToSpawn.GetTimeBetweenSpawns());
        }
    }
}
