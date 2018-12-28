using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShip : MonoBehaviour {
    [Header("Enemy")]
    [SerializeField] int enemyHealth = 5;
    [SerializeField] int points = 125;

    [Header("Projectiles")]
    [SerializeField] float shootRandomRange = 5f;
    [SerializeField] GameObject enemyProjectile;
    [SerializeField] float projectileSpeed = 2.5f;

    [Header("Effects")]
    [SerializeField] GameObject explosionVFX;

    Game game;
    int currentHitsLeft;
    float randomWaitTime;

    // Start is called before the first frame update
    void Start() {
        game = FindObjectOfType<Game>();
        currentHitsLeft = enemyHealth;
        randomWaitTime = UnityEngine.Random.Range(0.0f, shootRandomRange);
        StartCoroutine(ShootOnDelay());
    }

    private IEnumerator ShootOnDelay() {
        yield return new WaitForSeconds(randomWaitTime);
        if (game.GetGameState() == Game.GameState.Playing) {
            Vector3 instantiatePos = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
            GameObject projectile1 = Instantiate(enemyProjectile, instantiatePos, Quaternion.identity);
            projectile1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        }
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "PlayerBullet") {
            currentHitsLeft -= 1;
            Destroy(collision.gameObject);
            if (currentHitsLeft <= 0) {
                int score = game.AddToScore(points);
                Text[] texts = FindObjectsOfType<Text>();
                for (int textIndex = 0; textIndex < texts.Length; textIndex++) {
                    if (texts[textIndex].gameObject.tag == "Scoreboard") {
                        texts[textIndex].text = "Score: " + score;
                    }
                }
                GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
