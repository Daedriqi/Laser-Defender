using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveContainer : MonoBehaviour {
    int enemiesDestroyed = 0;
    int enemiesToDestroy = 0;
    bool destroyable = false;
    Coroutine waitForSpawns;
    WaveConfig wave;

    // Start is called before the first frame update
    void Start() {

    }

    private IEnumerator WaitForSpawns() {
        yield return new WaitForSeconds((wave.GetTimeBetweenSpawns() * wave.GetEnemyCount()));
        destroyable = true;
        StopCoroutine(waitForSpawns);
    }

    // Update is called once per frame
    void Update() {
        if (destroyable && transform.childCount == 0) {
            Destroy(gameObject);
        }
    }

    public void SetWave(WaveConfig wave) {
        this.wave = wave;
        enemiesToDestroy = wave.GetWaypoints().Count * wave.GetEnemyCount();
        waitForSpawns = StartCoroutine(WaitForSpawns());
    }

    public WaveConfig GetWave() {
        return wave;
    }

    public void UpdateEnemiesDestroyed() {
        enemiesDestroyed++;
    }

    public bool SpawnPowerUp() {
        bool retVal = false;
        if (enemiesDestroyed == enemiesToDestroy) {
            retVal = true;
        }
        return retVal;
    }
}
