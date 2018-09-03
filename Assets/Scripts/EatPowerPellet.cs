using UnityEngine;

public class EatPowerPellet : MonoBehaviour 
{
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
        if (gameObject == null || _gameController.PacManMover.Tile != _tile) return;
        _gameController.EatLargeDot (_tile);
        Destroy (gameObject);
    }
}
