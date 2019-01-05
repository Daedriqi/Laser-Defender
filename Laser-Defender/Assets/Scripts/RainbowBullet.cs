using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowBullet : MonoBehaviour {
    [SerializeField] List<Color> colors;
    SpriteRenderer spriteRenderer;

    bool canChange = true;
    int colorIndex = 0;
    // Start is called before the first frame update
    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (canChange) {
            canChange = false;
            StartCoroutine(ChangeColors());
        }
    }

    private IEnumerator ChangeColors() {
        spriteRenderer.color = colors[colorIndex];
        yield return new WaitForSeconds(0.05f);
        colorIndex++;
        if (colorIndex >= colors.Count) {
            colorIndex = 0;
        }
        canChange = true;
    }
}
