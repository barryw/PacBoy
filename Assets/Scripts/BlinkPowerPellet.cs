using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkPowerPellet : MonoBehaviour {
    public float flashTime = 1.0f;

	// Use this for initialization
	void Start () {
        StartCoroutine (Blink ());
	}
	
	// Update is called once per frame
	void Update () {
	}

    IEnumerator Blink()
    {
        while (true) {
            yield return new WaitForSeconds(flashTime);    
            GetComponent<SpriteRenderer> ().enabled = !GetComponent<SpriteRenderer> ().enabled;
        }
    }
}
