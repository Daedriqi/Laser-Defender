using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] List<WaveConfig> waveConfigs;

    int waveIndex = 0;
    bool nextWave = false;
    WaveConfig currentWave;

    // Start is called before the first frame update
    void Start() {
        currentWave = waveConfigs[0];
        StartCoroutine(SpawnAllEnimiesInWave(currentWave));
    }

    // Update is called once per frame
    void Update() {
        if (nextWave) {
            nextWave = false;
            StopCoroutine(SpawnAllEnimiesInWave(currentWave));
            StartCoroutine(SpawnAllEnimiesInWave(waveConfigs[waveIndex]));
        }
    }

    private IEnumerator SpawnAllEnimiesInWave(WaveConfig waveToSpawn) {
        int enemiesSpawned = 0;
        while (enemiesSpawned < waveToSpawn.GetEnemyCount()) {
            GameObject enemy = waveToSpawn.GetEnemyPrefab();
            enemy.GetComponent<EnemyPathing>().SetWave(waveToSpawn);
            Instantiate(enemy, waveToSpawn.GetWaypoints()[0].position, Quaternion.identity);
            enemiesSpawned++;
            yield return new WaitForSeconds(waveToSpawn.GetTimeBetweenSpawns());
        }
        if (waveIndex >= waveConfigs.Count - 1) {
            waveIndex = -1;
        }
        waveIndex++;
        yield return new WaitForSeconds(2);
        nextWave = true;
    }
}
