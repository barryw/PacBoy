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

    private TableOfValues _tov = TableOfValues.Instance ();

    Vector2 _nextDir = Vector2.right;

    bool dying = false;
    bool eatingSmallDots = false;
    bool eatingBigDots = false;
    bool frightened = false;

    new void Start () {
        base.Start ();
        Direction = Vector2.left;
        Animation = true;
        AnimationSpeed = 1.5f;

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
            SetPacManSpeed ();
            CheckForGhostCollision ();
            Move ();
            ReadInput ();
            ReadInputAndMove();
            Animate();
        }
	}

    /// <summary>
    /// PacMan doesn't really get frightened, but this lets us know that the ghosts are
    /// </summary>
    public bool Frightened
    {
        get {
            return frightened;
        }
        set {
            frightened = false;
        }
    }

    public bool EatingSmallDots
    {
        get {
            return eatingSmallDots;
        }
        set {
            eatingSmallDots = value;
        }
    }

    public bool EatingBigDots
    {
        get {
            return eatingBigDots;
        }
        set {
            eatingBigDots = value;
        }
    }

    private void SetPacManSpeed()
    {
        if (EatingSmallDots) {
            if (Frightened)
                Speed = _tov.PacManFrightenedDotSpeed (GameController.CurrentLevel) * _tov.Speed ();
            else
                Speed = _tov.PacManDotSpeed (GameController.CurrentLevel) * _tov.Speed ();
        }
        if (EatingBigDots) {
            Speed = _tov.PacManPowerPelletSpeed (GameController.CurrentLevel) * _tov.Speed ();
        }

        if (!EatingBigDots && !EatingSmallDots) {
            if (Frightened)
                Speed = _tov.PacManFrightenedSpeed (GameController.CurrentLevel) * _tov.Speed ();
            else
                Speed = _tov.PacManSpeed (GameController.CurrentLevel) * _tov.Speed ();
        }

        //Debug.Log ("PAC MAN SPEED " + Speed);
    }

    /// <summary>
    /// Check to see if the ghosts and Pac Man occupy the same tile
    /// </summary>
    void CheckForGhostCollision()
    {
        GhostMove ghost = GameController.GhostAtPacManTile ();
        if (ghost != null && (ghost.CurrentMode == GhostMove.Mode.CHASE || ghost.CurrentMode == GhostMove.Mode.SCATTER)) {
            //StartCoroutine (ShowDeathAnimation ());
        }
    }

    IEnumerator ShowDeathAnimation()
    {
        AnimationSpeed = 0.0f;
        dying = true;

        _audio.SirenPlaying = false;
        _audio.BlueGhostsPlaying = false;
        _audio.GhostEyesPlaying = false;

        GameController.IsReady = false;
        Animation = false;
        yield return new WaitForSecondsRealtime (0.75f);
        BlinkyMover.Hidden = true;
        InkyMover.Hidden = true;
        PinkyMover.Hidden = true;
        ClydeMover.Hidden = true;
        yield return new WaitForSecondsRealtime (0.25f);
        AnimationSpeed = 0.8f;
        _audio.PlayDeath ();
        Animation = true;
        Animator.SetBool ("Died", true);
        yield return new WaitForSecondsRealtime (2.0f);
        Hidden = true;
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
