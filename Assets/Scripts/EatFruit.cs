using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatFruit : MonoBehaviour {

    GameController gameController;
    Vector2 Tile;
    bool eatFruit = false;

    void Start()
    {
        GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
        if (gc != null)
            gameController = gc.GetComponent<GameController> ();
        Tile = new Vector2 (Mathf.Ceil (transform.position.x), Mathf.Ceil (transform.position.y));
    }

    void FixedUpdate()
    {
        if (gameController.PacManTile == Tile && !eatFruit) {
            eatFruit = true;
            GameController.PointSource points = gameController.GetBonusPoints ();
            gameController.EatFruit ();
            gameController.AddPoints (points);
            Destroy (gameObject);
            GameObject fruitPoints = gameController.GetFruitPoints ();
            Destroy (fruitPoints, 2.0f);
        }
    }
}
