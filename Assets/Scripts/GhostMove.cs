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

    new void Start()
    {
        base.Start ();

        // These are the locations of the scatter targets for each ghost
        ScatterTargets.Add (Ghost.BLINKY, new Vector2 (26, 35));
        ScatterTargets.Add (Ghost.PINKY, new Vector2 (2, 35));
        ScatterTargets.Add (Ghost.INKY, new Vector2 (26, 2));
        ScatterTargets.Add (Ghost.CLYDE, new Vector2 (2, 2));

        Directions.Add (Vector2.up);
        Directions.Add (Vector2.down);
        Directions.Add (Vector2.left);
        Directions.Add (Vector2.right);

        // Start off moving left
        SetDestination (Vector2.left);
        Direction = Vector2.left;
        Speed = 6.0f;
    }

	// Update is called once per frame
	void FixedUpdate () {
        Move ();
        Animate ();
           
        if (TileCenter == Destination) {
            List<Vector2> exits = GetExits (Tile);
            if (exits.Count == 1)
                SetDestination (exits [0]);
        }

//        if (waypoints != null && waypoints.Length > 0) {
//            if (transform.position != waypoints[cur].position) {
//                Vector2 p = Vector2.MoveTowards(transform.position,
//                    waypoints[cur].position,
//                    speed);
//                GetComponent<Rigidbody2D>().MovePosition(p);
//            }
//            // Waypoint reached, select next one
//            else cur = (cur + 1) % waypoints.Length;
//
//            // Animation
//            Vector2 dir = waypoints[cur].position - transform.position;
//            GetComponent<Animator>().SetFloat("DirX", dir.x);
//            GetComponent<Animator>().SetFloat("DirY", dir.y);
        //}
	}


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

    private Vector2 GetDirection(List<Vector2> exits)
    {
        float shortestDistance = float.PositiveInfinity;
        Vector2 shortestVector = Vector2.zero;
        foreach (Vector2 exit in exits) {
            float distance = Vector2.Distance (exit, Target ());
            if (distance < shortestDistance) {
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

    private Vector2 BlinkyTarget()
    {
        return PacMan.transform.position;
    }

    private Vector2 PinkyTarget()
    {
        return Vector2.zero;
    }

    private Vector2 InkyTarget()
    {
        return Vector2.zero;
    }

    private Vector2 ClydeTarget()
    {
        return Vector2.zero;
    }
}
