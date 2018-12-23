using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
    //configuration parameters
    [SerializeField] float shipSpeed = 5f;
    [SerializeField] float projectileSpeed = 15f;
    [SerializeField] float shootDelay = 0.05f;
    [SerializeField] Sprite leftTurn;
    [SerializeField] Sprite rightTurn;
    [SerializeField] GameObject plasmaBall;

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
        game = FindObjectOfType<Game>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        defaultSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        GetMoveBoundaries();
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

    private void Shoot() {
        if (Input.GetButton("Fire1") && true && Time.time > timeBuffer) {
            timeBuffer = Time.time + shootDelay;
            GameObject projectile1 = Instantiate(plasmaBall, transform.position, Quaternion.identity);
            projectile1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            //shooting in 3 directions at once in a spray
            //GameObject projectile2 = Instantiate(plasmaBall, transform.position, Quaternion.identity);
            //projectile2.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, projectileSpeed);
            //GameObject projectile3 = Instantiate(plasmaBall, transform.position, Quaternion.identity);
            //projectile3.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, projectileSpeed);
        }
        #region Coroutines
        //Coroutine tutorial not bad for certain things but it's terrible for this game because it allows weird abilities to decrease projectileDelay significantly
        //if (Input.GetButtonDown("Fire1")) {
        //    StartCoroutine(ShootOnDelay());
        //}
        //if (Input.GetButtonUp("Fire1")) {
        //    StopCoroutine(ShootOnDelay());
        //}

        //private IEnumerator ShootOnDelay() {
        //    while (Input.GetButton("Fire1")) {
        //        GameObject projectile = Instantiate(plasmaBall, transform.position, Quaternion.identity);
        //        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        //        yield return new WaitForSeconds(shootDelay);
        //    }
        //}
        #endregion
    }

    private void MoveShip() {
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * shipSpeed;
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * shipSpeed;
        if (deltaX > 0) {
            spriteRenderer.sprite = rightTurn;
        }
        else if (deltaX < 0) {
            spriteRenderer.sprite = leftTurn;
        }
        else {
            spriteRenderer.sprite = defaultSprite;
        }
        float newXPos = transform.position.x + deltaX;
        float newYPos = transform.position.y + deltaY;
        transform.position = new Vector2(Mathf.Clamp(newXPos, minX, maxX), Mathf.Clamp(newYPos, minY, maxY));
    }
}
