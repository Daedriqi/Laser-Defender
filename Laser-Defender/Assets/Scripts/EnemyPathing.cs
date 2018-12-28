using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour {
    WaveConfig wave;
    List<Transform> waypoints;
    Game game;
    int waypointsIndex = 0;
    float enemySpeed;
    GameObject path;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (game.GetGameState() == Game.GameState.Playing) {
            MoveEnemy();
        }
    }

    public void SetWave(WaveConfig waveToSet) {
        wave = waveToSet;
        game = FindObjectOfType<Game>();
        enemySpeed = wave.GetEnemySpeed();
        waypoints = wave.GetWaypoints()[0];
    }

    public void SetWaypoints(List<Transform> waypoints) {
        this.waypoints = waypoints;
        transform.position = waypoints[0].position;
    }

    public WaveConfig GetPathingWave() {
        return wave;
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
