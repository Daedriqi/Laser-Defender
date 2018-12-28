using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour {
    [Header("Pathing")]
    [SerializeField] WaveConfig wave;
    [SerializeField] List<Transform> waypoints;

    Game game;
    int waypointsIndex = 0;
    float enemySpeed;
    GameObject path;

    // Start is called before the first frame update
    void Start() {
        game = FindObjectOfType<Game>();
        enemySpeed = wave.GetEnemySpeed();
    }

    // Update is called once per frame
    void Update() {
        if (game.GetGameState() == Game.GameState.Playing) {
            MoveEnemy();
        }
    }

    public void SetWave(WaveConfig waveToSet) {
        wave = waveToSet;
        enemySpeed = wave.GetEnemySpeed();
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
