using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    [SerializeField] PowerUpType type;
    [SerializeField] AudioClip powerUpSound;
    [Range(0, 1)][SerializeField] float clipVolume = 0.25f;


    HealthBarUI healthBarUI;

    // Start is called before the first frame update
    void Start() {
        healthBarUI = FindObjectOfType<HealthBarUI>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void GetPowerUpEffect(GameObject playerObject) {
        AudioSource.PlayClipAtPoint(powerUpSound, Camera.main.transform.position, clipVolume);
        PlayerShip player = playerObject.GetComponent<PlayerShip>();
        if (type == PowerUpType.HealthUp) {
            player.UpdatePlayerHealth(50);
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
            player.UpdatePlayerShield(75);
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
