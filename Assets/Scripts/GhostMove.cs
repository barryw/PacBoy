using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMove : MonoBehaviour {
    public Transform[] waypoints;
    int cur = 0;
    public float speed = 0.3f;

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
}
