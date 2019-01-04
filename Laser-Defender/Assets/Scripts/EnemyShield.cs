using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour {
    int shieldHits = 0;

    // Start is called before the first frame update
    void Start() {
        shieldHits = FindObjectOfType<EnemySpawner>().GetLevelIndex();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "PlayerBullet") {
            Destroy(collision.gameObject);
            shieldHits--;
        }
        if (collision.gameObject.tag == "Destructor") {
            shieldHits--;
        }
        if (shieldHits <= 0) {
            Destroy(gameObject);
        }
    }
}
