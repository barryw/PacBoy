using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMove : MonoBehaviour {
    public Transform[] waypoints;
    int cur = 0;
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
    public Mode CurrentMode;

    private Dictionary<Ghost, Vector2> ScatterTargets = new Dictionary<Ghost, Vector2>();
    private List<Vector2> NormalIntersections = new List<Vector2> ();
    private List<Vector2> SpecialIntersections = new List<Vector2> ();

    void Start()
    {
        ScatterTargets.Add (Ghost.PINKY, new Vector2 (1.75f, 32.5f));
        ScatterTargets.Add (Ghost.BLINKY, new Vector2 (26.75f, 32.5f));
        ScatterTargets.Add (Ghost.INKY, new Vector2 (26.75f, -1.5f));
        ScatterTargets.Add (Ghost.CLYDE, new Vector2 (1.75f, -1.5f));

        SpecialIntersections.Add (new Vector2 (16.0f, 10.0f));
        SpecialIntersections.Add (new Vector2 (13.0f, 10.0f));

        NormalIntersections.Add (new Vector2 (16.0f, 4.0f));
        NormalIntersections.Add (new Vector2 (13.0f, 4.0f));
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (waypoints != null && waypoints.Length > 0) {
            if (transform.position != waypoints[cur].position) {
                Vector2 p = Vector2.MoveTowards(transform.position,
                    waypoints[cur].position,
                    speed);
                GetComponent<Rigidbody2D>().MovePosition(p);
            }
            // Waypoint reached, select next one
            else cur = (cur + 1) % waypoints.Length;

            // Animation
            Vector2 dir = waypoints[cur].position - transform.position;
            GetComponent<Animator>().SetFloat("DirX", dir.x);
            GetComponent<Animator>().SetFloat("DirY", dir.y);
        }
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
