using UnityEngine;

public class EatFruit : MonoBehaviour {
    private GameController _gameController;
    private AudioController _audio;

    private Vector2 _tileCenter;
    private bool _fruitEaten;

    private void Start()
    {
        var gc = GameObject.FindGameObjectWithTag ("GameController");
        if (gc != null)
            _gameController = gc.GetComponent<GameController> ();
        _audio = AudioController.Instance;
        _tileCenter = new Vector2 (Mathf.Ceil (transform.position.x) - 0.5f, Mathf.Ceil (transform.position.y) - 0.5f);
    }

    private void FixedUpdate()
    {
        // Don't handle collision detection for the fruit at the bottom of the screen since PacMan will never eat it
        if (_tileCenter != new Vector2 (13.5f, 15.5f))
            return;

        if (!Eaten) return;
        _fruitEaten = true;
        _audio.PlayEatFruit ();
        AddPoints ();
        ShowPoints ();
    }

    /// <summary>
    /// Display the point value in place of the fruit
    /// </summary>
    private void ShowPoints()
    {
        gameObject.transform.position = new Vector2 (-10, -10);
        Destroy (gameObject, 1.0f);
        var fruitPoints = _gameController.GetFruitPoints ();
        Destroy (fruitPoints, 2.0f);
    }

    /// <summary>
    /// Call back to the game controller to add the points to the player's score
    /// </summary>
    private void AddPoints()
    {
        var points = _gameController.GetBonusPoints ();
        _gameController.AddPoints (points);
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="EatFruit"/> is eaten.
    /// </summary>
    /// <value><c>true</c> if eaten; otherwise, <c>false</c>.</value>
    private bool Eaten
    {
        get {
            return _gameController.PacManMover.TileCenter == _tileCenter && !_fruitEaten;
        }
    }
}
