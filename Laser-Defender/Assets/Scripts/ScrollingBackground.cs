using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {
    [SerializeField] float scrollingSpeed = 0.005f;

    Game game;
    Material material;
    Vector2 offset;

    // Start is called before the first frame update
    void Start() {
        material = gameObject.GetComponent<Renderer>().material;
        game = FindObjectOfType<Game>();
        offset = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update() {
        if (game.GetGameState() == Game.GameState.Playing) {
            offset = new Vector2(0, offset.y + scrollingSpeed);
            material.mainTextureOffset = offset;
        }
    }
}
