using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using JetBrains.Annotations;
using MonsterLove;
using MonsterLove.Events;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GhostStates
{
    InitialPosition,
    InGhostHouse,
    LeavingGhostHouse,
    Chase,
    Scatter,
    Frightened,
    Dead
}

public enum GhostAnimations
{
    AnimFrightened,
    AnimSemiFrightened,
    AnimEyes,
    AnimNormal
}

public class GhostDriver
{
    public StateEvent FixedUpdate;
}

public class GhostMove : BaseActor {
    StateMachine<GhostStates, GhostDriver> _fsmState;
    StateMachine<GhostAnimations> _fsmAnimation;
    
    public GameObject PacMan;
    public GameObject Ghost200;
    public GameObject Ghost400;
    public GameObject Ghost800;
    public GameObject Ghost1600;

    public enum Ghost
    {
        Blinky,
        Pinky,
        Inky,
        Clyde
    }

    private readonly Color _blinkyColor = new Color (.81f, .24f, .098f);
    private readonly Color _pinkyColor = new Color (.917f, .509f, .898f);
    private readonly Color _inkyColor = new Color (.274f, .749f, .933f);
    private readonly Color _clydeColor = new Color (.858f, .521f, .109f);

    public Ghost ThisGhost;

    private Vector2 _scatterTarget = Vector2.zero;
    private readonly List<Vector2> _directions = new();
    private GhostMove _blinkyMover;
    private PacManMove _pacManMover;

    private bool _inCruiseElroy;
    
    private int _dotCounter;
    private int _dotsToLeave;
    private bool _isPreferred;
    
    public bool IsBlinking;
    public int CurrentBlink;

    private float _playStartTime;
    private readonly Vector2 _home = new Vector2 (14, 22);
    private Vector2 _ghostHome = Vector2.zero;

    private bool IsEatable => _pacManMover.Tile == Tile && CurrentMode == GhostStates.Frightened;

    public GhostStates CurrentMode
    {
        get => _fsmState.State;
        set => _fsmState.ChangeState(value);
    }

    public GhostAnimations CurrentAnimation
    {
        get => _fsmAnimation.State;
        set => _fsmAnimation.ChangeState(value);
    }

    private Color GhostColor
    {
        get
        {
            return ThisGhost switch
            {
                Ghost.Blinky => _blinkyColor,
                Ghost.Pinky => _pinkyColor,
                Ghost.Inky => _inkyColor,
                Ghost.Clyde => _clydeColor,
                _ => Color.white
            };
        }
    }

    #region Start

    private void Awake()
    {
        Anim = GetComponent<Animator> ();
        
        _fsmState = new StateMachine<GhostStates, GhostDriver>(this);
        _fsmAnimation = new StateMachine<GhostAnimations>(this);
        
        CurrentMode = GhostStates.InitialPosition;
        CurrentAnimation = GhostAnimations.AnimNormal;
    }
    
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

