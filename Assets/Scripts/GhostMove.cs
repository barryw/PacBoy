using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMove : BaseActor {
    public GameObject PacMan;

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

    public Ghost ThisGhost;
    public Mode CurrentMode = Mode.CHASE;

    private Dictionary<Ghost, Vector2> ScatterTargets = new Dictionary<Ghost, Vector2>();
    private List<Vector2> Directions = new List<Vector2> ();
    private GhostMove BlinkyMover;
    private PacManMove PacManMover;
    private bool InGhostHouse = false;
    private bool LeavingGhostHouse = false;
    private int DotCounter;
    private int DotsToLeave;
    private bool IsPreferred = false;

    public void Frighten()
    {
        CurrentMode = Mode.FRIGHTENED;
        Animator.SetBool ("Frightened", true);
        Direction = -Direction;
    }

    new void Start()
    {
        base.Start ();

        // These are the locations of the scatter targets for each ghost
        ScatterTargets.Add (Ghost.BLINKY, new Vector2 (28, 36));
        ScatterTargets.Add (Ghost.PINKY, new Vector2 (3, 36));
        ScatterTargets.Add (Ghost.INKY, new Vector2 (28, 1));
        ScatterTargets.Add (Ghost.CLYDE, new Vector2 (1, 1));

        Directions.Add (Vector2.up);
        Directions.Add (Vector2.down);
        Directions.Add (Vector2.left);
        Directions.Add (Vector2.right);

        // Start off moving left
        SetDestination (Vector2.left);
        Direction = Vector2.left;
        Speed = 6.0f;

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

	// Update is called once per frame
	void FixedUpdate () {
        if (GameController.IsReady) {
            Animate ();
            SetPreferredGhost ();
            Move ();

            if (DotCounter >= DotsToLeave && InGhostHouse)
                LeaveGhostHouse ();
            
            // This is for ghosts that have left the ghost house
            if (!InGhostHouse) {
                ShowTarget ();
                // The ghost has reached his/her destination. Find the next destination
                if (TileCenter == Destination) {
                    // Based on the ghost's location, figure out which directions he can go based
                    // on our mode.
                    List<Vector2> exits = GetExits (Tile);
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

    /// <summary>
    /// Increase the dot count for the preferred ghost
    /// </summary>
    public void IncreaseDotCount()
    {
        if (IsPreferred)
            DotCounter++;
    }

    /// <summary>
    /// Figure out which ghost is tracking dot count
    /// </summary>
    private void SetPreferredGhost()
    {
        IsPreferred = (ThisGhost == Ghost.PINKY && InGhostHouse) || (ThisGhost == Ghost.INKY && InGhostHouse) || (ThisGhost == Ghost.CLYDE && InGhostHouse);
    }

    /// <summary>
    /// Get the ghost out of the house
    /// </summary>
    public void LeaveGhostHouse()
    {
        if (!InGhostHouse)
            return;

        if(!LeavingGhostHouse)
            Debug.Log (ThisGhost + " is leaving the ghost house");
        
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

    /// <summary>
    /// Retrieve the available exits from the current location
    /// </summary>
    /// <returns>The exits.</returns>
    /// <param name="pos">Position.</param>
    private List<Vector2> GetExits(Vector2 pos)
    {
        bool canGoUp = true;
        if (_maze.SpecialLocations().Contains (pos) && (CurrentMode == Mode.CHASE || CurrentMode == Mode.SCATTER))
            canGoUp = false;
        
        List<Vector2> exits = new List<Vector2> ();

        foreach (Vector2 dir in Directions) {
            if (dir == Vector2.up && !canGoUp)
                continue;
            if (dir != -Direction) {
                Vector2 dest = pos + dir;
                if (_maze.ValidLocations ().Contains (dest)) {
                    exits.Add (dir);
                }
            }
        }

        return exits;
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
            if (distance < shortestDistance || Random.Range(1,10) == 5) {
                shortestDistance = distance;
                shortestVector = exit;
            }
        }
        return shortestVector;
    }

    // Get this ghost's target based on mode
    private Vector2 Target()
    {
        switch (CurrentMode) {
        case Mode.SCATTER:
            return ScatterTargets [ThisGhost];
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

    private Vector2 ProcessFrightened()
    {
        return Vector2.zero;
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
            return ScatterTargets [Ghost.CLYDE];
        } else {
            return PacManMover.TileCenter;
        }
    }

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

    private void ShowTarget()
    {
        Vector2 target = Target ();
        Color color = Color.white;
        switch (ThisGhost) {
        case Ghost.BLINKY:
            color = new Color (.81f, .24f, .098f);
            break;
        case Ghost.CLYDE:
            color = new Color (.858f, .521f, .109f);
            break;
        case Ghost.PINKY:
            color = new Color (.917f, .509f, .898f);
            break;
        case Ghost.INKY:
            color = new Color (.274f, .749f, .933f);
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
}
