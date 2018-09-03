using UnityEngine;

public class DrawGrid : MonoBehaviour {

	// Use this for initialization
	private void Start () {
        for (var x = 0; x <= 28; x++) {
            for (var y = 0; y <= 36; y++) {
                var hStart = new Vector3 (0, y);
                var hEnd = new Vector3 (28, y);
                var vStart = new Vector3 (x, 0);
                var vEnd = new Vector3 (x, 36);
                Debug.DrawLine (hStart, hEnd, Color.white, 3600.0f, false);
                Debug.DrawLine (vStart, vEnd, Color.white, 3600.0f, false);
            }
        }
	}
}
