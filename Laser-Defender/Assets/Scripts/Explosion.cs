using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {


    // Start is called before the first frame update
    void Start() {
        DestroyOnTimer();
    }

    // Update is called once per frame
    void Update() {

    }

    public void DestroyOnTimer() {
        StartCoroutine(GoBoom());
    }

    private IEnumerator GoBoom() {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
