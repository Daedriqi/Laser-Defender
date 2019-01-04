using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour {
    [SerializeField] BossType bossName;
    [SerializeField] float bossSpeed = 3f;
    [SerializeField] Vector3 startingPoint;
    [SerializeField] GameObject path;
    [SerializeField] List<GameObject> powerUps;

    Game game;
    List<Transform> waypoints;
    EnemyShip bossShip;

    bool movedIntoStartPosition = false;
    int waypointIndex = 0;
    bool canShoot = false;
    float randomWaitTime;

    // Start is called before the first frame update
    void Start() {
        bossShip = GetComponent<EnemyShip>();
        game = FindObjectOfType<Game>();
        game.PlayBossMusic();
        SetWaypoints();
    }

    // Update is called once per frame
    void Update() {
        if (game.GetGameState() == Game.GameState.Playing) {
            if (!movedIntoStartPosition) {
                MoveIntoPosition();
            }
            else {
                MoveOnPath();
            }
        }
    }

    private void MoveOnPath() {
        float deltaSpeed = Time.deltaTime * bossSpeed;
        if (transform.position != waypoints[waypointIndex].position) {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].position, deltaSpeed);
        }
        else {
            waypointIndex++;
        }
        if (waypointIndex > waypoints.Count - 1) {
            waypointIndex = 0;
        }
    }

    private void SetWaypoints() {
        waypoints = new List<Transform>();
        foreach (Transform transform in path.transform) {
            waypoints.Add(transform);
        }
    }

    private void MoveIntoPosition() {
        float deltaSpeed = Time.deltaTime * bossSpeed;
        if (transform.position != startingPoint) {
            transform.position = Vector3.MoveTowards(transform.position, startingPoint, deltaSpeed);
        }
        else {
            movedIntoStartPosition = true;
        }
    }

    public enum BossType {
        Tornado
    }

    public void DeathAnimation() {
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        spawner.BossDead();
        Destroy(gameObject);
    }
}
