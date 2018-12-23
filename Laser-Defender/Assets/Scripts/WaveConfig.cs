using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject {
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject path;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.5f;
    [SerializeField] float enemySpeed = 2f;
    [SerializeField] int enemyCount = 5;

    public GameObject GetEnemyPrefab() {
        return enemyPrefab;
    }

    public List<Transform> GetWaypoints() {
        List<Transform> points = new List<Transform>();
        foreach (Transform child in path.transform) {
            points.Add(child);
        }
        return points;
    }

    public float GetTimeBetweenSpawns() {
        return timeBetweenSpawns;
    }

    public float GetSpawnRandomFactor() {
        return spawnRandomFactor;
    }

    public float GetEnemySpeed() {
        return enemySpeed;
    }

    public int GetEnemyCount() {
        return enemyCount;
    }
}
