using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour {
    [SerializeField] RectTransform healthBar;
    [SerializeField] RectTransform shieldBar;

    float maxWidth = 150f;
    float maxXPos = 15f;
    float currentHealthWidth;
    float currentHealthXPos;
    float currentShieldWidth;
    float currentShieldXPos;

    private void Start() {
        currentHealthWidth = maxWidth;
        currentHealthXPos = maxXPos;
        currentShieldWidth = maxWidth;
        currentShieldXPos = maxXPos;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<RectTransform>();
        shieldBar = GameObject.FindGameObjectWithTag("ShieldBar").GetComponent<RectTransform>();
    }

    public void UpdateHealthBar(float amountToChange) {
        currentHealthWidth += amountToChange;
        currentHealthXPos += amountToChange / 2;
        if (currentHealthWidth < 0) {
            currentHealthWidth = 0;
            currentHealthXPos = 0;
        }
        if (currentHealthWidth > maxWidth) {
            currentHealthWidth = maxWidth;
            currentHealthXPos = maxXPos;
        }
        healthBar.localPosition = new Vector3(currentHealthXPos, healthBar.localPosition.y, healthBar.localPosition.z);
        healthBar.sizeDelta = new Vector2(currentHealthWidth, healthBar.rect.height);
    }

    public void UpdateShieldBar(float amountToChange) {
        currentShieldWidth += amountToChange;
        currentShieldXPos += amountToChange / 2;
        if (currentShieldWidth < 0) {
            currentShieldWidth = 0;
            currentShieldXPos = 0;
        }
        if (currentShieldWidth > maxWidth) {
            currentShieldWidth = maxWidth;
            currentShieldXPos = maxXPos;
        }
        shieldBar.localPosition = new Vector3(currentShieldXPos, shieldBar.localPosition.y, shieldBar.localPosition.z);
        shieldBar.sizeDelta = new Vector2(currentShieldWidth, shieldBar.rect.height);
    }
}
