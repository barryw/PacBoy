﻿using System;
using UnityEngine;

public class BaseActor : MonoBehaviour
{
    protected GameController _gameController;
    protected Animator _anim;
    protected float _animationSpeed = 0.8f;
    protected float _speed = 5.0f;
    protected Vector2 _destination = Vector2.zero;
    protected Vector2 _direction = Vector2.zero;
    protected Maze _maze = Maze.Instance();

    protected void Start()
    {
        GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
        if (gc != null)
            _gameController = gc.GetComponent<GameController> ();
        _anim = GetComponent<Animator> ();
    }

    protected Vector2 Tile
    {
        get {
            return new Vector2 (Mathf.Ceil (transform.position.x), Mathf.Ceil (transform.position.y));
        }
    }

    protected Vector2 TileCenter
    {
        get {
            return new Vector2 (Mathf.Ceil (transform.position.x) - 0.5f, Mathf.Ceil (transform.position.y) - 0.5f);
        }
    }

    /// <summary>
    /// Set the destination tile
    /// </summary>
    /// <param name="direction">The direction of travel as a Vector2</param>
    protected void SetDestination(Vector2 direction)
    {
        if (Valid (direction)) {
            Destination = TileCenter + direction;
            Animation = true;
        } else {
            Animation = false;
        }
    }

    /// <summary>
    /// Determine whether the direction from the current tile is a valid location
    /// </summary>
    /// <param name="direction">The direction of travel as a Vector2</param>
    protected bool Valid(Vector2 direction)
    {
        return _maze.ValidLocations ().Contains (Tile + direction);
    }

    /// <summary>
    /// Move the actor toward a position represented as a Vector2
    /// </summary>
    protected void Move()
    {
        Vector2 p = Vector2.MoveTowards(transform.position, Destination, Speed * Time.deltaTime);
        GetComponent<Rigidbody2D>().MovePosition(p);
    }

    /// <summary>
    /// Animate the actor
    /// </summary>
    protected void Animate()
    {
        Vector2 dir = Destination - TileCenter;
        Animator.SetFloat("DirX", dir.x);
        Animator.SetFloat("DirY", dir.y);
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

    /// <summary>
    /// Set the actor's speed
    /// </summary>
    /// <value>The speed.</value>
    public float Speed
    {
        get {
            return _speed;
        }
        set {
            _speed = value;
        }
    }

    /// <summary>
    /// Set the actor's destination tile
    /// </summary>
    /// <value>The destination.</value>
    protected Vector2 Destination
    {
        get {
            return _destination;
        }
        set {
            _destination = value;
        }
    }

    /// <summary>
    /// Set the direction that the actor is moving in
    /// </summary>
    /// <value>The direction.</value>
    protected Vector2 Direction
    {
        get {
            return _direction;
        }
        set {
            _direction = value;
        }
    }

    protected float AnimationSpeed
    {
        get {
            return _animationSpeed;
        }
        set {
            _animationSpeed = value;
        }
    }

    protected bool Animation {
        set {
            if (value) {
                Animator.speed = AnimationSpeed;
            } else {
                Animator.speed = 0.0f;
                Animator.Play ("", 0, 0.0f);
            }
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

