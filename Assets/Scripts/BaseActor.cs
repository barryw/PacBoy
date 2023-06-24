using UnityEngine;

/// <inheritdoc />
/// <summary>
/// Base Actor class that all characters share. It handles things
/// like movement, animation, speed, etc
/// </summary>
public class BaseActor : MonoBehaviour
{
    protected Animator Anim;
    private Vector2 _savePosition = Vector2.zero;
    protected AudioController Audio;

    protected void Start()
    {
        var gc = GameObject.FindGameObjectWithTag ("GameController");
        if (gc != null)
            GameController = gc.GetComponent<GameController> ();
        Anim = GetComponent<Animator> ();
        Audio = AudioController.Instance;
    }

    /// <summary>
    /// Get or set the location of the actor's rigid body
    /// </summary>
    /// <value>The location.</value>
    public Vector2 Location
    {
        set => GetComponent<Rigidbody2D> ().transform.position = value;
    }

    /// <summary>
    /// Return the integer tile of the actor's current location
    /// </summary>
    /// <value>The tile.</value>
    public Vector2 Tile => new Vector2 (Mathf.Ceil (transform.position.x), Mathf.Ceil (transform.position.y));

    /// <summary>
    /// Return the coordinates of the center of the tile of the actor's current location
    /// </summary>
    /// <value>The tile center.</value>
    public Vector2 TileCenter => new Vector2 (Mathf.Ceil (transform.position.x) - 0.5f, Mathf.Ceil (transform.position.y) - 0.5f);

    /// <summary>
    /// Set the destination tile
    /// </summary>
    /// <param name="direction">The direction of travel as a Vector2</param>
    protected void SetDestination(Vector2 direction)
    {
        if (Valid (direction)) {
            Destination = TileCenter + direction;
            Direction = Destination - TileCenter;
            Animation = true;
        } else {
            Animation = false;
        }
    }

    /// <summary>
    /// Determine whether the direction from the current tile is a valid location
    /// </summary>
    /// <param name="direction">The direction of travel as a Vector2</param>
    protected bool Valid(Vector2 direction)
    {
        return Maze.ValidLocations ().Contains (Tile + direction);
    }

    /// <summary>
    /// Move the actor toward a position represented as a Vector2
    /// </summary>
    protected void Move()
    {
        var p = Vector2.MoveTowards(transform.position, Destination, Speed * Time.deltaTime);
        GetComponent<Rigidbody2D>().MovePosition(p);
        HandleTunnel ();
    }

    /// <summary>
    /// If the actor enters the tunnel, transport them to the other side
    /// </summary>
    private void HandleTunnel()
    {
        if (Tile == Maze.LeftTunnel) {
            transform.position = new Vector2 (Maze.RightTunnel.x - 0.5f, Maze.RightTunnel.y - 0.5f) + Vector2.left;
            Destination = (Vector2)transform.position + Vector2.left;
        }

        if (Tile != Maze.RightTunnel) return;
        transform.position = new Vector2 (Maze.LeftTunnel.x - 0.5f, Maze.LeftTunnel.y - 0.5f) + Vector2.right;
        Destination = (Vector2)transform.position + Vector2.right;
    }

    protected bool InTunnel => (Mathf.Approximately(Tile.y, Maze.LeftTunnel.y) && ((Tile.x >= -1 && Tile.x <= 5) || (Tile.x >= 24 && Tile.x <= 32)));

    /// <summary>
    /// Animate the actor
    /// </summary>
    protected void Animate()
    {
        Animator.SetFloat("DirX", Direction.x);
        Animator.SetFloat("DirY", Direction.y);
    }

    /// <summary>
    /// Reset the actor's animation
    /// </summary>
    public void ResetAnimation()
    {
        Animator.Play ("", 0, 0.0f);
    }

    /// <summary>
    /// Gets the game controller.
    /// </summary>
    /// <value>The game controller.</value>
    protected GameController GameController { get; private set; }

    /// <summary>
    /// Gets the animator.
    /// </summary>
    /// <value>The animator.</value>
    protected Animator Animator => Anim;

    /// <summary>
    /// Set the actor's speed
    /// </summary>
    /// <value>The speed.</value>
    public float Speed { private get; set; }

    /// <summary>
    /// Set the actor's destination tile
    /// </summary>
    /// <value>The destination.</value>
    public Vector2 Destination { protected get; set; } = Vector2.zero;

    /// <summary>
    /// Set the direction that the actor is moving in
    /// </summary>
    /// <value>The direction.</value>
    public Vector2 Direction { get; set; } = Vector2.zero;

    /// <summary>
    /// Gets or sets the animation speed.
    /// </summary>
    /// <value>The animation speed.</value>
    public float AnimationSpeed { private get; set; } = 0.8f;

    /// <summary>
    /// Sets a value indicating whether this <see cref="BaseActor"/> is animating.
    /// </summary>
    /// <value><c>true</c> if animation; otherwise, <c>false</c>.</value>
    public bool Animation {
        set {
            if (value) {
                Animator.speed = AnimationSpeed;
            } else {
                Animator.speed = 0.0f;
                Animator.Play ("", 0, 0.0f);
            }
        }
    }

    /// <summary>
    /// Hide and unhide the actor
    /// </summary>
    /// <value><c>true</c> if hidden; otherwise, <c>false</c>.</value>
    public bool Hidden
    {
        set {
            if (value) {
                _savePosition = transform.position;
                transform.position = Maze.HiddenPosition;
            } else {
                transform.position = _savePosition;
                _savePosition = Vector2.zero;
            }
        }
    }
}
