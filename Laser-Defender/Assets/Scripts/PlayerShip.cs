using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour {
    //configuration parameters
    [Header("Player")]
    [SerializeField] float shipSpeed = 5f;
    [SerializeField] int playerHealth = 5;
    [SerializeField] float damageImmunityTime = 2f;

    [Header("Projectiles")]
    [SerializeField] float projectileSpeed = 15f;
    [SerializeField] float shootDelay = 0.05f;
    [SerializeField] GameObject plasmaBall;

    [Header("Effects")]
    [SerializeField] Sprite leftTurn;
    [SerializeField] Sprite rightTurn;
    [SerializeField] GameObject deathExplosionVFX;
    [SerializeField] AudioClip hitSound;

    List<GameObject> explostions;
    int currentHealthLeft;
    bool immuneToDamage = false;
    HUD Hud;

    //cache references
    Game game;
    SpriteRenderer spriteRenderer;
    Sprite defaultSprite;

    //state variables
    float timeBuffer;
    float paddingLeftRight = 0.35f;
    float paddingTop = 4f;
    float paddingBottom = 0.5f;
    float minX;
    float maxX;
    float minY;
    float maxY;

    // Start is called before the first frame update
    void Start() {
        Hud = FindObjectOfType<HUD>();
        currentHealthLeft = playerHealth;
        game = FindObjectOfType<Game>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        defaultSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        GetMoveBoundaries();
        Hud.FillHealthBar();
    }

    private void GetMoveBoundaries() {
        Camera camera = Camera.main;
        minX = camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + paddingLeftRight;
        maxX = camera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - paddingLeftRight;
        minY = camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + paddingBottom;
        maxY = camera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - paddingTop;
    }

    // Update is called once per frame
    void Update() {
        if (game.GetGameState() == Game.GameState.Playing) {
            MoveShip();
            Shoot();
        }
    }

    public int GetHealth() {
        return currentHealthLeft;
    }

    private void Shoot() {
        if (Input.GetButton("Fire1") && true && Time.time > timeBuffer) {
            timeBuffer = Time.time + shootDelay;
            GameObject projectile1 = Instantiate(plasmaBall, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), Quaternion.identity);
            projectile1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            //shooting in 3 directions at once in a spray
            //GameObject projectile2 = Instantiate(plasmaBall, transform.position, Quaternion.identity);
            //projectile2.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, projectileSpeed);
            //GameObject projectile3 = Instantiate(plasmaBall, transform.position, Quaternion.identity);
            //projectile3.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, projectileSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!immuneToDamage && collision.gameObject.tag.Contains("Enemy")) {
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
            immuneToDamage = true;
            StartCoroutine(DamagePlayer(1));
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!immuneToDamage && collision.gameObject.tag == "Enemy") {
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
            immuneToDamage = true;
            StartCoroutine(DamagePlayer(1));
        }
    }

    public IEnumerator DamagePlayer(int damage) {
        Hud.HealthLost();
        if (currentHealthLeft - damage <= 0) {
            StopAllCoroutines();
            StartCoroutine(DeathAnimation());
        }
        else {
            StartCoroutine(flashSprite());
            currentHealthLeft -= damage;
        }
        yield return new WaitForSeconds(damageImmunityTime);
        StopCoroutine(DamagePlayer(1));
        immuneToDamage = false;
    }

    private IEnumerator flashSprite() {
        while (immuneToDamage) {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        StopCoroutine(flashSprite());
    }

    private IEnumerator DeathAnimation() {
        game.SetTimeScale(0.35f);
        StartCoroutine(DeathExplostions());
        yield return new WaitForSeconds(0.75f);
        game.SetGameState(Game.GameState.GameOver);
        Destroy(gameObject);
    }

    private IEnumerator DeathExplostions() {
        AudioClip clip;
        Vector3 explosion1Position = new Vector3(transform.position.x - 0.1578f, transform.position.y - 0.118f, transform.position.z - 1);
        GameObject explostion1 = Instantiate(deathExplosionVFX, explosion1Position, Quaternion.identity);
        explostion1.GetComponent<AudioSource>().volume = 0.25f;
        explostion1.transform.parent = transform;
        clip = explostion1.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        yield return new WaitForSeconds(0.1f);
        Vector3 explosion2Position = new Vector3(transform.position.x - 0.1f, transform.position.y + 0.245f, transform.position.z - 1);
        GameObject explostion2 = Instantiate(deathExplosionVFX, explosion2Position, Quaternion.identity);
        explostion2.GetComponent<AudioSource>().volume = 0.5f;
        explostion2.transform.parent = transform;
        clip = explostion2.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        yield return new WaitForSeconds(0.15f);
        Vector3 explosion3Position = new Vector3(transform.position.x + 0.234f, transform.position.y - 0.045f, transform.position.z - 1);
        GameObject explostion3 = Instantiate(deathExplosionVFX, explosion3Position, Quaternion.identity);
        explostion3.GetComponent<AudioSource>().volume = 0.5f;
        explostion3.transform.parent = transform;
        clip = explostion3.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        yield return new WaitForSeconds(0.2f);
        Vector3 explosion4Position = new Vector3(transform.position.x + 0.038f, transform.position.y + 0.067f, transform.position.z - 1);
        GameObject explostion4 = Instantiate(deathExplosionVFX, explosion4Position, Quaternion.identity);
        explostion4.GetComponent<AudioSource>().volume = 1;
        explostion4.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        explostion4.transform.parent = transform;
        clip = explostion4.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    private void MoveShip() {
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * shipSpeed;
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * shipSpeed;
        if (spriteRenderer.enabled) {
            if (deltaX > 0) {
                spriteRenderer.sprite = rightTurn;
            }
            else if (deltaX < 0) {
                spriteRenderer.sprite = leftTurn;
            }
            else {
                spriteRenderer.sprite = defaultSprite;
            }
        }
        float newXPos = transform.position.x + deltaX;
        float newYPos = transform.position.y + deltaY;
        transform.position = new Vector2(Mathf.Clamp(newXPos, minX, maxX), Mathf.Clamp(newYPos, minY, maxY));
    }
}
