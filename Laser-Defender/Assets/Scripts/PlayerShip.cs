using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShip : MonoBehaviour {
    //configuration parameters
    [Header("Player")]
    [SerializeField] int playerHealth = 150;
    [SerializeField] float shieldCapacity = 150;
    [SerializeField] float damageImmunityTime = 2f;
    [SerializeField] GameObject shieldPrefab;

    [Header("Movement")]
    [SerializeField] float shipSpeed = 5f;
    [SerializeField] float minXPos;
    [SerializeField] float maxXPos;
    [SerializeField] float minYPos;
    [SerializeField] float maxYPos;

    [Header("Projectiles")]
    [SerializeField] float projectileSpeed = 15f;
    [SerializeField] List<GameObject> plasmaBalls;
    [SerializeField] GameObject destructor;
    [SerializeField] int maxBulletQuantity = 5;
    [SerializeField] int maxBulletPowerIndex = 3;
    [SerializeField] float minBulletDelay = 0.15f;
    [SerializeField] int maxBigBombs = 3;
    [SerializeField] int defaultBulletPowerIndex = 0;
    [SerializeField] int defaultBulletQuantity = 1;
    [SerializeField] int defaultBigBombQuantity = 3;
    [SerializeField] float defaultShootDelay = 0.3f;

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
    AudioClip shotSound;
    DamageDealer damageDealer;

    //state variables
    float currentShieldLeft;
    float currentShootDelay;
    int currentBigBombsLeft;
    int currentHealthLeft;
    int currentBulletPowerIndex;
    bool shieldUp = false;
    int numberOfBullets = 1;
    float timeBuffer;
    float paddingLeftRight = 0.35f;
    float paddingTop = 4f;
    float paddingBottom = 0.5f;
    //serialized for testing
    [SerializeField] bool immuneToDamage = false;


    // Start is called before the first frame update
    void Start() {
        bigBombUI = FindObjectOfType<BigBombUI>();
        EventSystem.current.SetSelectedGameObject(null);
        healthBar = FindObjectOfType<HealthBarUI>();
        shield = Instantiate(shieldPrefab, new Vector3(-50, -50, -1), Quaternion.identity);
        currentShieldLeft = shieldCapacity;
        currentHealthLeft = playerHealth;
        currentShootDelay = defaultShootDelay;
        currentBigBombsLeft = defaultBigBombQuantity;
        currentBulletPowerIndex = defaultBulletPowerIndex;
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        defaultSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        GetMoveBoundaries();
        bigBombUI = FindObjectOfType<BigBombUI>();
        bigBombUI.FillAmmoBar(currentBigBombsLeft);
    }

    private void GetMoveBoundaries() {
        minXPos = -3f;
        maxXPos = 3f;
        minYPos = -4.5f;
        maxYPos = 3f;
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
            damageDealer = plasmaBalls[currentBulletPowerIndex].GetComponent<DamageDealer>();
            AudioSource.PlayClipAtPoint(damageDealer.GetSound(), Camera.main.transform.position, damageDealer.GetVolume());
            timeBuffer = Time.time + currentShootDelay;
            if (numberOfBullets >= 1) {
                GameObject projectile1 = Instantiate(plasmaBalls[currentBulletPowerIndex], new Vector3(transform.position.x, transform.position.y + 0.5f, 0), Quaternion.identity);
                projectile1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            }
            if (numberOfBullets >= 3) {
                GameObject projectile2 = Instantiate(plasmaBalls[currentBulletPowerIndex], new Vector3(transform.position.x + 0.25f, transform.position.y + 0.25f, 0), Quaternion.identity);
                projectile2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
                GameObject projectile3 = Instantiate(plasmaBalls[currentBulletPowerIndex], new Vector3(transform.position.x - 0.25f, transform.position.y + 0.25f, 0), Quaternion.identity);
                projectile3.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            }
            if (numberOfBullets >= 5) {
                GameObject projectile4 = Instantiate(plasmaBalls[currentBulletPowerIndex], new Vector3(transform.position.x + 0.5f, transform.position.y + 0.25f, 0), Quaternion.Euler(0, 0, -40));
                projectile4.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed * 2, projectileSpeed);
                GameObject projectile5 = Instantiate(plasmaBalls[currentBulletPowerIndex], new Vector3(transform.position.x - 0.5f, transform.position.y + 0.25f, 0), Quaternion.Euler(0, 0, 40));
                projectile5.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed * 2, projectileSpeed);
            }
            if (numberOfBullets >= 7) {
                GameObject projectile4 = Instantiate(plasmaBalls[currentBulletPowerIndex], new Vector3(transform.position.x + 0.5f, transform.position.y + 0.25f, 0), Quaternion.identity);
                projectile4.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
                GameObject projectile5 = Instantiate(plasmaBalls[currentBulletPowerIndex], new Vector3(transform.position.x - 0.5f, transform.position.y + 0.25f, 0), Quaternion.identity);
                projectile5.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            }
            if (numberOfBullets >= 9) {
                GameObject projectile4 = Instantiate(plasmaBalls[currentBulletPowerIndex], new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, 0), Quaternion.Euler(0, 0, -40));
                projectile4.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed * 2, projectileSpeed);
                GameObject projectile5 = Instantiate(plasmaBalls[currentBulletPowerIndex], new Vector3(transform.position.x - 0.5f, transform.position.y + 0.5f, 0), Quaternion.Euler(0, 0, 40));
                projectile5.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed * 2, projectileSpeed);
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
        damageDealer = bigBomb.GetComponent<DamageDealer>();
        AudioSource.PlayClipAtPoint(damageDealer.GetSound(), Camera.main.transform.position, damageDealer.GetVolume());
        bigBomb.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10);
    }

    private void ShieldUp() {
        if (Input.GetButton("ShieldUp") && currentShieldLeft > 0) {
            UpdatePlayerShield(-0.5f);
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
        if (currentBulletPowerIndex < maxBulletPowerIndex && currentBulletPowerIndex < plasmaBalls.Count - 1) {
            currentBulletPowerIndex++;
        }
    }

    public void UpdateBigBlastAmmo() {
        if (currentBigBombsLeft < maxBigBombs) {
            currentBigBombsLeft++;
            bigBombUI.FillAmmoBar(1);
        }
    }

    public void UpdatePlayerHealth(int amountToChange) {
        currentHealthLeft += amountToChange;
        healthBar.UpdateHealthBar(amountToChange);
        if (currentHealthLeft > 150) {
            currentHealthLeft = 150;
        }
    }

    public void UpdatePlayerShield(float amountToChange) {
        currentShieldLeft += amountToChange;
        healthBar.UpdateShieldBar(amountToChange);
        if (currentShieldLeft > 150) {
            currentShieldLeft = 150;
        }
    }

    public void IncreaseBulletQuantity() {
        if (numberOfBullets < maxBulletQuantity) {
            numberOfBullets += 2;
        }
    }

    public void DecreaseShootDelay() {
        if (currentShootDelay > minBulletDelay) {
            currentShootDelay -= 0.035f;
        }
    }

    public void AddToMaxBulletQuantity() {
        maxBulletQuantity += 2;
    }

    public void AddToMaxBulletPowerIndex() {
        maxBulletPowerIndex++;
    }

    public void SubtractFromMinBulletDelay() {
        if (minBulletDelay > 0.15f) {
            minBulletDelay -= 0.15f;
        }
    }

    public void AddToMaxBigBombs() {
        maxBigBombs++;
    }

    public void IncreasePlayerStats() {
        AddToMaxBigBombs();
        AddToMaxBulletPowerIndex();
        AddToMaxBulletQuantity();
        SubtractFromMinBulletDelay();
    }

    public void ResetDefaultStats() {
        maxBigBombs = defaultBigBombQuantity;
        maxBulletPowerIndex = defaultBulletPowerIndex;
        maxBulletQuantity = defaultBulletQuantity;
        minBulletDelay = defaultShootDelay;
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
        UpdatePlayerHealth(-damage);
        StartCoroutine(flashSprite());
        if (numberOfBullets > defaultBulletQuantity) {
            numberOfBullets -= 1;
        }
        if (currentBulletPowerIndex > 0) {
            currentBulletPowerIndex--;
        }
        if (currentShootDelay < defaultShootDelay) {
            currentShootDelay += 0.05f;
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
        StartCoroutine(DeathExplosions());
        yield return new WaitForSeconds(0.75f);
        game.SetGameState(Game.GameState.GameOver);
        Destroy(gameObject);
    }

    private IEnumerator DeathExplosions() {
        AudioClip clip;
        Vector3 explosion1Position = new Vector3(transform.position.x - 0.1578f, transform.position.y - 0.118f, transform.position.z - 1);
        GameObject explosion1 = Instantiate(deathExplosionVFX, explosion1Position, Quaternion.identity);
        explosion1.GetComponent<AudioSource>().volume = 0.25f;
        explosion1.transform.parent = transform;
        clip = explosion1.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        yield return new WaitForSeconds(0.1f);
        Vector3 explosion2Position = new Vector3(transform.position.x - 0.1f, transform.position.y + 0.245f, transform.position.z - 1);
        GameObject explosion2 = Instantiate(deathExplosionVFX, explosion2Position, Quaternion.identity);
        explosion2.GetComponent<AudioSource>().volume = 0.5f;
        explosion2.transform.parent = transform;
        clip = explosion2.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        yield return new WaitForSeconds(0.15f);
        Vector3 explosion3Position = new Vector3(transform.position.x + 0.234f, transform.position.y - 0.045f, transform.position.z - 1);
        GameObject explosion3 = Instantiate(deathExplosionVFX, explosion3Position, Quaternion.identity);
        explosion3.GetComponent<AudioSource>().volume = 0.5f;
        explosion3.transform.parent = transform;
        clip = explosion3.GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        yield return new WaitForSeconds(0.2f);
        Vector3 explosion4Position = new Vector3(transform.position.x + 0.038f, transform.position.y + 0.067f, transform.position.z - 1);
        GameObject explosion4 = Instantiate(deathExplosionVFX, explosion4Position, Quaternion.identity);
        explosion4.GetComponent<AudioSource>().volume = 1;
        explosion4.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        explosion4.transform.parent = transform;
        clip = explosion4.GetComponent<AudioSource>().clip;
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
        transform.position = new Vector2(Mathf.Clamp(newXPos, minXPos, maxXPos), Mathf.Clamp(newYPos, minYPos, maxYPos));
    }
}
