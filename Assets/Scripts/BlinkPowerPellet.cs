using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class BlinkPowerPellet : MonoBehaviour {
    [FormerlySerializedAs("flashTime")] public float FlashTime = 0.12f;
    private GameController _gameController;

	// Use this for initialization
    private void Start () {
        var gc = GameObject.FindGameObjectWithTag ("GameController");
        if (gc != null)
            _gameController = gc.GetComponent<GameController> ();
        
        StartCoroutine (Blink ());
	}

    private IEnumerator Blink()
    {
        while (true) {
            yield return new WaitForSecondsRealtime (FlashTime);
            if(_gameController.IsReady)
                GetComponent<SpriteRenderer> ().enabled = !GetComponent<SpriteRenderer> ().enabled;
        }
    }
}
