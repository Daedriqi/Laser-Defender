using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialtyEnemy : MonoBehaviour {
    [SerializeField] SpecialtyWaveType type;
    [SerializeField] GameObject specialtyPath;
    [SerializeField] float enemySpeed = 2f;
    [SerializeField] GameObject shieldForEnemies;
    [SerializeField] float bombDelayMin = 0;
    [SerializeField] float bombDelayMax = 0;

    List<Transform> waypoints;
    Game game;
    GameObject projectile;
    DamageDealer damageDealer;
    EnemyShip enemyShip;

    int waypointIndex = 0;
    bool enemyAlive = true;
    bool bombTime = true;
    float randomVal;
    float projectileSpeed;

    // Start is called before the first frame update
    void Start() {
        game = FindObjectOfType<Game>();
        enemyShip = GetComponent<EnemyShip>();
        GetWaypoints();
        projectile = GetComponent<EnemyShip>().GetProjectile();
        transform.position = waypoints[0].position;
    }

    // Update is called once per frame
    void Update() {
        if (game.GetGameState() == Game.GameState.Playing) {
            FollowPath();
            if (type == SpecialtyWaveType.BigBomb || type == SpecialtyWaveType.Bomb) {
                BombProcess();
            }
            if (type == SpecialtyWaveType.Shielder) {
                ShielderProcess();
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

    private void BombProcess() {
        if (bombTime) {
            bombTime = false;
            StartCoroutine(ExplodeOnDelay());
        }
    }

    private IEnumerator ExplodeOnDelay() {
        randomVal = UnityEngine.Random.Range(bombDelayMin, bombDelayMax);
        yield return new WaitForSeconds(randomVal);
        if (type == SpecialtyWaveType.Bomb) {
            SmallExplosion();
        }
        else {
            BigExplosion();
        }
    }

    private void SmallExplosion() {
        projectileSpeed = enemyShip.GetProjectileSpeed();
        GameObject projectile1 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile1.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        GameObject projectile2 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile2.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile2.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, 0);
        GameObject projectile3 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile3.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile3.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        GameObject projectile4 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile4.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile4.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, 0);
        GameObject projectile5 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile5.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile5.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed / 1.5f, -projectileSpeed / 1.5f);
        GameObject projectile6 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile6.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile6.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed / 1.5f, -projectileSpeed / 1.5f);
        GameObject projectile7 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile7.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile7.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed / 1.5f, projectileSpeed / 1.5f);
        GameObject projectile8 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile8.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile8.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed / 1.5f, projectileSpeed / 1.5f);
        AudioSource.PlayClipAtPoint(damageDealer.GetSound(), Camera.main.transform.position, damageDealer.GetVolume());
    }

    private void BigExplosion() {
        projectileSpeed = enemyShip.GetProjectileSpeed();
        GameObject projectile1 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile1.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        GameObject projectile2 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile2.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile2.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, 0);
        GameObject projectile3 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile3.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile3.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        GameObject projectile4 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile4.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile4.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, 0);
        GameObject projectile5 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile5.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile5.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed / 1.25f, -projectileSpeed / 1.25f);
        GameObject projectile6 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile6.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile6.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed / 1.25f, -projectileSpeed / 1.25f);
        GameObject projectile7 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile7.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile7.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed / 1.25f, projectileSpeed / 1.25f);
        GameObject projectile8 = Instantiate(projectile, transform.position, Quaternion.identity);
        damageDealer = projectile8.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile8.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed / 1.25f, projectileSpeed / 1.25f);
        
        GameObject projectile9 = Instantiate(projectile, transform.position, Quaternion.identity);
        projectile9.name = "9";
        damageDealer = projectile9.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile9.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed / 2, -projectileSpeed);
        GameObject projectile10 = Instantiate(projectile, transform.position, Quaternion.identity);
        projectile10.name = "10";
        damageDealer = projectile10.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile10.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, -projectileSpeed / 2);
        GameObject projectile11 = Instantiate(projectile, transform.position, Quaternion.identity);
        projectile11.name = "11";
        damageDealer = projectile11.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile11.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed / 2, -projectileSpeed);
        GameObject projectile12 = Instantiate(projectile, transform.position, Quaternion.identity);
        projectile12.name = "12";
        damageDealer = projectile12.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile12.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, -projectileSpeed / 2);
        GameObject projectile13 = Instantiate(projectile, transform.position, Quaternion.identity);
        projectile13.name = "13";
        damageDealer = projectile13.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile13.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, projectileSpeed / 2);
        GameObject projectile14 = Instantiate(projectile, transform.position, Quaternion.identity);
        projectile14.name = "14";
        damageDealer = projectile14.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile14.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed / 2, projectileSpeed);
        GameObject projectile15 = Instantiate(projectile, transform.position, Quaternion.identity);
        projectile15.name = "15";
        damageDealer = projectile15.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile15.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, projectileSpeed / 2);
        GameObject projectile16 = Instantiate(projectile, transform.position, Quaternion.identity);
        projectile16.name = "16";
        damageDealer = projectile16.GetComponent<DamageDealer>();
        damageDealer.SetDamage(0);
        projectile16.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed / 2, projectileSpeed);
        AudioSource.PlayClipAtPoint(damageDealer.GetSound(), Camera.main.transform.position, damageDealer.GetVolume());
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


    public SpecialtyWaveType GetSpecialtyType() {
        return type;
    }

    public enum SpecialtyWaveType {
        Bomb,
        BigBomb,
        Shielder,
    }
}
