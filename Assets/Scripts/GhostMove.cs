﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostMove : BaseActor {
    public GameObject PacMan;

    public GameObject Ghost200;
    public GameObject Ghost400;
    public GameObject Ghost800;
    public GameObject Ghost1600;

    public enum Ghost
    {
        BLINKY,
        PINKY,
        INKY,
        CLYDE
    }

    public enum Mode
    {
        CHASE,
        SCATTER,
        FRIGHTENED
    }

    public enum Animations
    {
        FRIGHTENED,
        SEMI_FRIGHTENED,
        EYES,
        NORMAL
    }

    private readonly Color _blinkyColor = new Color (.81f, .24f, .098f);
    private readonly Color _pinkyColor = new Color (.917f, .509f, .898f);
    private readonly Color _inkyColor = new Color (.274f, .749f, .933f);
    private readonly Color _clydeColor = new Color (.858f, .521f, .109f);

    public Ghost ThisGhost;
    public Mode CurrentMode = Mode.CHASE;

    private Vector2 _scatterTarget = Vector2.zero;
    private readonly List<Vector2> _directions = new List<Vector2> ();
    private TableOfValues _tov = TableOfValues.Instance();
    private GhostMove _blinkyMover;
    private PacManMove _pacManMover;

    private bool _inCruiseElroy;

    private bool _inGhostHouse;
    private bool _leavingGhostHouse;
    private int _dotCounter;
    private int _dotsToLeave;
    private bool _isPreferred;

    public bool IsEaten;
    public bool IsBlinking;
    public int CurrentBlink;

    private float _playStartTime;
    private readonly Vector2 _home = new Vector2 (14, 22);
    private Vector2 _ghostHome = Vector2.zero;

    #region Start

    private new void Start()
    {
        base.Start ();

        _directions.Add (Vector2.up);
        _directions.Add (Vector2.down);
        _directions.Add (Vector2.left);
        _directions.Add (Vector2.right);

        // Start off moving left
        SetDestination (Vector2.left);
        Direction = Vector2.left;
        Animation = true;

        _pacManMover = PacMan.GetComponent<PacManMove> ();
        if (ThisGhost == Ghost.INKY) {
            Debug.Log ("Linking Blinky's mover to Inky's");
            var blinky = GameObject.FindGameObjectWithTag ("Blinky");
            _blinkyMover = blinky.GetComponent<GhostMove> ();
        }
        SetDotsToLeave();
    }

    #endregion
        
    #region FixedUpdate

    private void FixedUpdate () {
        if (GameController.IsReady) {
            if (_playStartTime == 0.0f)
                _playStartTime = Time.fixedTime;
            
            SetMode ();
            Animation = true;
            SetGhostSpeed ();
            Animate ();
            SetPreferredGhost ();
            Move ();
            CheckCollision ();
            PutGhostBackInHouse ();
            LeaveGhostHouse ();
            NavigateGhost ();
        } else {
            Animation = false;
        }
	}

    #endregion

    private void CheckCollision()
    {
        if (_pacManMover.Tile != Tile || CurrentMode != Mode.FRIGHTENED || IsEaten) return;
        Debug.Log (ThisGhost + " has been eaten");
        StartCoroutine (EatGhost ());
    }

    private IEnumerator EatGhost()
    {
        Vector2 position = transform.position;

        _pacManMover.Hidden = true;
        Hidden = true;
        IsEaten = true;
        IsBlinking = false;

        GameObject points = null;
        switch (GameController.GhostsEaten) {
        case 0:
            points = Instantiate (Ghost200);
            GameController.AddPoints (GameController.PointSource.FirstGhost);
            break;
        case 1:
            points = Instantiate (Ghost400);
            GameController.AddPoints (GameController.PointSource.SecondGhost);
            break;
        case 2:
            points = Instantiate (Ghost800);
            GameController.AddPoints (GameController.PointSource.ThirdGhost);
            break;
        case 3:
            points = Instantiate (Ghost1600);
            GameController.AddPoints (GameController.PointSource.FourthGhost);
            break;
        }

        GameController.GhostsEaten++;
        if (GameController.GhostsEaten == 4)
            GameController.GhostsEaten = 0;
        
        points.transform.position = position;

        GameController.IsReady = false;
        Audio.PlayEatGhost ();

        Destroy (points, Audio.EatGhostLength);
        yield return new WaitForSecondsRealtime (Audio.EatGhostLength);

        GameController.IsReady = true;
        SetAnimation (Animations.EYES);

        Audio.BlueGhostsPlaying = false;
        Audio.GhostEyesPlaying = true;
        Audio.SirenPlaying = false;

        Hidden = false;
        _pacManMover.Hidden = false;
    }
       
    public void SetAnimation(Animations anim)
    {
        switch (anim) {
        case Animations.FRIGHTENED:
            Animator.SetBool ("Frightened", true);
            Animator.SetBool ("SemiFrightened", false);
            Animator.SetBool ("Eaten", false);
            break;
        case Animations.SEMI_FRIGHTENED:
            Animator.SetBool ("SemiFrightened", true);
            Animator.SetBool ("Frightened", false);
            Animator.SetBool ("Eaten", false);
            break;
        case  Animations.EYES:
            Animator.SetBool ("SemiFrightened", false);
            Animator.SetBool ("Frightened", false);
            Animator.SetBool ("Eaten", true);
            break;
        case Animations.NORMAL:
        default:
            Animator.SetBool ("SemiFrightened", false);
            Animator.SetBool ("Frightened", false);
            Animator.SetBool ("Eaten", false);
            break;
        }
    }

    private float LevelTime()
    {
        return Time.fixedTime - _playStartTime;
    }
       
    #region Ghost House Rules

    /// <summary>
    /// Set the number of dots PacMan needs to have eaten for ghosts to leave the ghost house
    /// </summary>
    private void SetDotsToLeave()
    {
        _dotsToLeave = 0;

        if (GameController.CurrentLevel == 1) {
            if (ThisGhost == Ghost.PINKY)
                _dotsToLeave = 0;
            if (ThisGhost == Ghost.INKY)
                _dotsToLeave = 30;
            if (ThisGhost == Ghost.CLYDE)
                _dotsToLeave = 60;
        }

        if (GameController.CurrentLevel == 2) {
            if (ThisGhost == Ghost.PINKY || ThisGhost == Ghost.INKY)
                _dotsToLeave = 0;
            if (ThisGhost == Ghost.CLYDE)
                _dotsToLeave = 50;
        }
    }

    /// <summary>
    /// Once our eyes hit the home location, put them back in the ghost house and reincarnate the ghost
    /// </summary>
    private void PutGhostBackInHouse()
    {
        if (IsEaten && Tile == _home){
            Destination = _ghostHome;
        }

        if (IsEaten && Tile == _ghostHome) {
            Audio.GhostEyesPlaying = false;
            Audio.BlueGhostsPlaying = true;
            SetAnimation (Animations.NORMAL);
            _dotsToLeave = 0;
            _dotCounter = 0;
            _inGhostHouse = true;
            IsEaten = false;
            CurrentMode = GetMode ();
        }
    }

    /// <summary>
    /// Get the ghost out of the house
    /// </summary>
    public void LeaveGhostHouse()
    {
        //if ((DotCounter >= DotsToLeave && InGhostHouse) || (InGhostHouse && IsPreferred && GameController.TimeSinceLastDot >= 4.0f)) {
        if(_dotCounter >= _dotsToLeave && _inGhostHouse) {
            _leavingGhostHouse = true;

            // Wait until the ghost hits the top of the ghost house
            if (transform.position.y < 19) {
                Destination = new Vector2 (transform.position.x, 19);
                Direction = Vector2.up;
            }

            if (transform.position.x != 14 && transform.position.y == 19) {
                Destination = new Vector2 (14, 19);
                if (ThisGhost == Ghost.INKY)
                    Direction = Vector2.right;
                if (ThisGhost == Ghost.CLYDE)
                    Direction = Vector2.left;
            }

            if ((Vector2)transform.position == new Vector2 (14, 19)) {
                Destination = new Vector2 (14, 21.5f);
                Direction = Vector2.up;
            }

            if ((Vector2)transform.position == new Vector2 (14, 21.5f)) {
                _inGhostHouse = false;
                SetDestination (Vector2.left);
                Direction = Vector2.left;
            }
        }
    }

    /// <summary>
    /// Figure out which ghost is tracking dot count
    /// </summary>
    private void SetPreferredGhost()
    {
        _isPreferred = (ThisGhost == Ghost.PINKY && _inGhostHouse) || (ThisGhost == Ghost.INKY && _inGhostHouse) || (ThisGhost == Ghost.CLYDE && _inGhostHouse);
    }

    /// <summary>
    /// Increase the dot count for the preferred ghost
    /// </summary>
    public void IncreaseDotCount()
    {
        if (_isPreferred)
            _dotCounter++;
    }

    /// <summary>
    /// Bounce the ghost around in the ghost house
    /// </summary>
    private void BounceInGhostHouse()
    {
        if (!_inGhostHouse || _leavingGhostHouse)
            return;

        if (transform.position.y < 19 && Direction == Vector2.up) {
            Destination = new Vector2 (transform.position.x, 19);
            Direction = Vector2.up;
        } else {
            Direction = Vector2.down;
        }
        if (transform.position.y > 18 && Direction == Vector2.down) {
            Destination = new Vector2 (transform.position.x, 18);
            Direction = Vector2.down;
        } else {
            Direction = Vector2.up;
        }
    }

    #endregion

    #region Ghost Mode

    /// <summary>
    /// Set the ghost's current mode
    /// </summary>
    private void SetMode()
    {
        // If frightened, just return
        if (CurrentMode == Mode.FRIGHTENED) return;
        var mode = GetMode ();
        
        // If the new mode is the same as the current mode, or the ghost is in the house, return
        if (mode == CurrentMode || _inGhostHouse) return;
        
        // Changing the mode reverses the direction of the ghost
        Direction = -Direction;
        Debug.Log (LevelTime() + " : Setting " + ThisGhost + " mode from " + CurrentMode + " to " + mode);
        CurrentMode = mode;
    }

    /// <summary>
    /// Get the ghost mode based on the number of seconds since the start of the level
    /// </summary>
    /// <returns>The mode.</returns>
    public Mode GetMode()
    {
        int level = GameController.CurrentLevel;
        float levelSeconds = LevelTime ();

        if ((level >= 1 && level <= 4 && levelSeconds <= 7) || (level >=5 && levelSeconds <= 5))
            return Mode.SCATTER;
        if ((level >= 1 && level <= 4 && levelSeconds >= 7 && levelSeconds < 27) || (level >= 5 && levelSeconds >= 5 && levelSeconds < 25))
            return Mode.CHASE;
        if ((level >= 1 && level <= 4 && levelSeconds >= 27 && levelSeconds < 34) || (level >= 5 && levelSeconds >= 25 && levelSeconds < 30))
            return Mode.SCATTER;
        if ((level >= 1 && level <= 4 && levelSeconds >= 34 && levelSeconds < 54) || (level >=5 && levelSeconds >= 30 && levelSeconds < 50))
            return Mode.CHASE;
        if ((level >= 1 && level <= 4 && levelSeconds >= 54 && levelSeconds < 59) || (level >= 5 && levelSeconds >= 50 && levelSeconds < 55))
            return Mode.SCATTER;
        if ((level == 1 && levelSeconds >= 59 && levelSeconds < 79) || (level >= 2 && level <= 4 && levelSeconds >= 59 && levelSeconds < 1092) || (level >= 5 && levelSeconds >= 55 && levelSeconds < 1092))
            return Mode.CHASE;
        if ((level == 1 && levelSeconds >= 79 && levelSeconds < 84) || (level >= 2 && level <= 4 && levelSeconds >= 1092 && levelSeconds < 1092.01666666f) || (level >=5 && levelSeconds >= 1092 && levelSeconds < 1092.01666666f))
            return Mode.SCATTER;

        return Mode.CHASE;
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="GhostMove"/> is frightened.
    /// </summary>
    /// <value><c>true</c> if frightened; otherwise, <c>false</c>.</value>
    public bool Frightened
    {
        get {
            return CurrentMode == Mode.FRIGHTENED;
        }
        set {
            if (value) {
                if (!IsEaten) {
                    CurrentMode = Mode.FRIGHTENED;
                    SetAnimation (Animations.FRIGHTENED);
                    Direction = -Direction;
                }
            } else {
                CurrentMode = GetMode ();
                IsBlinking = false;
                CurrentBlink = 0;
                GameController.GhostsEaten = 0;
                SetAnimation (Animations.NORMAL);
            }
        }
    }

    /// <summary>
    /// Kick off the coroutine to blink the ghosts when they're frightened
    /// </summary>
    public void DoBlinkGhost()
    {
        if (IsEaten)
            return;
        
        IsBlinking = true;
        CurrentBlink = 0;

        StartCoroutine (BlinkGhost());
    }

    /// <summary>
    /// If the ghost is frightened, it will start to blink after a certain number of seconds.
    /// </summary>
    public IEnumerator BlinkGhost()
    {        
        int totalBlinks = TableOfValues.GhostFrightenedFlashes (GameController.CurrentLevel);
        while (CurrentBlink <= totalBlinks && !IsEaten) {
            SetAnimation (Animations.SEMI_FRIGHTENED);
            yield return new WaitForSeconds (.25f);
            if (IsEaten)
                continue;
            SetAnimation (Animations.FRIGHTENED);
            yield return new WaitForSeconds (.25f);
            CurrentBlink++;
        }

        if(!IsEaten)
            Frightened = false;
    }

    #endregion

    #region Ghost Movement

    /// <summary>
    /// Set the ghost's speed based on a few factors
    /// </summary>
    private void SetGhostSpeed()
    {
        // Ghost is eaten and is currently a set of eyes on their way back to the ghost house
        if (IsEaten) {
            Speed = TableOfValues.Speed () * 2.0f;
            return;
        }

        // Ghost is in the tunnel
        if(InTunnel)
        {
            Speed = TableOfValues.GhostTunnelSpeed (GameController.CurrentLevel) * TableOfValues.Speed ();
            return;
        }

        // Ghost is frightened
        if (CurrentMode == Mode.FRIGHTENED) {
            Speed = TableOfValues.GhostFrightenedSpeed (GameController.CurrentLevel) * TableOfValues.Speed ();
            return;
        }

        // Ghost is in the ghost house. Cut the speed in half
        if (_inGhostHouse) {
            Speed = TableOfValues.Speed () * 0.5f;
            return;
        }

        // Set normal speed for this level
        if (!_inCruiseElroy) {
            Speed = TableOfValues.GhostSpeed (GameController.CurrentLevel) * TableOfValues.Speed ();   
            return;
        }         

        // Check whether Blinky is in Cruise Elroy mode
        CruiseElroy ();
    }

    /// <summary>
    /// Find the right direction to go based on mode. If 2 or more directions have the same distance from
    /// the target tile, the order of precedence is up, left, down, right.
    /// </summary>
    /// <returns>The direction that the ghost will be traveling</returns>
    /// <param name="exits">The list of available exits from the ghost's location</param>
    private Vector2 GetDirection(ICollection<Vector2> exits)
    {
        var target = Target();

        var shortest = exits.Select(exit => Vector2.Distance(TileCenter + exit, target)).Concat(new[] {float.PositiveInfinity}).Min();

        var upDistance = Vector2.Distance(TileCenter + Vector2.up, target);
        var downDistance = Vector2.Distance(TileCenter + Vector2.down, target);
        var leftDistance = Vector2.Distance(TileCenter + Vector2.left, target);
        var rightDistance = Vector2.Distance(TileCenter + Vector2.right, target);

        if (exits.Contains(Vector2.up) && Mathf.Approximately(shortest, upDistance))
            return Vector2.up;
        if (exits.Contains(Vector2.left) && Mathf.Approximately(shortest, leftDistance))
            return Vector2.left;
        if (exits.Contains(Vector2.down) && Mathf.Approximately(shortest, downDistance))
            return Vector2.down;
        if (exits.Contains(Vector2.right) && Mathf.Approximately(shortest, rightDistance))
            return Vector2.right;

        return Vector2.zero;
    }

    /// <summary>
    /// Retrieve the available exits from the current location
    /// </summary>
    /// <returns>The exits.</returns>
    private List<Vector2> GetExits()
    {
        var canGoUp = !(Maze.SpecialLocations().Contains (Tile) && (CurrentMode == Mode.CHASE || CurrentMode == Mode.SCATTER));
        var exits = new List<Vector2> ();

        foreach (var dir in _directions) {
            if (dir == Vector2.up && !canGoUp)
                continue;
            if (dir == -Direction) continue;
            var dest = Tile + dir;
            if (Maze.ValidLocations ().Contains (dest)) {
                exits.Add (dir);
            }
        }

        return exits;
    }

    /// <summary>
    /// Figure out where the ghost should be going
    /// </summary>
    private void NavigateGhost()
    {
        // This is for ghosts that have left the ghost house
        if (!_inGhostHouse) {
            ShowTarget ();
            // The ghost has reached his/her destination. Find the next destination
            if (TileCenter == Destination) {
                // Based on the ghost's location, figure out which directions he can go based
                // on our mode.
                List<Vector2> exits = GetExits ();
                if (exits.Count == 1) {
                    // Only a single exit? Go for it.
                    SetDestination (exits [0]);
                } else {
                    // Based on mode, pick the best exit for our target
                    SetDestination (GetDirection (exits));
                }
            }
        } else {
            BounceInGhostHouse ();
        }
    }

    /// <summary>
    /// Check to see if Blinky can enter Cruise Elroy mode
    /// </summary>
    private void CruiseElroy()
    {
        // Check for Cruise Elroy 1
        if (ThisGhost == Ghost.BLINKY && GameController.SmallDotsLeft == TableOfValues.CruiseElroy1DotsLeft(GameController.CurrentLevel) && !_inCruiseElroy) {
            Speed = TableOfValues.CruiseElroy1Speed (GameController.CurrentLevel) * TableOfValues.Speed ();
            _inCruiseElroy = true;
        }

        // Check for Cruise Elroy 2
        if (ThisGhost == Ghost.BLINKY && GameController.SmallDotsLeft == TableOfValues.CruiseElroy2DotsLeft (GameController.CurrentLevel)) {
            Speed = TableOfValues.CruiseElroy2Speed (GameController.CurrentLevel) * TableOfValues.Speed ();
            _inCruiseElroy = true;
        }
    }

    /// <summary>
    /// Initialize the ghost
    /// </summary>
    public void GhostInit()
    {
        CurrentMode = Mode.SCATTER;
        IsEaten = false;
        IsBlinking = false;
        Destination = Vector2.zero;
        _playStartTime = 0.0f;
        _inCruiseElroy = false;
        _leavingGhostHouse = false;
        CurrentMode = Mode.SCATTER;

        if (Animator == null)
            Anim = GetComponent<Animator> ();

        SetAnimation (Animations.NORMAL);

        switch (ThisGhost) {
        case Ghost.BLINKY:
            Location = new Vector2 (14.0f, 21.5f);
            Direction = Vector2.left;
            Destination = TileCenter + Vector2.left;
            _inGhostHouse = false;
            _scatterTarget = new Vector2 (26, 36);
            _ghostHome = new Vector2 (14, 19);
            break;
        case Ghost.PINKY:
            Location = new Vector2 (14.0f, 18.5f);
            Direction = Vector2.down;
            _inGhostHouse = true;
            _scatterTarget = new Vector2 (3, 36);
            _ghostHome = new Vector2 (14, 19);
            break;
        case Ghost.INKY:
            Location = new Vector2 (12.0f, 18.5f);
            Direction = Vector2.up;
            _inGhostHouse = true;
            _scatterTarget = new Vector2 (28, 1);
            _ghostHome = new Vector2 (12, 19);
            break;
        case Ghost.CLYDE:
            Location = new Vector2 (16.0f, 18.5f);
            Direction = Vector2.up;
            _inGhostHouse = true;
            _scatterTarget = new Vector2 (1, 1);
            _ghostHome = new Vector2 (16, 19);
            break;
        }
    }

    #endregion

    #region Ghost Targeting

    /// <summary>
    /// Get the ghost's target
    /// </summary>
    private Vector2 Target()
    {
        // Quick out if the ghost has been eaten
        if (IsEaten) {
            return _home;
        }

        switch (CurrentMode) {
        case Mode.SCATTER:
            return ProcessScatter ();
        case Mode.CHASE:
            switch (ThisGhost) {
            case Ghost.BLINKY:
                return BlinkyTarget ();
            case Ghost.PINKY:
                return PinkyTarget ();
            case Ghost.INKY:
                return InkyTarget ();
            case Ghost.CLYDE:
                return ClydeTarget ();
            default:
                return Vector2.zero;
            }
        case Mode.FRIGHTENED:
            return ProcessFrightened ();
        default:
            return Vector2.zero;
        }
    }

    /// <summary>
    /// If in scatter mode, return the scatter target for Inky, Pinky and Clyde.
    /// If Blinky is in "Cruise Elroy" mode, his target is still Pac Man
    /// </summary>
    /// <returns>The scatter target for this ghost</returns>
    private Vector2 ProcessScatter()
    {
        if (ThisGhost == Ghost.BLINKY && _inCruiseElroy) {
            return BlinkyTarget ();
        }
        return _scatterTarget;
    }

    /// <summary>
    /// Pick a random exit
    /// </summary>
    /// <returns>The next location</returns>
    private Vector2 ProcessFrightened()
    {
        var exits = GetExits ();
        switch (exits.Count)
        {
            case 0:
                return TileCenter - Direction;
            case 1:
                return TileCenter + exits [0];
            default:
                return TileCenter + exits [Random.Range (0, exits.Count - 1)];
        }
    }

    /// <summary>
    /// Blinky targets PacMan
    /// </summary>
    /// <returns>The target.</returns>
    private Vector2 BlinkyTarget()
    {
        return _pacManMover.TileCenter;
    }

    /// <summary>
    /// Pinky's target is always 4 tiles ahead of PacMan's current direction
    /// </summary>
    /// <returns>The target.</returns>
    private Vector2 PinkyTarget()
    {
        Vector2 target;
        // Simulate overflow bug in original PacMan
        if (_pacManMover.Direction == Vector2.up) {
            target = new Vector2 (_pacManMover.TileCenter.x - 4, _pacManMover.TileCenter.y + 4);
        } else {
            target = _pacManMover.TileCenter + (_pacManMover.Direction * 4);
        }
        return target;
    }

    /// <summary>
    /// Inky has similar targeting to Pinky, but it also uses Blinky's position in its calculation
    /// </summary>
    /// <returns>The target.</returns>
    private Vector2 InkyTarget()
    {
        Vector2 target;
        if (_pacManMover.Direction == Vector2.up) {
            target = new Vector2 (_pacManMover.TileCenter.x - 2, _pacManMover.TileCenter.y + 2);
        } else {
            target = _pacManMover.TileCenter + (_pacManMover.Direction * 2);
        }

        // Compute vector from blinky's position to target and then double to get Inky's target
        var blinkysPos = _blinkyMover.TileCenter;
        var diff =  target - blinkysPos;

        return target + diff;
    }

    /// <summary>
    /// Clyde will target PacMan if he is within 8 tiles of him. Otherwise, he reverts to scatter mode.
    /// </summary>
    /// <returns>The target.</returns>
    private Vector2 ClydeTarget()
    {
        var distance = Vector2.Distance (TileCenter, _pacManMover.TileCenter);
        return distance > 8 ? _pacManMover.TileCenter : _scatterTarget;
    }

    /// <summary>
    /// Draw colored boxes - one for each ghost - showing what that ghost is targeting
    /// </summary>
    private void ShowTarget()
    {
        var target = Target ();
        Color color;
        switch (ThisGhost) {
        case Ghost.BLINKY:
            color = _blinkyColor;
            break;
        case Ghost.CLYDE:
            color = _clydeColor;
            break;
        case Ghost.PINKY:
            color = _pinkyColor;
            break;
        case Ghost.INKY:
            color = _inkyColor;
            break;
        default:
            color = Color.white;
            break;
        }

        Debug.DrawLine(TileCenter, target, color);
        Debug.DrawLine (new Vector3 (target.x - .5f, target.y - .5f), new Vector3 (target.x + .5f, target.y - .5f), color, 0, false);
        Debug.DrawLine (new Vector3 (target.x - .5f, target.y - .5f), new Vector3 (target.x - .5f, target.y + .5f), color, 0, false);
        Debug.DrawLine (new Vector3 (target.x - .5f, target.y + .5f), new Vector3 (target.x + .5f, target.y + .5f), color, 0, false);
        Debug.DrawLine (new Vector3 (target.x + .5f, target.y - .5f), new Vector3 (target.x + .5f, target.y + .5f), color, 0, false);
    }

    #endregion
}
