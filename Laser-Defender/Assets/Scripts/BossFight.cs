using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour {
    [SerializeField] float bossSpeed = 3f;
    [SerializeField] Vector3 startingPoint;
    [SerializeField] GameObject path;
    [SerializeField] List<GameObject> powerUps;
    [SerializeField] GameObject explosionPattern;

    Game game;
    List<Transform> waypoints;
    List<Transform> explosionPoints;
    EnemyShip bossShip;

    bool movedIntoStartPosition = false;
    int waypointIndex = 0;
    float randomWaitTime;

    // Start is called before the first frame update
    void Start() {
        bossShip = GetComponent<EnemyShip>();
        game = FindObjectOfType<Game>();
        game.PlayBossMusic();
        SetWaypoints();
        SetExplosionPoints();
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

    private void SetExplosionPoints() {
        explosionPoints = new List<Transform>();
        foreach (Transform transform in explosionPattern.transform) {
            explosionPoints.Add(transform);
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

    public void BossDeath() {
        StartCoroutine(DeathAnimation());
    }

    private IEnumerator DeathAnimation() {
        AudioClip clip;
        EnemyShip bossShip = GetComponent<EnemyShip>();
        bossShip.MakeDead();
        GameObject deathExplosionVFX = bossShip.GetExplosion();
        foreach (Transform point in explosionPoints) {
            float rand = UnityEngine.Random.Range(0.15f, 0.5f);
            GameObject explosion = Instantiate(deathExplosionVFX);
            explosion.transform.parent = transform;
            explosion.transform.position = point.position + transform.position;
            explosion.GetComponent<AudioSource>().volume = 0.5f;
            clip = explosion.GetComponent<AudioSource>().clip;
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            yield return new WaitForSeconds(rand);
        }
        GameObject finalExplosion = Instantiate(deathExplosionVFX, transform.position, Quaternion.identity);
        finalExplosion.transform.localScale = new Vector3(8, 8, 0);
        finalExplosion.GetComponent<AudioSource>().volume = 0.5f;
        finalExplosion.transform.parent = transform;
        clip = finalExplosion.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        yield return new WaitForSeconds(0.5f);
        Destroy(GetComponent<SpriteRenderer>());
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnPowerUps() {
        float xSpawn;
        float ySpawn;
        foreach (GameObject lootDrop in powerUps) {
            xSpawn = UnityEngine.Random.Range(-2.8f, 2.9f);
            ySpawn = UnityEngine.Random.Range(5.35f, 6f);
            GameObject powerUp = Instantiate(lootDrop, new Vector3(xSpawn, ySpawn, 0), Quaternion.identity);
            powerUp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -2);
            yield return new WaitForSeconds(UnityEngine.Random.Range(0, 0.25f));
        }
        Destroy(gameObject);
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        spawner.BossDead();
    }
}
