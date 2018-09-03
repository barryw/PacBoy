using UnityEngine;

public class EatPacDot : MonoBehaviour {
    private GameController _gameController;
    private Vector2 _tile;

    private void Start()
    {
        var gc = GameObject.FindGameObjectWithTag ("GameController");
        if (gc != null)
            _gameController = gc.GetComponent<GameController> ();
        _tile = new Vector2 (Mathf.Ceil (transform.position.x), Mathf.Ceil (transform.position.y));
    }

    private void FixedUpdate()
    {
        if (_gameController.PacManMover.Tile == _tile) {
            _gameController.EatSmallDot (_tile);
            Destroy (gameObject);
        } else {
            _gameController.PacManMover.EatingSmallDots = false;
        }
    }
}
