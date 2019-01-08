using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementFight : MonoBehaviour {
    [SerializeField] int bigLaserDelay = 10;
    [SerializeField] GameObject laserBaseLeft;
    [SerializeField] GameObject laserBaseMiddle;
    [SerializeField] GameObject laserBaseRight;
    [SerializeField] GameObject laserBeam;
    [SerializeField] float rotatingSpeed = 20;

    BossMechanics bossMechanics;
    Game game;
    bool doingTheSpin = false;
    bool doneSpinning = false;
    bool laserWarmingUp = false;
    int randomDirectionDecider;
    bool clockwiseRotation = false;
    bool canSpawnEnemies = false;
    // Start is called before the first frame update
    void Start() {
        game = FindObjectOfType<Game>();
        bossMechanics = FindObjectOfType<BossMechanics>();
        StartCoroutine(WaitForLaserDelay());
    }

    // Update is called once per frame
    void Update() {
        if (laserWarmingUp) {
            laserWarmingUp = false;
            StartCoroutine(WarmupAndShootBigLaser());
        }
        if (doingTheSpin) {
            RotateJudgement();
            if ((transform.rotation.z < 0 && !clockwiseRotation) || (transform.rotation.z > 0 && clockwiseRotation)) {
                doingTheSpin = false;
                laserBaseLeft.transform.localScale = new Vector3(0, 0, 0);
                laserBaseRight.transform.localScale = new Vector3(0, 0, 0);
                laserBaseMiddle.transform.localScale = new Vector3(0, 0, 0);
                laserBeam.transform.localScale = new Vector3(1, 1, 0);
                laserBeam.transform.localPosition = new Vector3(0, -0.1f, 0);
                transform.rotation = new Quaternion(0, 0, 0, 0);
                EndBigLaser();
            }
        }
        if (canSpawnEnemies) {
            canSpawnEnemies = false;
            StartCoroutine(SpawnMinions());
        }
    }

    private IEnumerator WaitForLaserDelay() {
        yield return new WaitForSeconds(bigLaserDelay);
        bossMechanics.SetIsFiringBigLaserOn();
    }

    private IEnumerator WarmupAndShootBigLaser() {
        yield return new WaitForSeconds(1);
        laserBaseLeft.transform.localScale = new Vector3(0.8f, 2.7f, 0);
        laserBaseRight.transform.localScale = new Vector3(0.8f, 2.7f, 0);
        laserBaseMiddle.transform.localScale = new Vector3(1, 3, 0);
        yield return new WaitForSeconds(1.2f);
        laserBaseLeft.transform.localScale = new Vector3(1.6f, 2.7f, 0);
        laserBaseRight.transform.localScale = new Vector3(1.6f, 2.7f, 0);
        laserBaseMiddle.transform.localScale = new Vector3(2, 3, 0);
        laserBaseMiddle.transform.localPosition = new Vector3(0, -1.3f, 0);
        yield return new WaitForSeconds(1.2f);
        laserBaseLeft.transform.localScale = new Vector3(2.4f, 2.7f, 0);
        laserBaseRight.transform.localScale = new Vector3(2.4f, 2.7f, 0);
        laserBaseMiddle.transform.localScale = new Vector3(3, 3, 0);
        laserBaseMiddle.transform.localPosition = new Vector3(0, -1.4f, 0);
        yield return new WaitForSeconds(1.2f);
        laserBeam.transform.localPosition = new Vector3(0, -5, 0);
        laserBeam.transform.localScale = new Vector3(1, 50, 0);
        yield return new WaitForSeconds(0.5f);
        doingTheSpin = true;
    }

    private void RotateJudgement() {
        if (game.GetGameState() == Game.GameState.Playing) {
            if (clockwiseRotation) {
                transform.Rotate(0, 0, rotatingSpeed * Time.deltaTime * -1);
            }
            else {
                transform.Rotate(0, 0, rotatingSpeed * Time.deltaTime);
            }
        }
    }

    public void StartBigLaser() {
        randomDirectionDecider = UnityEngine.Random.Range(-10, 10);
        while (randomDirectionDecider == 0) {
            randomDirectionDecider = UnityEngine.Random.Range(-10, 10);
        }
        if (randomDirectionDecider > 0) {
            clockwiseRotation = false;
        }
        else {
            clockwiseRotation = true;
        }
        laserWarmingUp = true;
        StartCoroutine(WarmupAndShootBigLaser());
    }

    public void EndBigLaser() {
        canSpawnEnemies = true;
        bossMechanics.SetIsFiringBigLaserOff();
        StartCoroutine(WaitForLaserDelay());
    }

    private IEnumerator SpawnMinions() {
        yield return new WaitForSeconds(5);
    }
}
