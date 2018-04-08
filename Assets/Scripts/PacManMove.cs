using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManMove : MonoBehaviour {
    GameController gameController;

    public float speed = 1.0f;
    Vector2 _dest = Vector2.zero;
    Vector2 _dir = Vector2.right;
    Vector2 _nextDir = Vector2.right;
    Animator _anim;

    bool dying = false;

	// Use this for initialization
	void Start () {
        _dest = transform.position;
        GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
        if (gc != null)
            gameController = gc.GetComponent<GameController> ();
        _anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (dying)
            return;
        
        if (gameController.IsReady) {
            MovePacMan ();
            ReadInput ();
            ReadInputAndMove();
            Animate();
        }
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Ghost" && ! dying) {
            dying = true;
            gameController.StopSiren ();
            gameController.StopGhosts ();
            StartCoroutine (ShowDeathAnimation ());
        }
    }

    IEnumerator ShowDeathAnimation()
    {
        Animation = false;
        yield return new WaitForSeconds (0.75f);
        Animation = true;
        _anim.SetBool ("Died", true);

        yield return new WaitForSeconds (2.0f);
        Destroy (gameObject);
        gameController.Reset ();
    }

    void Animate()
    {
        Vector2 dir = _dest - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    bool Animation {
        set {
            if (value) {
                _anim.speed = 0.8f;
            } else {
                _anim.speed = 0.0f;
                _anim.Play ("", 0, 0.0f);
            }
        }
    }

    bool Valid(Vector2 direction)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos, pos + direction, LayerMask.GetMask("Maze"));
        return hit.collider == null;
    }

    void MovePacMan()
    {
        Vector2 p = Vector2.MoveTowards(transform.position, _dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);
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
            // Is the current direction valid?
            if (Valid (_dir)) {
                SetDestination (_dir);
                Animation = true;
            } else {
                Animation = false;
            }
        } else {
            if (Valid (_nextDir)) {
                _dir = _nextDir;
                SetDestination (_nextDir);
                Animation = true;
            } else {
                if (Valid (_dir)) {
                    // Keep on truckin' good buddy!
                    SetDestination(_dir);
                    Animation = true;
                } else {
                    Animation = false;
                }
            }
        }
    }

    void SetDestination(Vector2 direction)
    {
        _dest = (Vector2)transform.position + (direction * 0.3f);
    }
}
