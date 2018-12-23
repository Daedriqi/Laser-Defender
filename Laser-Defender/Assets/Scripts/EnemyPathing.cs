using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour {
    [SerializeField] WaveConfig wave;

    List<Transform> waypoints;
    int waypointsIndex = 0;
    float enemySpeed;
    GameObject path;

    // Start is called before the first frame update
    void Start() {
        waypoints = wave.GetWaypoints();
        transform.position = waypoints[0].position;
        enemySpeed = wave.GetEnemySpeed();
    }

    // Update is called once per frame
    void Update() {
        MoveEnemy();
    }

    public void SetWave(WaveConfig newWave) {
        wave = newWave;
    }

    private void MoveEnemy() {
        if (waypointsIndex < waypoints.Count) {
            float speedDelta = Time.deltaTime * enemySpeed;
            transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointsIndex].position, speedDelta);
            if (transform.position == waypoints[waypointsIndex].position) {
                waypointsIndex++;
            }
        }
        else {
            Destroy(gameObject);
        }
    }
}
