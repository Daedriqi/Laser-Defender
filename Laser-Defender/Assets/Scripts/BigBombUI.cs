using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBombUI : MonoBehaviour {
    [SerializeField] GameObject bombSprite;
    [SerializeField] float startingXPos = -3.25f;
    [SerializeField] float constantYPos = 4.25f;

    Game game;

    float currentXPos;

    void Start() {
        game = FindObjectOfType<Game>();
    }

    public void Update() {

    }

    public void FillAmmoBar(int numberToFill) {
        if (transform.childCount == 0) {
            currentXPos = startingXPos;
            GameObject ammo = Instantiate(bombSprite, new Vector3(currentXPos, constantYPos, -1), Quaternion.identity);
            ammo.transform.parent = transform;
            numberToFill--;
        }
        for (int index = 1; index <= numberToFill; index++) {
            currentXPos = transform.GetChild(transform.childCount - 1).position.x + 0.5f;
            GameObject ammo = Instantiate(bombSprite, new Vector3(currentXPos, constantYPos, -1), Quaternion.identity);
            ammo.transform.parent = transform;
        }
    }

    public void RemoveAmmo() {
        if (transform.childCount > 0) {
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        }
    }
}
