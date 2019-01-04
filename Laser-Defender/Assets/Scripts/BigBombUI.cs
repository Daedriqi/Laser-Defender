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
        currentXPos = startingXPos;
    }

    public void Update() {

    }

    public void SetStartingXPos() {
        currentXPos = startingXPos;
    }

    public void FillAmmoBar(int numberToFill) {
        for (int index = 1; index <= numberToFill; index++) {
            currentXPos += 0.5f;
            GameObject ammo = Instantiate(bombSprite);
            ammo.transform.parent = transform;
            ammo.transform.position = new Vector3(currentXPos, constantYPos, -1);
        }
    }

    public void RemoveAmmo() {
        if (transform.childCount > 0) {
            currentXPos -= 0.5f;
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        }
    }
}
