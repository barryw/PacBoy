using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMove : BaseActor {
    public float speed = 0.3f;
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
    private List<Vector2> NormalIntersections = new List<Vector2> ();
    private List<Vector2> SpecialIntersections = new List<Vector2> ();
    private List<Vector2> Directions = new List<Vector2> ();
    private Vector2 Waypoint = Vector2.zero;

    new void Start()
    {
        base.Start ();

        // These are the locations of the scatter targets for each ghost
        ScatterTargets.Add (Ghost.BLINKY, new Vector2 (26, 35));
        ScatterTargets.Add (Ghost.PINKY, new Vector2 (2, 35));
        ScatterTargets.Add (Ghost.INKY, new Vector2 (26, 2));
        ScatterTargets.Add (Ghost.CLYDE, new Vector2 (2, 2));

        // There are 4 special intersections that ghosts cannot turn "up" at
        SpecialIntersections.Add (new Vector2 (16, 10));
        SpecialIntersections.Add (new Vector2 (13, 10));
        SpecialIntersections.Add (new Vector2 (16, 22));
        SpecialIntersections.Add (new Vector2 (13, 22));

        // And a bunch of normal intersections
        NormalIntersections.Add (new Vector2 (16, 4));
        NormalIntersections.Add (new Vector2 (13, 4));
        NormalIntersections.Add (new Vector2 (4, 7));
        NormalIntersections.Add (new Vector2 (25, 7));
        NormalIntersections.Add (new Vector2 (7, 10));
        NormalIntersections.Add (new Vector2 (10, 10));
        NormalIntersections.Add (new Vector2 (19, 10));
        NormalIntersections.Add (new Vector2 (22, 10));
        NormalIntersections.Add (new Vector2 (7, 13));
        NormalIntersections.Add (new Vector2 (10, 13));
        NormalIntersections.Add (new Vector2 (19, 13));
        NormalIntersections.Add (new Vector2 (22, 13));
        NormalIntersections.Add (new Vector2 (10, 16));
        NormalIntersections.Add (new Vector2 (19, 16));
        NormalIntersections.Add (new Vector2 (7, 19));
        NormalIntersections.Add (new Vector2 (10, 19));
        NormalIntersections.Add (new Vector2 (19, 19));
        NormalIntersections.Add (new Vector2 (22, 19));
        NormalIntersections.Add (new Vector2 (7, 25));
        NormalIntersections.Add (new Vector2 (22, 25));
        NormalIntersections.Add (new Vector2 (2, 28));
        NormalIntersections.Add (new Vector2 (7, 28));
        NormalIntersections.Add (new Vector2 (10, 28));
        NormalIntersections.Add (new Vector2 (13, 28));
        NormalIntersections.Add (new Vector2 (16, 28));
        NormalIntersections.Add (new Vector2 (19, 28));
        NormalIntersections.Add (new Vector2 (22, 28));
        NormalIntersections.Add (new Vector2 (27, 28));
        NormalIntersections.Add (new Vector2 (7, 32));
        NormalIntersections.Add (new Vector2 (22, 32));

        Directions.Add (Vector2.up);
        Directions.Add (Vector2.down);
        Directions.Add (Vector2.left);
        Directions.Add (Vector2.right);

        Waypoint = Tile + Vector2.left;
    }

	// Update is called once per frame
	void FixedUpdate () {
        List<Vector2> exits = GetExits(Waypoint);

        if (NormalIntersections.Contains (Tile)) {
            // At an intersection. Make a choice
            Vector2 direction = GetDirection (exits);

        }
        if (SpecialIntersections.Contains (Tile)) {
            // Cannot move up in chase or scatter
            if (CurrentMode == Mode.CHASE || CurrentMode == Mode.SCATTER) {
            }
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
        List<Vector2> exits = new List<Vector2> ();

        foreach (Vector2 direction in Directions) {
            if (Tile != (pos + direction)) {
                Vector2 dest = pos + direction;
                RaycastHit2D hit = Physics2D.Linecast (pos, dest, LayerMask.GetMask ("Maze"));
                if (hit.collider == null) {
                    exits.Add (direction);
                    Debug.DrawLine (pos, pos + direction, Color.red);
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

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
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
