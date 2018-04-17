using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatPacDot : MonoBehaviour {

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
        if (gameController.PacManMover.Tile == Tile) {
            gameController.LastDotEatenTime = Time.fixedTime;
            gameController.PacManMover.EatingSmallDots = true;
            gameController.AddPoints (GameController.PointSource.SMALLDOT);
            gameController.UpdateGhostDotCounts ();
            Destroy (gameObject);
        } else {
            gameController.PacManMover.EatingSmallDots = false;
        }
    }
}
