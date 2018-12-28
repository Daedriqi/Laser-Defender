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

    public void GetPowerUpEffect(GameObject player) {
        if (type == PowerUpType.ExtraLife) {
            player.GetComponent<PlayerShip>().AddToHealth();
        }
    }

    public enum PowerUpType {
        ExtraLife,
        ProjectileSpeedUp,
        ProjectileQuantityUp,
        PlasmaProjectile,
        LaserProjectile
    }
}
