using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShip : MonoBehaviour {
    //configuration parameters
    [Header("Player")]
    [SerializeField] float shipSpeed = 5f;
    [SerializeField] int playerHealth = 150;
    [SerializeField] float shieldCapacity = 150;
    [SerializeField] float damageImmunityTime = 2f;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] int bigBombsCount = 3;

    [Header("Projectiles")]
    [SerializeField] float projectileSpeed = 15f;
    [SerializeField] float shootDelay = 0.30f;
    [SerializeField] List<GameObject> plasmaBalls;
    [SerializeField] GameObject destructor;
    [SerializeField] float shotVolume = 0.1f;

    [Header("Effects")]
    [SerializeField] Sprite leftTurn;
    [SerializeField] Sprite rightTurn;
    [SerializeField] GameObject deathExplosionVFX;
    [SerializeField] AudioClip hitSound;


    //cache references
    Game game;
    SpriteRenderer spriteRenderer;
    Sprite defaultSprite;
    GameObject shield;
    HealthBarUI healthBar;
    BigBombUI bigBombUI;

    //state variables
    int currentBigBombsLeft;
    int currentHealthLeft;
    float currentShieldLeft;
    int currentBulletSizeIndex = 0;
    int maxBibBombs = 3;
    bool shieldUp = false;
    bool immuneToDamage = false;
    int defaultNumberOfBullets = 1;
    int numberOfBullets = 1;
    float defaultShootDelay;
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
        EventSystem.current.SetSelectedGameObject(null);
        healthBar = FindObjectOfType<HealthBarUI>();
        shield = Instantiate(shieldPrefab, new Vector3(-50, -50, -1), Quaternion.identity);
        currentShieldLeft = shieldCapacity;
        defaultShootDelay = shootDelay;
        currentHealthLeft = playerHealth;
        currentBigBombsLeft = bigBombsCount;
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        defaultSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        GetMoveBoundaries();
        bigBombUI = FindObjectOfType<BigBombUI>();
        bigBombUI.SetStartingXPos();
        bigBombUI.FillAmmoBar(currentBigBombsLeft);
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
            ShieldUp();
            MoveShip();
            Shoot();
        }
    }

    public int GetHealth() {
        return currentHealthLeft;
    }

    private void Shoot() {
        if (Input.GetButton("Fire1") && Time.time > timeBuffer) {
            timeBuffer = Time.time + shootDelay;
            if (numberOfBullets >= 1) {
                GameObject projectile1 = Instantiate(plasmaBalls[currentBulletSizeIndex], new Vector3(transform.position.x, transform.position.y + 0.5f, 0), Quaternion.identity);
                projectile1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            }
            if (numberOfBullets >= 5) {
                GameObject projectile2 = Instantiate(plasmaBalls[currentBulletSizeIndex], transform.position, Quaternion.identity);
                projectile2.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed / 20, projectileSpeed);
                GameObject projectile3 = Instantiate(plasmaBalls[currentBulletSizeIndex], transform.position, Quaternion.identity);
                projectile3.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed / 20, projectileSpeed);
            }
            if (numberOfBullets >= 9) {
                GameObject projectile4 = Instantiate(plasmaBalls[currentBulletSizeIndex], transform.position, Quaternion.identity);
                projectile4.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed / 10, projectileSpeed);
                GameObject projectile5 = Instantiate(plasmaBalls[currentBulletSizeIndex], transform.position, Quaternion.identity);
                projectile5.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed / 10, projectileSpeed);
            }
        }
        if (Input.GetButtonDown("Fire2")) {
            if (currentBigBombsLeft > 0) {
                BigBombBlast();
            }
        }
    }

    private void BigBombBlast() {
        bigBombUI.RemoveAmmo();
        currentBigBombsLeft -= 1;
        GameObject bigBomb = Instantiate(destructor, new Vector3(0, -5f, 0), Quaternion.identity);
        bigBomb.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10);
    }

    private void ShieldUp() {
        if (Input.GetButton("ShieldUp") && currentShieldLeft > 0) {
            currentShieldLeft -= 1.5f;
            healthBar.UpdateShieldBar(-1.5f);
            shield.transform.position = new Vector3(transform.position.x, transform.position.y, 1);
            shieldUp = true;
            if (currentShieldLeft <= 0) {
                shield.transform.position = new Vector3(-50, -50, 0);
                shieldUp = false;
            }
        }
        if (Input.GetButtonUp("ShieldUp")) {
            shield.transform.position = new Vector3(-50, -50, 0);
            shieldUp = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!immuneToDamage && collision.gameObject.tag.Contains("Enemy") && !shieldUp) {
            immuneToDamage = true;
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
            int damage = collision.GetComponent<DamageDealer>().GetDamage();
            StartCoroutine(DamagePlayer(damage));
        }
        if (collision.gameObject.tag == "PowerUp") {
            HandlePowerup(collision);
        }
    }

    public void IncreaseBulletSize() {
        if (currentBulletSizeIndex < plasmaBalls.Count - 1) {
            currentBulletSizeIndex++;
        }
    }

    public void UpdateBigBlastAmmo() {
        currentBigBombsLeft++;
        if (currentBigBombsLeft > maxBibBombs) {
            currentBigBombsLeft = maxBibBombs;
        }
        else {
            FindObjectOfType<BigBombUI>().FillAmmoBar(1);
        }
    }

    public void UpdatePlayerHealth(int amountToChange) {
        currentHealthLeft += amountToChange;
        if (currentHealthLeft > 150) {
            currentHealthLeft = 150;
        }
    }

    public void UpdatePlayerShield(int amountToChange) {
        currentShieldLeft += amountToChange;
        if (currentShieldLeft > 150) {
            currentShieldLeft = 150;
        }
    }

    public void IncreaseBulletQuantity() {
        numberOfBullets += 4;
        if (numberOfBullets > 12) {
            numberOfBullets = 12;
        }
    }

    public void DecreaseShootDelay() {
        shootDelay -= 0.05f;
        if (shootDelay < 0.1f) {
            shootDelay = 0.1f;
        }
    }

    private void HandlePowerup(Collider2D collision) {
        collision.GetComponent<PowerUp>().GetPowerUpEffect(gameObject);
        Destroy(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!immuneToDamage && collision.gameObject.tag == "Enemy" && !shieldUp) {
            immuneToDamage = true;
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
            int damage = collision.GetComponent<DamageDealer>().GetDamage();
            StartCoroutine(DamagePlayer(damage));
        }
    }

    public IEnumerator DamagePlayer(int damage) {
        healthBar.UpdateHealthBar(-damage);
        UpdatePlayerHealth(-damage);
        StartCoroutine(flashSprite());
        if (numberOfBullets > defaultNumberOfBullets) {
            numberOfBullets -= 1;
        }
        if (currentBulletSizeIndex > 0) {
            currentBulletSizeIndex--;
        }
        if (shootDelay < defaultShootDelay) {
            shootDelay += 0.05f;
        }
        if (currentHealthLeft <= 0) {
            currentHealthLeft -= 9999999;
            StopAllCoroutines();
            StartCoroutine(DeathAnimation());
        }
        yield return new WaitForSeconds(damageImmunityTime);
        StopCoroutine(DamagePlayer(damage));
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
        if (!spriteRenderer.enabled) {
            spriteRenderer.enabled = true;
        }
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
