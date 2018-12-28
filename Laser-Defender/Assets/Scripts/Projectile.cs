using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        AudioClip clip = GetComponent<AudioSource>().clip;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    // Update is called once per frame
    void Update() {

    }
}
