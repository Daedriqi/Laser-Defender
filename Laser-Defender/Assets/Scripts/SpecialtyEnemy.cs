using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialtyEnemy : MonoBehaviour {
    [SerializeField] SpecialtyWaveType type;
    [SerializeField] GameObject specialtyPath;
    [SerializeField] float enemySpeed = 2f;
    [SerializeField] GameObject shieldForEnemies;

    List<Transform> waypoints;
    Game game;

    int waypointIndex = 0;
    bool enemyAlive = true;

    // Start is called before the first frame update
    void Start() {
        game = FindObjectOfType<Game>();
        GetWaypoints();
        transform.position = waypoints[0].position;
    }

    // Update is called once per frame
    void Update() {
        if (game.GetGameState() == Game.GameState.Playing) {
            FollowPath();
            if (type == SpecialtyWaveType.Bomb) {
                BombProcess();
            }
            if (type == SpecialtyWaveType.Shielder) {
                ShielderProcess();
            }
            if (type == SpecialtyWaveType.BigBomb) {
                BigBombProcess();
            }
        }
    }

    private void FollowPath() {
        if (waypointIndex < waypoints.Count) {
            float deltaSpeed = Time.deltaTime * enemySpeed;
            transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].position, deltaSpeed);
            if (transform.position == waypoints[waypointIndex].position) {
                waypointIndex++;
            }
        }
        else {
            waypointIndex = 2;
        }
    }

    private void GetWaypoints() {
        waypoints = new List<Transform>();
        foreach (Transform transform in specialtyPath.transform) {
            waypoints.Add(transform);
        }
    }

    private void BigBombProcess() {
    }

    private void ShielderProcess() {
        EnemyShip[] enemies = FindObjectsOfType<EnemyShip>();
        foreach (EnemyShip enemy in enemies) {
            if (!enemy.IsSpecialtyType() && !enemy.GetIsShielded()) {
                Vector3 shieldPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 5);
                GameObject shield = Instantiate(shieldForEnemies, enemy.transform.position, Quaternion.identity);
                shield.transform.parent = enemy.transform;
                enemy.SetShielded();
            }
        }
    }

    private void BombProcess() {

    }

    public SpecialtyWaveType GetSpecialtyType() {
        return type;
    }

    public enum SpecialtyWaveType {
        Bomb,
        Shielder,
        BigBomb,
    }
}