        _pacManMover = PacMan.GetComponent<PacManMove> ();
        if (ThisGhost == Ghost.Inky) {
            var blinky = GameObject.FindGameObjectWithTag ("Blinky");
            _blinkyMover = blinky.GetComponent<GhostMove> ();
        }
        SetDotsToLeave();
    }

    #endregion

    #region Ghost States
    
    #region Initial Position

    [UsedImplicitly]
    void InitialPosition_Enter()
    {
        Animation = false;
        IsBlinking = false;
        Destination = Vector2.zero;
        _playStartTime = 0.0f;
        _inCruiseElroy = false;
        CurrentAnimation = GhostAnimations.AnimNormal;

        switch (ThisGhost) {
            case Ghost.Blinky:
                Location = Maze.BlinkyStartLocation;
                Direction = Maze.BlinkyStartDirection;
                Destination = TileCenter + Vector2.left;
                CurrentMode = GhostStates.Scatter;
                _scatterTarget = Maze.BlinkyScatterTarget;
                _ghostHome = Maze.Ghost2Home;
                break;
            case Ghost.Pinky:
                Location = Maze.PinkyStartLocation;
                Direction = Maze.PinkyStartDirection;
                CurrentMode = GhostStates.InGhostHouse;
                _scatterTarget = Maze.PinkyScatterTarget;
                _ghostHome = Maze.Ghost2Home;
                break;
            case Ghost.Inky:
                Location = Maze.InkyStartLocation;
                Direction = Maze.InkyStartDirection;
                CurrentMode = GhostStates.InGhostHouse;
                _scatterTarget = Maze.InkyScatterTarget;
                _ghostHome = Maze.Ghost1Home;
                break;
            case Ghost.Clyde:
                Location = Maze.ClydeStartLocation;
                Direction = Maze.ClydeStartDirection;
                CurrentMode = GhostStates.InGhostHouse;
                _scatterTarget = Maze.ClydeScatterTarget;
                _ghostHome = Maze.Ghost3Home;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [UsedImplicitly]
    private void InitialPosition_FixedUpdate()
    {
        DoUpdates();
    }
    
    #endregion
    
    #region In Ghost House

    [UsedImplicitly]
    private void InGhostHouse_Enter()
    {
        Animation = false;
    }

    [UsedImplicitly]
    private void InGhostHouse_FixedUpdate()
    {
        DoUpdates();
        
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
    
    #region Leaving Ghost House
    
    [UsedImplicitly]
    private void LeavingGhostHouse_Enter()
    {
        CurrentAnimation = GhostAnimations.AnimNormal;
        Destination = _home;
    }
    
    [UsedImplicitly]
    private void LeavingGhostHouse_FixedUpdate()
    {
        DoUpdates();
        
        // Wait until the ghost hits the top of the ghost house
        if (transform.position.y < 19) {
            Destination = new Vector2 (transform.position.x, 19);
            Direction = Vector2.up;
        }

        // If we're inky and clyde, we need to move to the center of the ghost house before exiting
        // Blinky and pinky can just exit
        if (!Mathf.Approximately(transform.position.x, 14) && Mathf.Approximately(transform.position.y, 19))
        {
            Destination = Maze.Ghost2Home;
            switch (ThisGhost)
            {
                case Ghost.Inky:
                    Direction = Vector2.right;
                    break;
                case Ghost.Clyde:
                    Direction = Vector2.left;
                    break;
            }
        }

        // Once in the center, set their target to the ghost house door
        if ((Vector2)transform.position == Maze.Ghost2Home)
        {
            Destination = Maze.BlinkyStartLocation;
            Direction = Vector2.up;
        }
        
        // If they haven't reached the door yet, just keep moving
        if ((Vector2) transform.position != Maze.BlinkyStartLocation) return;
        
        // Once outside, set the mode and direction
        CurrentMode = GetMode();
        SetDestination (Vector2.left);
        Direction = Vector2.left;
    }
    
    #endregion
    
    #region Chase
    
    [UsedImplicitly]
    void Chase_Enter()
    {
        CurrentAnimation = GhostAnimations.AnimNormal;
    }

    [UsedImplicitly]
    void Chase_FixedUpdate()
    {
        DoUpdates();
    }
    
    #endregion

    #region Frightened
    
    [UsedImplicitly]
    private void Frightened_Enter()
    {
        CurrentAnimation = GhostAnimations.AnimFrightened;
    }

    [UsedImplicitly]
    private void Frightened_FixedUpdate()
    {
        DoUpdates();
    }
    
    #endregion

    #region Scatter
    
    [UsedImplicitly]
    private void Scatter_Enter()
    {
        CurrentAnimation = GhostAnimations.AnimNormal;
    }

    [UsedImplicitly]
    private void Scatter_FixedUpdate()
    {
        DoUpdates();    
    }
    
    #endregion
    
    #region Dead
    
    [UsedImplicitly]
    private IEnumerator Dead_Enter()
    {
        Audio.PlayEatGhost();

        var position = transform.position;
        
        _pacManMover.Hidden = true;
        Hidden = true;
        IsBlinking = false;

        GameObject points;
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
            default:
                throw new ArgumentException();
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

        Audio.BlueGhostsPlaying = false;
        Audio.GhostEyesPlaying = true;
        Audio.SirenPlaying = false;

        Hidden = false;
        _pacManMover.Hidden = false;
        
        CurrentMode = GhostStates.Dead;
        CurrentAnimation = GhostAnimations.AnimEyes;
    }

    [UsedImplicitly]
    private void Dead_FixedUpdate()
    {
        DoUpdates();
    }

    #endregion

    #region Idle
    
    [UsedImplicitly]
    private void Idle_FixedUpdate()
    {
        DoUpdates();
    }

    #endregion
    
    private void DoUpdates()
    {
        DebugAids.ShowTarget(this, Target(), GhostColor);
        DebugAids.ShowGrid();

        if (!GameController.IsReady) return;
        
        if (Mathf.Approximately(_playStartTime, 0.0f))
            _playStartTime = Time.fixedTime;
            
        SetMode ();
        SetGhostSpeed ();
        Animate ();
        SetPreferredGhost ();
        Move ();
        CheckCollision ();
        PutGhostBackInHouse ();
        GetGhostDestination ();
        GetGhostOutOfGhostHouse ();
    }
    
    #endregion
    
    #region Ghost Animation States
    
    [UsedImplicitly]
    private void AnimFrightened_Enter()
    {
        Animator.SetBool (Constants.SEMI_FRIGTHENED, false);
        Animator.SetBool (Constants.FRIGHTENED, true);
        Animator.SetBool (Constants.EATEN, false);
    }

    [UsedImplicitly]
    private void AnimSemiFrightened_Enter()
    {
        Animator.SetBool (Constants.SEMI_FRIGTHENED, true);
        Animator.SetBool (Constants.FRIGHTENED, false);
        Animator.SetBool (Constants.EATEN, false);
    }

    [UsedImplicitly]
    private void AnimEyes_Enter()
    {
        Animator.SetBool (Constants.SEMI_FRIGTHENED, false);
        Animator.SetBool (Constants.FRIGHTENED, false);
        Animator.SetBool (Constants.EATEN, true);
    }

    [UsedImplicitly]
    private void AnimNormal_Enter()
    {
        Animator.SetBool (Constants.SEMI_FRIGTHENED, false);
        Animator.SetBool (Constants.FRIGHTENED, false);
        Animator.SetBool (Constants.EATEN, false);
    }
    
    #endregion
    
    #region FixedUpdate

    private void FixedUpdate () {
        _fsmState.Driver.FixedUpdate.Invoke();
        _fsmAnimation.Driver.FixedUpdate.Invoke();
    }

    #endregion

    private void CheckCollision()
    {
        if (IsEatable)
            CurrentMode = GhostStates.Dead;
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

        switch (GameController.CurrentLevel)
        {
            case 1:
            {
                _dotsToLeave = ThisGhost switch
                {
                    Ghost.Pinky => 0,
                    Ghost.Inky => 30,
                    Ghost.Clyde => 60,
                    _ => _dotsToLeave
                };

                break;
            }
            case 2:
            {
                switch (ThisGhost)
                {
                    case Ghost.Pinky:
                    case Ghost.Inky:
                        _dotsToLeave = 0;
                        break;
                    case Ghost.Clyde:
                        _dotsToLeave = 50;
                        break;
                    case Ghost.Blinky:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            }
        }
    }

    /// <summary>
    /// Determine if the ghost should leave the ghost house. If so, set the mode to LeavingGhostHouse
    /// </summary>
    private void GetGhostOutOfGhostHouse()
    {
        if (CurrentMode != GhostStates.InGhostHouse || GameController.SmallDotsEaten < _dotsToLeave) return;
        CurrentMode = GhostStates.LeavingGhostHouse;
    }
    
    /// <summary>
    /// Once our eyes hit the home location, put them back in the ghost house and reincarnate the ghost
    /// </summary>
    private void PutGhostBackInHouse()
    {
        if (CurrentMode != GhostStates.Dead) return;
        
        if (Tile == _home){
            Destination = _ghostHome;
        }
        
        if (CurrentMode != GhostStates.Dead || Tile != _ghostHome) return;
        
        Audio.GhostEyesPlaying = false;
        Audio.BlueGhostsPlaying = true;
        _dotsToLeave = 0;
        _dotCounter = 0;
        CurrentMode = GhostStates.LeavingGhostHouse;
    }

    /// <summary>
    /// Figure out which ghost is tracking dot count
    /// </summary>
    private void SetPreferredGhost()
    {
        _isPreferred = (ThisGhost == Ghost.Pinky && CurrentMode == GhostStates.InGhostHouse) 
                       || (ThisGhost == Ghost.Inky && CurrentMode == GhostStates.InGhostHouse) 
                       || (ThisGhost == Ghost.Clyde && CurrentMode == GhostStates.InGhostHouse);
    }

    /// <summary>
    /// Increase the dot count for the preferred ghost
    /// </summary>
    public void IncreaseDotCount()
    {
        if (_isPreferred)
            _dotCounter++;
    }

    #endregion

    #region Ghost Mode

    /// <summary>
    /// Set the ghost's current mode
    /// </summary>
    private void SetMode()
    {
        if (CurrentMode is GhostStates.Frightened or GhostStates.Dead or GhostStates.InGhostHouse or GhostStates.LeavingGhostHouse) return;
        
        var mode = GetMode ();
        
        // If the new mode is the same as the current mode, just return
        if (mode == CurrentMode) return;
        
        // Changing the mode reverses the direction of the ghost
        Direction = -Direction;
        Debug.Log (LevelTime() + " : Setting " + ThisGhost + " mode from " + CurrentMode + " to " + mode);
        CurrentMode = mode;
    }

    /// <summary>
    /// Get the ghost mode based on the number of seconds since the start of the level
    /// </summary>
    /// <returns>The mode.</returns>
    private GhostStates GetMode()
    {
        int level = GameController.CurrentLevel;
        float levelSeconds = LevelTime ();

        if ((level >= 1 && level <= 4 && levelSeconds <= 7) || (level >= 5 && levelSeconds <= 5))
            return GhostStates.Scatter;
        if ((level >= 1 && level <= 4 && levelSeconds >= 7 && levelSeconds < 27) || (level >= 5 && levelSeconds >= 5 && levelSeconds < 25))
            return GhostStates.Chase;
        if ((level >= 1 && level <= 4 && levelSeconds >= 27 && levelSeconds < 34) || (level >= 5 && levelSeconds >= 25 && levelSeconds < 30))
            return GhostStates.Scatter;
        if ((level >= 1 && level <= 4 && levelSeconds >= 34 && levelSeconds < 54) || (level >=5 && levelSeconds >= 30 && levelSeconds < 50))
            return GhostStates.Chase;
        if ((level >= 1 && level <= 4 && levelSeconds >= 54 && levelSeconds < 59) || (level >= 5 && levelSeconds >= 50 && levelSeconds < 55))
            return GhostStates.Scatter;
        if ((level == 1 && levelSeconds >= 59 && levelSeconds < 79) || (level >= 2 && level <= 4 && levelSeconds >= 59 && levelSeconds < 1092) || (level >= 5 && levelSeconds >= 55 && levelSeconds < 1092))
            return GhostStates.Chase;
        if ((level == 1 && levelSeconds >= 79 && levelSeconds < 84) || (level >= 2 && level <= 4 && levelSeconds >= 1092 && levelSeconds < 1092.01666666f) || (level >=5 && levelSeconds >= 1092 && levelSeconds < 1092.01666666f))
            return GhostStates.Scatter;

        return GhostStates.Chase;
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="GhostMove"/> is frightened.
    /// </summary>
    /// <value><c>true</c> if frightened; otherwise, <c>false</c>.</value>
    public bool Frightened
    {
        get => CurrentMode == GhostStates.Frightened;
        set {
            if (value) {
                if (CurrentMode != GhostStates.Dead) {
                    CurrentMode = GhostStates.Frightened;
                    Direction = -Direction;
                }
            } else
            {
                CurrentMode = GetMode();
                CurrentAnimation = GhostAnimations.AnimNormal;
                IsBlinking = false;
                CurrentBlink = 0;
                GameController.GhostsEaten = 0;
            }
        }
    }

    /// <summary>
    /// Kick off the coroutine to blink the ghosts when they're frightened
    /// </summary>
    public void DoBlinkGhost()
    {
        if (CurrentMode == GhostStates.Dead)
            return;
        
        IsBlinking = true;
        CurrentBlink = 0;

        StartCoroutine (BlinkGhost());
    }

    /// <summary>
    /// If the ghost is frightened, it will start to blink after a certain number of seconds.
    /// </summary>
    private IEnumerator BlinkGhost()
    {        
        int totalBlinks = TableOfValues.GhostFrightenedFlashes (GameController.CurrentLevel);
        while (CurrentBlink <= totalBlinks && CurrentMode != GhostStates.Dead) {
            _fsmAnimation.ChangeState(GhostAnimations.AnimSemiFrightened);
            yield return new WaitForSeconds (.25f);
            if (CurrentMode == GhostStates.Dead)
                continue;
            _fsmAnimation.ChangeState(GhostAnimations.AnimFrightened);
            yield return new WaitForSeconds (.25f);
            CurrentBlink++;
        }

        if(CurrentMode != GhostStates.Dead)
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
        if (CurrentMode == GhostStates.Dead) {
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
        if (CurrentMode == GhostStates.Frightened) {
            Speed = TableOfValues.GhostFrightenedSpeed (GameController.CurrentLevel) * TableOfValues.Speed ();
            return;
        }

        // Ghost is in the ghost house. Cut the speed in half
        if (CurrentMode == GhostStates.InGhostHouse) {
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
        var canGoUp = !(Maze.SpecialLocations().Contains (Tile) && CurrentMode is GhostStates.Chase or GhostStates.Scatter);

        return (from dir in _directions 
            where dir != Vector2.up || canGoUp 
            where dir != -Direction 
            let dest = Tile + dir 
            where Maze.ValidLocations().Contains(dest) 
            select dir)
            .ToList();
    }

    /// <summary>
    /// Figure out where the ghost should be going
    /// </summary>
    private void GetGhostDestination()
    {
        // This is for ghosts that have left the ghost house
        if (CurrentMode == GhostStates.InGhostHouse || TileCenter != Destination) return;
        
        // Based on the ghost's location, figure out which directions he can go based on our mode.
        var exits = GetExits ();
        SetDestination(exits.Count == 1 ? exits[0] : GetDirection(exits));
    }

    /// <summary>
    /// Check to see if Blinky can enter Cruise Elroy mode
    /// </summary>
    private void CruiseElroy()
    {
        // Check for Cruise Elroy 1
        if (ThisGhost == Ghost.Blinky && GameController.SmallDotsLeft == TableOfValues.CruiseElroy1DotsLeft(GameController.CurrentLevel) && !_inCruiseElroy) {
            Speed = TableOfValues.CruiseElroy1Speed (GameController.CurrentLevel) * TableOfValues.Speed ();
            _inCruiseElroy = true;
        }

        // Check for Cruise Elroy 2
        if (ThisGhost == Ghost.Blinky && GameController.SmallDotsLeft == TableOfValues.CruiseElroy2DotsLeft (GameController.CurrentLevel)) {
            Speed = TableOfValues.CruiseElroy2Speed (GameController.CurrentLevel) * TableOfValues.Speed ();
            _inCruiseElroy = true;
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
        if (CurrentMode == GhostStates.Dead) {
            return _home;
        }

        switch (CurrentMode) {
        case GhostStates.Scatter:
            return ProcessScatter ();
        case GhostStates.Chase:
            switch (ThisGhost) {
            case Ghost.Blinky:
                return BlinkyTarget ();
            case Ghost.Pinky:
                return PinkyTarget ();
            case Ghost.Inky:
                return InkyTarget ();
            case Ghost.Clyde:
                return ClydeTarget ();
            default:
                return Vector2.zero;
            }
        case GhostStates.Frightened:
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
        if (ThisGhost == Ghost.Blinky && _inCruiseElroy) {
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
        return exits.Count switch
        {
            0 => TileCenter - Direction,
            1 => TileCenter + exits[0],
            _ => TileCenter + exits[Random.Range(0, exits.Count - 1)]
        };
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
    
    #endregion
}
