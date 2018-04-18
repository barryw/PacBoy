using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Color BLINKY_COLOR = new Color (.81f, .24f, .098f);
    private Color PINKY_COLOR = new Color (.917f, .509f, .898f);
    private Color INKY_COLOR = new Color (.274f, .749f, .933f);
    private Color CLYDE_COLOR = new Color (.858f, .521f, .109f);

    public Ghost ThisGhost;
    public Mode CurrentMode = Mode.CHASE;

    private Vector2 ScatterTarget = Vector2.zero;
    private List<Vector2> Directions = new List<Vector2> ();
    private TableOfValues _tov = TableOfValues.Instance();
    private GhostMove BlinkyMover;
    private PacManMove PacManMover;

    private bool InCruiseElroy = false;

    private bool InGhostHouse = false;
    private bool LeavingGhostHouse = false;
    private int DotCounter;
    private int DotsToLeave;
    private bool IsPreferred = false;

    public bool IsEaten = false;
    public bool IsBlinking = false;
    public int CurrentBlink = 0;

    private float playStartTime = 0.0f;
    private Vector2 Home = new Vector2 (14, 22);
    private Vector2 GhostHome = Vector2.zero;

    #region Start

    new void Start()
    {
        base.Start ();

        switch (ThisGhost) {
        case Ghost.BLINKY:
            ScatterTarget = new Vector2 (28, 36);
            GhostHome = new Vector2 (14, 19);
            break;
        case Ghost.INKY:
            ScatterTarget = new Vector2 (28, 1);
            GhostHome = new Vector2 (12, 19);
            break;
        case Ghost.PINKY:
            ScatterTarget = new Vector2 (3, 36);
            GhostHome = new Vector2 (14, 19);
            break;
        case Ghost.CLYDE:
            ScatterTarget = new Vector2 (1, 1);
            GhostHome = new Vector2 (16, 19);
            break;
        }

        Directions.Add (Vector2.up);
        Directions.Add (Vector2.down);
        Directions.Add (Vector2.left);
        Directions.Add (Vector2.right);

        // Start off moving left
        SetDestination (Vector2.left);
        Direction = Vector2.left;
        Animation = true;

        // Everybody but Blinky is in the ghost house
        if (ThisGhost != Ghost.BLINKY) {
            InGhostHouse = true;
        }

        SetDotsToLeave ();

        PacManMover = PacMan.GetComponent<PacManMove> ();
        if (ThisGhost == Ghost.INKY) {
            Debug.Log ("Linking Blinky's mover to Inky's");
            GameObject blinky = GameObject.FindGameObjectWithTag ("Blinky");
            BlinkyMover = blinky.GetComponent<GhostMove> ();
        }
    }

    #endregion
        
    #region FixedUpdate

	void FixedUpdate () {
        if (GameController.IsReady) {
            if (playStartTime == 0.0f)
                playStartTime = Time.fixedTime;
            
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

    void CheckCollision()
    {
        if (PacManMover.Tile == Tile && CurrentMode == Mode.FRIGHTENED && !IsEaten) {
            Debug.Log (ThisGhost + " has been eaten");
            StartCoroutine (EatGhost ());
        }
    }

    IEnumerator EatGhost()
    {
        Vector2 position = transform.position;

        PacManMover.Hidden = true;
        Hidden = true;
        IsEaten = true;
        IsBlinking = false;

        GameObject points = null;
        switch (GameController.GhostsEaten) {
        case 0:
            points = Instantiate (Ghost200);
            GameController.AddPoints (GameController.PointSource.FIRST_GHOST);
            break;
        case 1:
            points = Instantiate (Ghost400);
            GameController.AddPoints (GameController.PointSource.SECOND_GHOST);
            break;
        case 2:
            points = Instantiate (Ghost800);
            GameController.AddPoints (GameController.PointSource.THIRD_GHOST);
            break;
        case 3:
            points = Instantiate (Ghost1600);
            GameController.AddPoints (GameController.PointSource.FOURTH_GHOST);
            break;
        }

        GameController.GhostsEaten++;
        if (GameController.GhostsEaten == 4)
            GameController.GhostsEaten = 0;
        
        points.transform.position = position;

        GameController.IsReady = false;
        _audio.PlayEatGhost ();

        Destroy (points, _audio.EatGhostLength);
        yield return new WaitForSecondsRealtime (_audio.EatGhostLength);

        GameController.IsReady = true;
        SetAnimation (Animations.EYES);

        _audio.BlueGhostsPlaying = false;
        _audio.GhostEyesPlaying = true;
        _audio.SirenPlaying = false;

        Hidden = false;
        PacManMover.Hidden = false;
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
        return Time.fixedTime - playStartTime;
    }
       
    #region Ghost House Rules

    /// <summary>
    /// Set the number of dots PacMan needs to have eaten for ghosts to leave the ghost house
    /// </summary>
    void SetDotsToLeave()
    {
        if (GameController.CurrentLevel == 1) {
            if (ThisGhost == Ghost.PINKY)
                DotsToLeave = 0;
            if (ThisGhost == Ghost.INKY)
                DotsToLeave = 30;
            if (ThisGhost == Ghost.CLYDE)
                DotsToLeave = 60;
        }

        if (GameController.CurrentLevel == 2) {
            if (ThisGhost == Ghost.PINKY || ThisGhost == Ghost.INKY)
                DotsToLeave = 0;
            if (ThisGhost == Ghost.CLYDE)
                DotsToLeave = 50;
        }
    }

    /// <summary>
    /// Once our eyes hit the home location, put them back in the ghost house and reincarnate the ghost
    /// </summary>
    void PutGhostBackInHouse()
    {
        if (IsEaten && Tile == Home){
            Destination = GhostHome;
        }

        if (IsEaten && Tile == GhostHome) {
            _audio.GhostEyesPlaying = false;
            _audio.BlueGhostsPlaying = true;
            SetAnimation (Animations.NORMAL);
            DotsToLeave = 0;
            DotCounter = 0;
            InGhostHouse = true;
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
        if(DotCounter >= DotsToLeave && InGhostHouse) {
            LeavingGhostHouse = true;

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
                InGhostHouse = false;
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
        IsPreferred = (ThisGhost == Ghost.PINKY && InGhostHouse) || (ThisGhost == Ghost.INKY && InGhostHouse) || (ThisGhost == Ghost.CLYDE && InGhostHouse);
    }

    /// <summary>
    /// Increase the dot count for the preferred ghost
    /// </summary>
    public void IncreaseDotCount()
    {
        if (IsPreferred)
            DotCounter++;
    }

    /// <summary>
    /// Bounce the ghost around in the ghost house
    /// </summary>
    private void BounceInGhostHouse()
    {
        if (!InGhostHouse || LeavingGhostHouse)
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
        if (CurrentMode != Mode.FRIGHTENED) {
            Mode mode = GetMode ();
            if (mode != CurrentMode && !InGhostHouse) {
                Direction = -Direction;
                Debug.Log (LevelTime() + " : Setting " + ThisGhost + " mode from " + CurrentMode + " to " + mode);
                CurrentMode = mode;
            }
        }
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
        int totalBlinks = _tov.GhostFrightenedFlashes (GameController.CurrentLevel);
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
            Speed = _tov.Speed () * 2.0f;
            return;
        }

        // Ghost is frightened
        if (CurrentMode == Mode.FRIGHTENED) {
            Speed = _tov.GhostFrightenedSpeed (GameController.CurrentLevel) * _tov.Speed ();
            return;
        }

        // Ghost is in the ghost house. Cut the speed in half
        if (InGhostHouse) {
            Speed = _tov.Speed () * 0.5f;
            return;
        }

        // Ghost is in the tunnel
        if(Tile.y == 19 && ((Tile.x >= -1 && Tile.x <= 5) || (Tile.x >= 24 && Tile.x <= 32)))
        {
            Speed = _tov.GhostTunnelSpeed (GameController.CurrentLevel) * _tov.Speed ();
            Debug.Log (ThisGhost + " is in the tunnel. Setting speed to " + Speed);
            return;
        }

        // Set normal speed for this level
        if (!InCruiseElroy) {
            Speed = _tov.GhostSpeed (GameController.CurrentLevel) * _tov.Speed ();   
            return;
        }         

        // Check whether Blinky is in Cruise Elroy mode
        CruiseElroy ();
    }

    /// <summary>
    /// Find the right direction to go based on mode
    /// </summary>
    /// <returns>The direction.</returns>
    /// <param name="exits">Exits.</param>
    private Vector2 GetDirection(List<Vector2> exits)
    {
        float shortestDistance = float.PositiveInfinity;
        Vector2 shortestVector = Vector2.zero;
        foreach (Vector2 exit in exits) {
            float distance = Vector2.Distance (TileCenter + exit, Target ());
            //if (distance < shortestDistance || Random.Range(1,10) == 5) {
            if (distance < shortestDistance) {
                shortestDistance = distance;
                shortestVector = exit;
            }
        }
        return shortestVector;
    }

    /// <summary>
    /// Retrieve the available exits from the current location
    /// </summary>
    /// <returns>The exits.</returns>
    private List<Vector2> GetExits()
    {
        bool canGoUp = true;
        if (_maze.SpecialLocations().Contains (Tile) && (CurrentMode == Mode.CHASE || CurrentMode == Mode.SCATTER))
            canGoUp = false;

        List<Vector2> exits = new List<Vector2> ();

        foreach (Vector2 dir in Directions) {
            if (dir == Vector2.up && !canGoUp)
                continue;
            if (dir != -Direction) {
                Vector2 dest = Tile + dir;
                if (_maze.ValidLocations ().Contains (dest)) {
                    exits.Add (dir);
                }
            }
        }

        return exits;
    }

    /// <summary>
    /// Figure out where the ghost should be going
    /// </summary>
    void NavigateGhost()
    {
        // This is for ghosts that have left the ghost house
        if (!InGhostHouse) {
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
    void CruiseElroy()
    {
        // Check for Cruise Elroy 1
        if (ThisGhost == Ghost.BLINKY && GameController.SmallDotsLeft == _tov.CruiseElroy1DotsLeft(GameController.CurrentLevel) && !InCruiseElroy) {
            Speed = _tov.CruiseElroy1Speed (GameController.CurrentLevel) * _tov.Speed ();
            InCruiseElroy = true;
        }

        // Check for Cruise Elroy 2
        if (ThisGhost == Ghost.BLINKY && GameController.SmallDotsLeft == _tov.CruiseElroy2DotsLeft (GameController.CurrentLevel)) {
            Speed = _tov.CruiseElroy2Speed (GameController.CurrentLevel) * _tov.Speed ();
            InCruiseElroy = true;
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
            return Home;
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
        if (ThisGhost == Ghost.BLINKY && InCruiseElroy) {
            return BlinkyTarget ();
        }
        return ScatterTarget;
    }

    /// <summary>
    /// Pick a random exit
    /// </summary>
    /// <returns>The next location</returns>
    private Vector2 ProcessFrightened()
    {
        List<Vector2> exits = GetExits ();
        if (exits.Count == 0)
            return TileCenter - Direction;
        if (exits.Count == 1)
            return TileCenter + exits [0];
        else
            return TileCenter + exits [Random.Range (0, exits.Count - 1)];
    }

    /// <summary>
    /// Blinky targets PacMan
    /// </summary>
    /// <returns>The target.</returns>
    private Vector2 BlinkyTarget()
    {
        return PacManMover.TileCenter;
    }

    /// <summary>
    /// Pinky's target is always 4 tiles ahead of PacMan's current direction
    /// </summary>
    /// <returns>The target.</returns>
    private Vector2 PinkyTarget()
    {
        Vector2 target = Vector2.zero;
        // Simulate overflow bug in original PacMan
        if (PacManMover.Direction == Vector2.up) {
            target = new Vector2 (PacManMover.TileCenter.x - 4, PacManMover.TileCenter.y + 4);
        } else {
            target = PacManMover.TileCenter + (PacManMover.Direction * 4);
        }
        return target;
    }

    /// <summary>
    /// Inky has similar targetting to Pinky, but it also uses Blinky's position in its calculation
    /// </summary>
    /// <returns>The target.</returns>
    private Vector2 InkyTarget()
    {
        Vector2 target = Vector2.zero;
        if (PacManMover.Direction == Vector2.up) {
            target = new Vector2 (PacManMover.TileCenter.x - 2, PacManMover.TileCenter.y + 2);
        } else {
            target = PacManMover.TileCenter + (PacManMover.Direction * 2);
        }

        // Compute vector from blinky's position to target and then double to get Inky's target
        Vector2 blinkysPos = BlinkyMover.TileCenter;
        Vector2 diff =  target - blinkysPos;

        return target + diff;
    }

    /// <summary>
    /// Clyde will target PacMan if he is within 8 tiles of him. Otherwise, he reverts to scatter mode.
    /// </summary>
    /// <returns>The target.</returns>
    private Vector2 ClydeTarget()
    {
        float distance = Vector2.Distance (TileCenter, PacManMover.TileCenter);
        if (distance > 8) {
            return ScatterTarget;
        } else {
            return PacManMover.TileCenter;
        }
    }

    /// <summary>
    /// Draw colored boxes - one for each ghost - showing what that ghost is targeting
    /// </summary>
    private void ShowTarget()
    {
        Vector2 target = Target ();
        Color color = Color.white;
        switch (ThisGhost) {
        case Ghost.BLINKY:
            color = BLINKY_COLOR;
            break;
        case Ghost.CLYDE:
            color = CLYDE_COLOR;
            break;
        case Ghost.PINKY:
            color = PINKY_COLOR;
            break;
        case Ghost.INKY:
            color = INKY_COLOR;
            break;
        default:
            color = Color.white;
            break;
        }

        Debug.DrawLine (new Vector3 (target.x - .5f, target.y - .5f), new Vector3 (target.x + .5f, target.y - .5f), color, 0, false);
        Debug.DrawLine (new Vector3 (target.x - .5f, target.y - .5f), new Vector3 (target.x - .5f, target.y + .5f), color, 0, false);
        Debug.DrawLine (new Vector3 (target.x - .5f, target.y + .5f), new Vector3 (target.x + .5f, target.y + .5f), color, 0, false);
        Debug.DrawLine (new Vector3 (target.x + .5f, target.y - .5f), new Vector3 (target.x + .5f, target.y + .5f), color, 0, false);
    }

    #endregion
}
