using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGrid : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for (int x = 0; x <= 28; x++) {
            for (int y = 0; y <= 36; y++) {
                Vector3 hStart = new Vector3 (0, y);
                Vector3 hEnd = new Vector3 (28, y);
                Vector3 vStart = new Vector3 (x, 0);
                Vector3 vEnd = new Vector3 (x, 36);
                Debug.DrawLine (hStart, hEnd, Color.white, 3600.0f, false);
                Debug.DrawLine (vStart, vEnd, Color.white, 3600.0f, false);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
