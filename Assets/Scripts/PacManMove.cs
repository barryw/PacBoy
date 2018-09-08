using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManMove : BaseActor
{    
    public GameObject Blinky;
    public GameObject Pinky;
    public GameObject Inky;
    public GameObject Clyde;

    private GhostMove _blinkyMover;
    private GhostMove _pinkyMover;
    private GhostMove _inkyMover;
    private GhostMove _clydeMover;

    private Vector2 _nextDir = Vector2.right;

    private bool _dying;
    private bool _frightened;

    private new void Start () {
        base.Start ();
        Direction = Vector2.left;
        Animation = true;
        AnimationSpeed = 10.5f;

        _blinkyMover = Blinky.GetComponent<GhostMove> ();
        _pinkyMover = Pinky.GetComponent<GhostMove> ();
        _inkyMover = Inky.GetComponent<GhostMove> ();
        _clydeMover = Clyde.GetComponent<GhostMove> ();
	}

    private void FixedUpdate()
    {
        if (!GameController.IsReady || _dying) return;
        
        SetPacManSpeed ();
        CheckForGhostCollision ();
        Move ();
        Animate();
    }
    
    private void Update()
    {
        ReadInput ();
        ReadInputAndMove();
    }

    /// <summary>
    /// PacMan doesn't really get frightened, but this lets us know that the ghosts are
    /// </summary>
    public bool Frightened
    {
        get {
            return _frightened;
        }
        set {
            _frightened = false;
        }
    }

    public bool EatingSmallDots { private get; set; }

    public bool EatingBigDots { private get; set; }

    private void SetPacManSpeed()
    {
        if (EatingSmallDots) {
            if (Frightened)
                Speed = TableOfValues.PacManFrightenedDotSpeed (GameController.CurrentLevel) * TableOfValues.Speed ();
            else
                Speed = TableOfValues.PacManDotSpeed (GameController.CurrentLevel) * TableOfValues.Speed ();
        }
        if (EatingBigDots) {
            Speed = TableOfValues.PacManPowerPelletSpeed (GameController.CurrentLevel) * TableOfValues.Speed ();
        }

        if (EatingBigDots || EatingSmallDots) return;
        if (Frightened)
            Speed = TableOfValues.PacManFrightenedSpeed (GameController.CurrentLevel) * TableOfValues.Speed ();
        else
            Speed = TableOfValues.PacManSpeed (GameController.CurrentLevel) * TableOfValues.Speed ();
    }

    /// <summary>
    /// Check to see if the ghosts and Pac Man occupy the same tile
    /// </summary>
    private void CheckForGhostCollision()
    {
        var ghost = GhostAtPacManTile ();
        if (ghost != null && (ghost.CurrentMode == GhostMove.Mode.Chase || ghost.CurrentMode == GhostMove.Mode.Scatter)) {
            StartCoroutine (ShowDeathAnimation ());
        }
    }

    /// <summary>
    /// Get the ghost that currently shares PacMan's tile
    /// </summary>
    /// <returns>The at pac man tile.</returns>
    private GhostMove GhostAtPacManTile()
    {
        GhostMove ghost = null;

        if (_blinkyMover.Tile == Tile)
            ghost = _blinkyMover;

        if(_pinkyMover.Tile == Tile)
            ghost = _pinkyMover;

        if (_inkyMover.Tile == Tile)
            ghost = _inkyMover;

        if (_clydeMover.Tile == Tile)
            ghost = _clydeMover;

        return ghost;
    }

    /// <summary>
    /// PacMan has been eaten!
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowDeathAnimation()
    {
        AnimationSpeed = 0.0f;
        _dying = true;
        
        Audio.SirenPlaying = false;
        Audio.BlueGhostsPlaying = false;
        Audio.GhostEyesPlaying = false;

        GameController.IsReady = false;
        Animation = false;
        yield return new WaitForSecondsRealtime (0.75f);
        _blinkyMover.Hidden = true;
        _inkyMover.Hidden = true;
        _pinkyMover.Hidden = true;
        _clydeMover.Hidden = true;
        yield return new WaitForSecondsRealtime (0.25f);
        AnimationSpeed = 0.8f;
        Audio.PlayDeath ();
        Animation = true;
        Animator.SetBool ("Died", true);        
        yield return new WaitForSecondsRealtime (2.0f);
        Hidden = true;
        yield return new WaitForSecondsRealtime(.75f);
        Animator.SetBool("Died", false);
        _dying = false;
        Animation = true;
        GameController.numberOfPacs--;
        GameController.Ready();
    }

    /// <summary>
    /// Get player input and figure out which direction to move PacMan
    /// </summary>
    private void ReadInput()
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

    private void ReadInputAndMove()
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

    /// <summary>
    /// Move PacMan back to his starting position
    /// </summary>
    public void PacManInit()
    {
        Frightened = false;
        Animator.SetBool ("Died", false);
        Location = Maze.PacManHome;
        Animation = false;
    }
}
