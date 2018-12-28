using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {
    [SerializeField] GameObject healthIcon;

    PlayerShip player;
    float nextHealthTickXPos;
    float constantHealthTickYPos;
    int playerHealth;

    // Start is called before the first frame update
    void Start() {
        player = FindObjectOfType<PlayerShip>();
        nextHealthTickXPos = -2.5f;
        constantHealthTickYPos = 4.5f;
    }

    public void FillHealthBar() {
        playerHealth = player.GetHealth();
        for (int childIndex = 0; childIndex < playerHealth; childIndex++) {
            GameObject tick = Instantiate(healthIcon, new Vector3(nextHealthTickXPos, constantHealthTickYPos, 0), Quaternion.identity);
            tick.transform.parent = transform;
            nextHealthTickXPos += 0.5f;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void HealthLost() {
        nextHealthTickXPos -= 0.5f;
        int childIndex = transform.childCount - 1;
        Destroy(transform.GetChild(childIndex).gameObject);
    }

    public void HealthGained() {
        GameObject tick = Instantiate(healthIcon, new Vector3(nextHealthTickXPos, constantHealthTickYPos, 0), Quaternion.identity);
        tick.transform.parent = transform;
        nextHealthTickXPos += 0.5f;
    }
}
