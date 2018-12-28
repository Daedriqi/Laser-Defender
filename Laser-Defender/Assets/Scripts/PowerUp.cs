using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    [SerializeField] PowerUpType type;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void GetPowerUpEffect(GameObject playerObject) {
        PlayerShip player = playerObject.GetComponent<PlayerShip>();
        if (type == PowerUpType.ExtraLife) {
            player.AddToHealth();
        }
        if (type == PowerUpType.ProjectileQuantityUp) {
            player.IncreaseBulletQuantity();
        }
        if (type == PowerUpType.ProjectileSpeedUp) {
            player.DecreaseShootDelay();
        }
    }

    public enum PowerUpType {
        ExtraLife,
        ProjectileSpeedUp,
        ProjectileQuantityUp,
        ProjectileSizeUp,
        ShieldBoost
    }
}
