using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatPowerPellet : MonoBehaviour {
    public GameObject Blinky;
    public GameObject Pinky;
    public GameObject Inky;
    public GameObject Clyde;
    public GameObject PacMan;

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
            gameController.AddPoints (GameController.PointSource.POWER_PELLET);
            gameController.Chomp ();
            gameController.FrightenGhosts ();
            Destroy (gameObject);
        }
    }
}
