using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManMove : BaseActor {

    public float speed = 1.0f;
    Vector2 _dest = Vector2.zero;
    Vector2 _dir = Vector2.right;
    Vector2 _nextDir = Vector2.right;
    Maze _maze = Maze.Instance();

    bool dying = false;

    void Start () {
        base.Start ();
        _dest = TileCenter + _dir;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (dying)
            return;

        if (GameController.IsReady) {
            CheckForGhostCollision ();
            MovePacMan ();
            ReadInput ();
            ReadInputAndMove();
            Animate();
        }
	}

    void CheckForGhostCollision()
    {
        if (Tile == GameController.BlinkyTile) {
            StartCoroutine (ShowDeathAnimation ());
        }
    }

    IEnumerator ShowDeathAnimation()
    {
        dying = true;
        GameController.StopSiren ();
        GameController.StopGhosts ();
        Animation = false;
        yield return new WaitForSeconds (0.75f);
        Animation = true;
        Animator.SetBool ("Died", true);
        yield return new WaitForSeconds (2.0f);
        Destroy (gameObject);
        GameController.Reset ();
    }

    void Animate()
    {
        Vector2 dir = _dest - TileCenter;
        Animator.SetFloat("DirX", dir.x);
        Animator.SetFloat("DirY", dir.y);
    }

    bool Animation {
        set {
            if (value) {
                Animator.speed = 0.8f;
            } else {
                Animator.speed = 0.0f;
                Animator.Play ("", 0, 0.0f);
            }
        }
    }

    bool Valid(Vector2 direction)
    {
        return _maze.ValidLocations ().Contains (Tile + direction);
    }

    void MovePacMan()
    {
        Vector2 p = Vector2.MoveTowards(transform.position, _dest, 10f * Time.deltaTime);
        GetComponent<Rigidbody2D>().MovePosition(p);
    }

    void ReadInput()
    {
        if (Input.GetAxis("Horizontal") > 0) 
            _nextDir = Vector2.right;
        if (Input.GetAxis ("Horizontal") < 0)
            _nextDir = Vector2.left;
        if (Input.GetAxis("Vertical") > 0) 
            _nextDir = Vector2.up;
        if (Input.GetAxis ("Vertical") < 0)
            _nextDir = Vector2.down;

        if (!Input.anyKey)
            _nextDir = Vector2.zero;
    }

    void ReadInputAndMove()
    {
        // Continue in _dir if no button was pressed
        if (_nextDir == Vector2.zero) {
            // Is the current direction valid?
            if (Valid (_dir)) {
                SetDestination (_dir);
                Animation = true;
            } else {
                Animation = false;
            }
        } else {
            if (Valid (_nextDir)) {
                _dir = _nextDir;
                SetDestination (_nextDir);
                Animation = true;
            } else {
                if (Valid (_dir)) {
                    // Keep on truckin' good buddy!
                    SetDestination(_dir);
                    Animation = true;
                } else {
                    Animation = false;
                }
            }
        }
    }

    void SetDestination(Vector2 direction)
    {
        _dest = TileCenter + direction;
    }
}
