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
    int currentRound = 0;
    bool nextWave = true;
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
    }

    // Update is called once per frame
    void Update() {
        if (game == null) {
            game = FindObjectOfType<Game>();
        }
        if (nextWave && game.GetGameState() == Game.GameState.Playing) {
            nextWave = false;
            if (spawnAllEnemiesForPath != null) {
                StopCoroutine(spawnAllEnemiesForPath);
            }
            if (spawnAllEnemiesInWave != null) {
                StopCoroutine(spawnAllEnemiesInWave);
            }
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
        waveIndex++;
        yield return new WaitForSeconds((waveToSpawn.GetTimeBetweenSpawns() * waveToSpawn.GetEnemyCount()) + 1);
        if (!bossFight) {
            if (waveCount >= 5) {
                game.RoundComplete();
                currentRound++;
                waveCount = 0;
            }
            if (currentRound >= roundsToBossFight) {
                bossFight = true;
                BossFight();
            }
        }
        if (bossFight) {
            if (waveCount >= 5) {
                waveCount = 0;
            }
            yield return new WaitForSeconds(15);
        }
        if (levelIndex >= 1) {
            Debug.Log("Level index is now more than 1");
            SpawnSpecialtyWave();
        }
        nextWave = true;
    }

    private void BossFight() {
        GameObject boss = Instantiate(bosses[bossIndex], new Vector3(0, 6.5f, 0), Quaternion.identity);
        GameObject waveContainerObject = Instantiate(waveContainerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        waveContainerObject.name = "BossContainer";
        WaveContainer waveContainer = waveContainerObject.GetComponent<WaveContainer>();
        boss.transform.parent = waveContainerObject.transform;
        EnemyShip bossShip = boss.GetComponent<EnemyShip>();
        bossShip.SetHealthOnSpawn(0);
    }

    public void BossDead() {
        StopCoroutine(spawnAllEnemiesInWave);
        StopCoroutine(spawnAllEnemiesForPath);
        bossFight = false;
        if (!game) {
            game = FindObjectOfType<Game>();
        }
        game.PlayGameMusic();
        currentRound = 0;
        waveCount = 0;
        waveIndex = 0;
        levelIndex++;
        bossIndex++;
        StartCoroutine(BreakTime());
    }

    private IEnumerator BreakTime() {
        yield return new WaitForSeconds(5);
        nextWave = true;
    }

    public int GetLevelIndex() {
        return levelIndex;
    }

    private void SpawnSpecialtyWave() {
        GameObject waveContainerObject1 = Instantiate(waveContainerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        waveContainerObject1.name = "SpecialContainer";
        GameObject specialEnemy1 = Instantiate(specialtySpawns[0], new Vector3(0, 5.75f, 0), Quaternion.identity);
        WaveContainer waveContainer1 = waveContainerObject1.GetComponent<WaveContainer>();
        specialEnemy1.transform.parent = waveContainerObject1.transform;
        EnemyShip enemyShip1 = specialEnemy1.GetComponent<EnemyShip>();
        enemyShip1.SetHealthOnSpawn(game.GetHealthScaling());

        if (levelIndex >= 3) {
            Debug.Log("Level index is now more than 3");
            GameObject waveContainerObject2 = Instantiate(waveContainerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            waveContainerObject2.name = "SpecialContainer";
            GameObject specialEnemy2 = Instantiate(specialtySpawns[1], new Vector3(0, 5.75f, 0), Quaternion.identity);
            WaveContainer waveContainer2 = waveContainerObject2.GetComponent<WaveContainer>();
            specialEnemy2.transform.parent = waveContainerObject2.transform;
            EnemyShip enemyShip2 = specialEnemy2.GetComponent<EnemyShip>();
            enemyShip2.SetHealthOnSpawn(game.GetHealthScaling());
        }
        if (levelIndex >= 5) {
            Debug.Log("Level index is now more than 1");
            GameObject waveContainerObject3 = Instantiate(waveContainerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            waveContainerObject3.name = "SpecialContainer";
            GameObject specialEnemy3 = Instantiate(specialtySpawns[2], new Vector3(0, 5.75f, 0), Quaternion.identity);
            WaveContainer waveContainer3 = waveContainerObject3.GetComponent<WaveContainer>();
            specialEnemy3.transform.parent = waveContainerObject3.transform;
            EnemyShip enemyShip3 = specialEnemy3.GetComponent<EnemyShip>();
            enemyShip3.SetHealthOnSpawn(game.GetHealthScaling());
        }
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
