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
            return new Vector2 (Mathf.Ceil (transform.position.x), Mathf.Ceil (transform.position.y));
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

