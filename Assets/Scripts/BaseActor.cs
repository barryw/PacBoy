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
            return new Vector2 (Mathf.Round (transform.position.x + 0.25f), Mathf.Round (transform.position.y + 2.5f));
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
}

