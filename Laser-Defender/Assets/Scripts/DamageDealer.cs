using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour {
    [SerializeField] int damage = 5;
    [SerializeField] AudioClip shootSound;
    [Range(0, 1)][SerializeField] float shootVolume = 0.25f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public AudioClip GetSound()
    {
        return shootSound;
    }

    public float GetVolume()
    {
        return shootVolume;
    }

    public int GetDamage() {
        return damage;
    }

}
