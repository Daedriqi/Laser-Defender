using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShip : MonoBehaviour {
    [Header("Enemy")]
    [SerializeField] int enemyHealth = 5;
    [SerializeField] int points = 125;
    [SerializeField] bool specialtyTypeEnemy = false;
    [SerializeField] bool isBoss = false;
    [SerializeField] Vector3 shieldScale = new Vector3(2, 2, 0);

    [Header("Projectiles")]
    [SerializeField] float shootrandomRangeMin = 0.5f;
    [SerializeField] float shootRandomRange = 5f;
    [SerializeField] GameObject enemyProjectile;
    [SerializeField] float projectileSpeed = 2.5f;
    [SerializeField] int projectileQuantity = 1;
    [SerializeField] bool homingShot = false;
    [SerializeField] int addedDamage = 0;

    [Header("Effects")]
    [SerializeField] GameObject explosionVFX;

    WaveContainer waveContainer;
    Coroutine shootOnDelay;
    Game game;
    DamageDealer damageDealer;
    GameObject player;

    int currentHitsLeft;
    float randomWaitTime;
    bool dead = false;
    bool canShoot = true;
    bool specialtyEnemyPresent = false;
    bool immuneToDamage = true;
    bool isShielded = false;

    // Start is called before the first frame update
    void Start() {
        player = FindObjectOfType<PlayerShip>().gameObject;
        waveContainer = transform.parent.GetComponent<WaveContainer>();
        game = FindObjectOfType<Game>();
    }

    private IEnumerator ShootOnDelay() {
        if (game.GetGameState() == Game.GameState.Playing && !specialtyTypeEnemy) {
            canShoot = false;
            randomWaitTime = UnityEngine.Random.Range(shootrandomRangeMin, shootRandomRange);
            yield return new WaitForSeconds(randomWaitTime);
            Vector3 instantiatePos = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
            GameObject projectile1 = Instantiate(enemyProjectile, instantiatePos, Quaternion.identity);
            damageDealer = projectile1.GetComponent<DamageDealer>();
            damageDealer.SetDamage(addedDamage);
            AudioSource.PlayClipAtPoint(damageDealer.GetSound(), Camera.main.transform.position, damageDealer.GetVolume());
            if (homingShot) {
                projectile1.GetComponent<Rigidbody2D>().velocity = new Vector2(player.transform.position.x - transform.position.x, -projectileSpeed);
            }
            else {
                projectile1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
            }
            if (projectileQuantity > 2) {
                GameObject projectile2 = Instantiate(enemyProjectile, instantiatePos, Quaternion.identity);
                damageDealer = projectile2.GetComponent<DamageDealer>();
                damageDealer.SetDamage(addedDamage);
                projectile2.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, -projectileSpeed);
                GameObject projectile3 = Instantiate(enemyProjectile, instantiatePos, Quaternion.identity);
                damageDealer = projectile3.GetComponent<DamageDealer>();
                damageDealer.SetDamage(addedDamage);
                projectile3.GetComponent<Rigidbody2D>().velocity = new Vector2(1, -projectileSpeed);
            }
            canShoot = true;
        }
    }

    // Update is called once per frame
    void Update() {
        //checks if they are on the screen and makes them damageable if they are on screen
        if (immuneToDamage) {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            if (onScreen) {
                MakeDamagable();
            }
        }
        if (canShoot && game.GetGameState() == Game.GameState.Playing) {
            shootOnDelay = StartCoroutine(ShootOnDelay());
        }
    }

    public int GetHealth() {
        return currentHitsLeft;
    }

    public GameObject GetProjectile() {
        return enemyProjectile;
    }

    public float GetProjectileSpeed() {
        return projectileSpeed;
    }

    public void MakeDamagable() {
        immuneToDamage = false;
    }

    public void SetHealthOnSpawn(int health) {
        enemyHealth += health;
        currentHitsLeft = enemyHealth;
    }

    public bool IsSpecialtyType() {
        return specialtyTypeEnemy;
    }

    public bool GetIsShielded() {
        return isShielded;
    }

    public void SetShielded() {
        isShielded = true;
    }

    public Vector3 GetShieldScale() {
        return shieldScale;
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
                if (isBoss) {
                    BossFight bossFight = FindObjectOfType<BossFight>();
                    bossFight.DeathAnimation();
                }
                else {
                    this.waveContainer.UpdateEnemiesDestroyed();
                    if (this.waveContainer.SpawnPowerUp()) {
                        GameObject powerUp = Instantiate(waveContainer.GetWave().GetPowerUp(), transform.position, Quaternion.identity);
                        powerUp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -2);
                    }
                    GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<TextMeshProUGUI>().text = score.ToString();
                    GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }
    }
}
