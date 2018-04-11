using System;
using UnityEngine;

public class BaseActor : MonoBehaviour
{
    protected GameController _gameController;
    protected Animator _anim;

    protected void Start()
    {
        GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
        if (gc != null)
            _gameController = gc.GetComponent<GameController> ();
        _anim = GetComponent<Animator> ();
    }

    public Vector2 Tile
    {
        get {
            return new Vector2 (Mathf.Ceil (transform.position.x - 0.5f), Mathf.Ceil (transform.position.y - 0.5f));
        }
    }

    public GameController GameController
    {
        get {
            return _gameController;
        }
    }

    public Animator Animator
    {
        get {
            return _anim;
        }
    }

    public void ColorTile()
    {
        Debug.DrawLine (Tile + new Vector2 (-.5f, -.5f), Tile + new Vector2 (.5f, -.5f), Color.black, 0, false);
        Debug.DrawLine (Tile + new Vector2 (-.5f, -.5f), Tile + new Vector2 (-.5f, .5f), Color.black, 0, false);
        Debug.DrawLine (Tile + new Vector2 (.5f, .5f), Tile + new Vector2 (.5f, -.5f), Color.black, 0, false);
        Debug.DrawLine (Tile + new Vector2 (-.5f, .5f), Tile + new Vector2 (.5f, .5f), Color.black, 0, false);
    }
}

