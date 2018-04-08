using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatPacDot : MonoBehaviour {

    GameController gameController;

    void Start()
    {
        GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
        if (gc != null)
            gameController = gc.GetComponent<GameController> ();
    }

    void Update()
    {
        if (gameObject != null && gameController.PacManTile == Tile) {
            gameController.AddPoints (GameController.PointSource.SMALLDOT);
            gameController.Chomp ();
            Destroy (gameObject);
        }
    }

    Vector2 Tile
    {
        get {
            return new Vector2 (Mathf.Round (transform.position.x + 0.25f), Mathf.Round (transform.position.y + 2.5f));
        }
    }
}
