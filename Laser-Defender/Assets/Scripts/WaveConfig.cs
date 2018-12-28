using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject {
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] List<GameObject> powerUps;
    [SerializeField] List<GameObject> paths;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float enemySpeed = 2f;
    [SerializeField] int enemyCount = 5;

    int enemiesDestroyed = 0;

    public GameObject GetEnemyPrefab() {
        return enemyPrefab;
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

    public GameObject AllEnemiesDestroyed() {
        GameObject retVal = null;
        enemiesDestroyed++;
        if (enemiesDestroyed == enemyCount * paths.Count) {
            retVal = powerUps[0];
        }
        return retVal;
    }

    public void SetCounterZero() {
        enemiesDestroyed = 0;
    }

    public float GetEnemySpeed() {
        return enemySpeed;
    }

    public int GetEnemyCount() {
        return enemyCount;
    }
}
