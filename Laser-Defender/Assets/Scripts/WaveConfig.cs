﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject {
    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] List<GameObject> powerUps;
    [SerializeField] List<GameObject> paths;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float enemySpeed = 2f;
    [SerializeField] int enemyCount = 5;

    GameObject enemyPrefab;

    int randomRange;


    public GameObject GetEnemyPrefab() {
        return enemyPrefab;
    }
    
    public void SetEnemyPrefab(int levelIndex) {
        enemyPrefab = enemyPrefabs[levelIndex];
    }

    public List<List<Transform>> GetWaypoints() {
        List<List<Transform>> points = new List<List<Transform>>();
        foreach (GameObject path in paths) {
            List<Transform> tempList = new List<Transform>();
            foreach (Transform child in path.transform) {
                tempList.Add(child);
            }
            points.Add(tempList);
        }
        return points;
    }

    public float GetTimeBetweenSpawns() {
        return timeBetweenSpawns;
    }

    public GameObject GetPowerUp() {
        int randomRange = Random.Range(0, powerUps.Count);
        return powerUps[randomRange];
    }

    public float GetEnemySpeed() {
        return enemySpeed;
    }

    public int GetEnemyCount() {
        return enemyCount;
    }
}
