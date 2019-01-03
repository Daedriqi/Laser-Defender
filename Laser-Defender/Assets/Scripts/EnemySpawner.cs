using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] GameObject waveContainerPrefab;
    [SerializeField] int roundsToBossFight = 3;
    [SerializeField] List<GameObject> specialtySpawns;
    [SerializeField] List<GameObject> bosses;

    int waveIndex = 0;
    int waveCount = 0;
    int levelIndex = 0;
    int specialSpawnIndex = 0;
    int currentRound = 3;
    bool nextWave = false;
    bool bossFight = false;
    int bossIndex = 0;
    WaveConfig currentWave;
    Game game;
    Coroutine spawnAllEnemiesInWave;
    Coroutine spawnAllEnemiesForPath;
    Coroutine bossFightRoutine;

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
        yield return new WaitForSeconds(0.1f);
        GameObject waveContainerObject = Instantiate(waveContainerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        waveContainerObject.name = waveToSpawn.name + "Container";
        WaveContainer waveContainer = waveContainerObject.GetComponent<WaveContainer>();
        waveContainer.SetWave(waveToSpawn);
        waveToSpawn.SetEnemyPrefab(levelIndex);
        foreach (List<Transform> path in waveToSpawn.GetWaypoints()) {
            spawnAllEnemiesForPath = StartCoroutine(SpawnAllEnemiesForPath(path, waveToSpawn, waveContainerObject));
        }
        waveCount++;
        if (waveIndex >= waveConfigs.Count - 1) {
            waveIndex = -1;
        }
        if ((levelIndex + 1) % 2 == 0 && currentRound >= 2) {
            SpawnSpecialtyWave(waveToSpawn);
        }
        waveIndex++;
        yield return new WaitForSeconds((waveToSpawn.GetTimeBetweenSpawns() * waveToSpawn.GetEnemyCount()) + 1);
        if (waveCount >= 5) {
            game.RoundComplete();
            currentRound++;
            waveCount = 0;
        }
        if (currentRound >= roundsToBossFight) {
            currentRound = 0;
            levelIndex++;
            bossFight = true;
            bossFightRoutine = StartCoroutine(BossFight());
            yield return new WaitWhile(() => bossFight);
        }
        nextWave = true;
    }

    private IEnumerator BossFight() {
        GameObject boss = Instantiate(bosses[bossIndex], new Vector3(0, 3.5f, 0), Quaternion.identity);
        GameObject waveContainerObject = Instantiate(waveContainerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        waveContainerObject.name = "BossContainer";
        WaveContainer waveContainer = waveContainerObject.GetComponent<WaveContainer>();
        boss.transform.parent = waveContainerObject.transform;
        EnemyShip bossShip = boss.GetComponent<EnemyShip>();
        bossShip.SetHealthOnSpawn(0);
        yield return null;
    }

    public bool BossSpawned() {
        return bossFight;
    }

    private void SpawnSpecialtyWave(WaveConfig waveToSpawn) {
        GameObject waveContainerObject = Instantiate(waveContainerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        waveContainerObject.name = "SpecialContainer";
        GameObject specialEnemy = Instantiate(specialtySpawns[specialSpawnIndex], new Vector3(0, 5.75f, 0), Quaternion.identity);
        WaveContainer waveContainer = waveContainerObject.GetComponent<WaveContainer>();
        specialEnemy.transform.parent = waveContainerObject.transform;
        EnemyShip enemyShip = specialEnemy.GetComponent<EnemyShip>();
        enemyShip.SetHealthOnSpawn(game.GetHealthScaling());
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
        }
    }
}
