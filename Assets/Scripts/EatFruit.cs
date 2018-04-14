using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatFruit : MonoBehaviour {

    GameController gameController;
    Vector2 Tile;
    bool fruitEaten = false;

    void Start()
    {
        GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
        if (gc != null)
            gameController = gc.GetComponent<GameController> ();
        Tile = new Vector2 (Mathf.Ceil (transform.position.x), Mathf.Ceil (transform.position.y));
    }

    void FixedUpdate()
    {
        if (Eaten) {
            fruitEaten = true;
            PlaySound ();
            AddPoints ();
            ShowPoints ();
        }
    }

    /// <summary>
    /// Display the point value in place of the fruit
    /// </summary>
    void ShowPoints()
    {
        gameObject.transform.position = new Vector2 (-10, -10);
        Destroy (gameObject, 1.0f);
        GameObject fruitPoints = gameController.GetFruitPoints ();
        Destroy (fruitPoints, 2.0f);
    }

    /// <summary>
    /// Call back to the game controller to add the points to the player's score
    /// </summary>
    void AddPoints()
    {
        GameController.PointSource points = gameController.GetBonusPoints ();
        gameController.AddPoints (points);
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="EatFruit"/> is eaten.
    /// </summary>
    /// <value><c>true</c> if eaten; otherwise, <c>false</c>.</value>
    bool Eaten
    {
        get {
            return gameController.PacManMover.Tile == Tile && !fruitEaten;
        }
    }

    /// <summary>
    /// Play the fruit eaten sound
    /// </summary>
    void PlaySound()
    {
        gameObject.GetComponent<AudioSource> ().Play ();
    }
}
