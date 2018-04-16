using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatFruit : MonoBehaviour {

    GameController gameController;
    AudioController _audio;

    Vector2 TileCenter;
    bool fruitEaten = false;

    void Start()
    {
        GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
        if (gc != null)
            gameController = gc.GetComponent<GameController> ();
        _audio = AudioController.Instance;
        TileCenter = new Vector2 (Mathf.Ceil (transform.position.x) - 0.5f, Mathf.Ceil (transform.position.y) - 0.5f);
    }

    void FixedUpdate()
    {
        // Don't handle collision detection for the fruit at the bottom of the screen since PacMan will never eat it
        if (TileCenter != new Vector2 (13.5f, 15.5f))
            return;
        
        if (Eaten) {
            fruitEaten = true;
            _audio.PlayEatFruit ();
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
        int points = gameController.GetBonusPoints ();
        gameController.AddPoints (points);
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="EatFruit"/> is eaten.
    /// </summary>
    /// <value><c>true</c> if eaten; otherwise, <c>false</c>.</value>
    bool Eaten
    {
        get {
            return gameController.PacManMover.TileCenter == TileCenter && !fruitEaten;
        }
    }
}
