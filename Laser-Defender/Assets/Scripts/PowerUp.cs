using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    [SerializeField] PowerUpType type;

    HealthBarUI healthBarUI;

    // Start is called before the first frame update
    void Start() {
        healthBarUI = FindObjectOfType<HealthBarUI>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void GetPowerUpEffect(GameObject playerObject) {
        PlayerShip player = playerObject.GetComponent<PlayerShip>();
        if (type == PowerUpType.HealthUp) {
            player.UpdatePlayerHealth(25);
            healthBarUI.UpdateHealthBar(25);
        }
        if (type == PowerUpType.ProjectileQuantityUp) {
            player.IncreaseBulletQuantity();
        }
        if (type == PowerUpType.ProjectileSpeedUp) {
            player.DecreaseShootDelay();
        }
        if (type == PowerUpType.ProjectileSizeUp) {
            player.IncreaseBulletSize();
        }
        if (type == PowerUpType.ShieldBoost) {
            player.UpdatePlayerShield(40);
            healthBarUI.UpdateShieldBar(40);
        }
        if (type == PowerUpType.BigBlastAmmo) {
            player.UpdateBigBlastAmmo();
        }
    }

    public enum PowerUpType {
        HealthUp,
        ProjectileSpeedUp,
        ProjectileQuantityUp,
        ProjectileSizeUp,
        ShieldBoost,
        BigBlastAmmo
    }
}
