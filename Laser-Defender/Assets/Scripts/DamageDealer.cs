using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour {
    [SerializeField] int damage = 5;
    [SerializeField] AudioClip shootSound;
    [SerializeField] float shootVolume = 0.25f;

    // Start is called before the first frame update
    void Start() {
        if (shootSound) {
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootVolume);
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public int GetDamage() {
        return damage;
    }

}
