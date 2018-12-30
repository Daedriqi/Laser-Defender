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

    WaveContainer waveContainer;
    Coroutine shootOnDelay;
    Game game;
    int currentHitsLeft;
    float randomWaitTime;
    bool dead = false;
    bool immuneToDamage = true;

    // Start is called before the first frame update
    void Start() {
        waveContainer = transform.parent.GetComponent<WaveContainer>();
        game = FindObjectOfType<Game>();
        shootOnDelay = StartCoroutine(ShootOnDelay());
    }

    private IEnumerator ShootOnDelay() {
        while (true) {
            randomWaitTime = UnityEngine.Random.Range(0.5f, shootRandomRange);
            yield return new WaitForSeconds(randomWaitTime);
            if (game.GetGameState() == Game.GameState.Playing) {
                Vector3 instantiatePos = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
                GameObject projectile1 = Instantiate(enemyProjectile, instantiatePos, Quaternion.identity);
                projectile1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void MakeDamagable() {
        immuneToDamage = false;
    }

    private void OnDestroy() {
        StopCoroutine(shootOnDelay);
    }

    public void SetHealthOnSpawn(int health) {
        enemyHealth = enemyHealth + health;
        currentHitsLeft = enemyHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if ((collision.gameObject.tag == "PlayerBullet" || collision.gameObject.tag == "Destructor") && !dead && !immuneToDamage) {
            DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
            currentHitsLeft -= damageDealer.GetDamage();
            if (collision.gameObject.tag != "Destructor") {
                Destroy(collision.gameObject);
            }
            if (currentHitsLeft <= 0) {
                dead = true;
                int score = game.AddToScore(points);
                this.waveContainer.UpdateEnemiesDestroyed();
                if (this.waveContainer.SpawnPowerUp()) {
                    GameObject powerUp = Instantiate(waveContainer.GetWave().GetPowerUp(), transform.position, Quaternion.identity);
                    powerUp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -2);
                }
                Text[] texts = FindObjectsOfType<Text>();
                for (int textIndex = 0; textIndex < texts.Length; textIndex++) {
                    if (texts[textIndex].gameObject.tag == "Scoreboard") {
                        texts[textIndex].text = score.ToString();
                    }
                }
                GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
