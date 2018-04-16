using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkPowerPellet : MonoBehaviour {
    public float flashTime = 0.12f;
    private GameController _gameController;

	// Use this for initialization
	void Start () {
        GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
        if (gc != null)
            _gameController = gc.GetComponent<GameController> ();
        
        StartCoroutine (Blink ());
	}

    IEnumerator Blink()
    {
        while (true) {
            yield return new WaitForSecondsRealtime (flashTime);
            if(_gameController.IsReady)
                GetComponent<SpriteRenderer> ().enabled = !GetComponent<SpriteRenderer> ().enabled;
        }
    }
}
