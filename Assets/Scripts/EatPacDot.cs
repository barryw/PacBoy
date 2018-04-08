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

    void OnTriggerEnter2D(Collider2D co) {
        if (co.name == "PacMan") {
            gameController.AddPoints (GameController.PointSource.SMALLDOT);
            gameController.Chomp ();
            Destroy(gameObject);
        }
    }
}
