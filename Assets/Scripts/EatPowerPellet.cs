using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatPowerPellet : MonoBehaviour 
{
    GameController gameController;
    Vector2 Tile;

    void Start()
    {
        GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
        if (gc != null)
            gameController = gc.GetComponent<GameController> ();
        Tile = new Vector2 (Mathf.Ceil (transform.position.x), Mathf.Ceil (transform.position.y));
    }
	
    void FixedUpdate()
    {
        if (gameObject != null && gameController.PacManMover.Tile == Tile) {
            gameController.EatLargeDot (Tile);
            Destroy (gameObject);
        }
    }
}
