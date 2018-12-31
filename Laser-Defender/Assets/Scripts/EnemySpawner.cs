using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] GameObject waveContainerPrefab;

    int waveIndex = 0;
    bool nextWave = false;
    WaveConfig currentWave;
    Game game;
    Coroutine spawnAllEnemiesInWave;
    Coroutine spawnAllEnemiesForPath;

    // Start is called before the first frame update
    void Start() {
        game = FindObjectOfType<Game>();
        currentWave = waveConfigs[0];
        spawnAllEnemiesInWave = StartCoroutine(SpawnAllEnemiesInWave(currentWave));
    }

    // Update is called once per frame
    void Update() {
        if (nextWave) {
            nextWave = false;
            StopCoroutine(spawnAllEnemiesInWave);
            StopCoroutine(spawnAllEnemiesForPath);
            spawnAllEnemiesInWave = StartCoroutine(SpawnAllEnemiesInWave(waveConfigs[waveIndex]));
        }
        if (game.GetGameState() == Game.GameState.GameOver) {
            StopAllCoroutines();
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveToSpawn) {
        GameObject waveContainer = Instantiate(waveContainerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        waveContainer.name = waveToSpawn.name + "Container";
        waveContainer.GetComponent<WaveContainer>().SetWave(waveToSpawn);
        foreach (List<Transform> path in waveToSpawn.GetWaypoints()) {
            spawnAllEnemiesForPath = StartCoroutine(SpawnAllEnemiesForPath(path, waveToSpawn, waveContainer));
        }
        if (waveIndex >= waveConfigs.Count - 1) {
            waveIndex = -1;
        }
        waveIndex++;
        yield return new WaitForSeconds((waveToSpawn.GetTimeBetweenSpawns() * waveToSpawn.GetEnemyCount()) + 1);
        nextWave = true;
        game.RoundComplete();
    }

    private IEnumerator SpawnAllEnemiesForPath(List<Transform> path, WaveConfig waveToSpawn, GameObject waveContainer) {
        int enemiesSpawned = 0;
        while (enemiesSpawned < waveToSpawn.GetEnemyCount()) {
            GameObject enemyPrefab = waveToSpawn.GetEnemyPrefab();
            GameObject enemy = Instantiate(enemyPrefab, path[0].position, Quaternion.identity);
            enemy.transform.parent = waveContainer.transform;
            EnemyPathing pathing = enemy.GetComponent<EnemyPathing>();
            pathing.SetWave(waveToSpawn);
            pathing.SetWaypoints(path);
            EnemyShip enemyShip = enemy.GetComponent<EnemyShip>();
            enemyShip.SetHealthOnSpawn(game.GetHealthScaling());
            enemiesSpawned++;
            yield return new WaitForSeconds(waveToSpawn.GetTimeBetweenSpawns());
            enemyShip.MakeDamagable();
        }
    }
}
