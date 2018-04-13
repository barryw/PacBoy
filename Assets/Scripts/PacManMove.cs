using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManMove : BaseActor {

    public GameObject Blinky;
    public GameObject Pinky;
    public GameObject Inky;
    public GameObject Clyde;

    private GhostMove BlinkyMover;
    private GhostMove PinkyMover;
    private GhostMove InkyMover;
    private GhostMove ClydeMover;

    Vector2 _nextDir = Vector2.right;

    bool dying = false;

    new void Start () {
        base.Start ();
        Direction = Vector2.right;
        SetDestination (Direction);
        Animation = true;
        Speed = 7.0f;

        BlinkyMover = Blinky.GetComponent<GhostMove> ();
        PinkyMover = Pinky.GetComponent<GhostMove> ();
        InkyMover = Inky.GetComponent<GhostMove> ();
        ClydeMover = Clyde.GetComponent<GhostMove> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (dying)
            return;

        if (GameController.IsReady) {
            CheckForGhostCollision ();
            Move ();
            ReadInput ();
            ReadInputAndMove();
            Animate();
        }
	}

    void CheckForGhostCollision()
    {
        if (Tile == BlinkyMover.Tile || Tile == InkyMover.Tile || Tile == PinkyMover.Tile || Tile == ClydeMover.Tile) {
            if (BlinkyMover.CurrentMode == GhostMove.Mode.CHASE || BlinkyMover.CurrentMode == GhostMove.Mode.SCATTER) {
                // PacMan is dead :-(
                //StartCoroutine (ShowDeathAnimation ());
            } else {
                // EAT THE GHOST!
                Debug.Log("EATED THE GHOST!");
            }
        }
    }

    IEnumerator ShowDeathAnimation()
    {
        dying = true;
        GameController.StopSiren ();
        GameController.IsReady = false;
        Animation = false;
        yield return new WaitForSeconds (0.75f);
        Animation = true;
        Animator.SetBool ("Died", true);
        yield return new WaitForSeconds (2.0f);
        Destroy (gameObject);
        GameController.Reset ();
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
            SetDestination (Direction);
        } else {
            if (Valid (_nextDir)) {
                Direction = _nextDir;
                SetDestination (_nextDir);
            } else {
                SetDestination (Direction);
            }
        }
    }
}
