using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBombUI : MonoBehaviour {
    [SerializeField] GameObject bombSprite;
    [SerializeField] float startingXPos = -2.5f;
    [SerializeField] float constantYPos = 4.25f;

    float currentXPos;

    private void Start() {
        currentXPos = startingXPos;
    }

    public void SetStartingXPos() {
        currentXPos = startingXPos;
    }

    public void FillAmmoBar(int numberToFill) {
        for (int index = 1; index <= numberToFill; index++) {
            GameObject ammo = Instantiate(bombSprite);
            ammo.transform.parent = transform;
            ammo.transform.position = new Vector3(currentXPos, constantYPos, -1);
            currentXPos += 0.5f;
        }
    }

    public void RemoveAmmo() {
        if (transform.childCount > 0) {
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
            currentXPos -= 0.5f;
        }
    }
}
