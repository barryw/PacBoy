using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour {
    public GameObject zero;
    public GameObject one;
    public GameObject two;
    public GameObject three;
    public GameObject four;
    public GameObject five;
    public GameObject six;
    public GameObject seven;
    public GameObject eight;
    public GameObject nine;

    private int currentPlayer = 1;
    private int player1Score = 0;
    private int player2Score = 0;
    private int highScore = 0;
    private Dictionary<string, List<GameObject>> scores = new Dictionary<string, List<GameObject>> ();

    private static ScoreBoard _instance;

    public static ScoreBoard Instance
    {
        get {
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this) {
            Destroy (this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad (this.gameObject);
    }

	// Use this for initialization
	void Start () {
        StartCoroutine (FlashCurrentPlayer ());
	}
	
	// Update is called once per frame
	void Update () {
        DisplayScore ();
	}

    public int CurrentPlayer
    {
        get {
            return currentPlayer;
        }
        set{
            currentPlayer = value;
        }
    }

    public int Player1Score
    {
        get {
            return player1Score;
        }
    }

    public int Player2Score
    {
        get {
            return player2Score;
        }
    }

    public void AddPoints(int points)
    {
        if (currentPlayer == 1) {
            player1Score += points;
        } else {
            player2Score += points;
        }
        if (player1Score > highScore)
            highScore = player1Score;
        if (player2Score > highScore)
            highScore = player2Score;
    }

    private void DisplayScore()
    {
        DisplayPlayerScore (player1Score, 6.25f, "1up");
        DisplayPlayerScore (player2Score, 25.25f, "2up");
    }

    /// <summary>
    /// Flash the 1UP or 2UP
    /// </summary>
    private IEnumerator FlashCurrentPlayer()
    {
        GameObject oneUp = GameObject.FindGameObjectWithTag ("1UP");
        GameObject twoUp = GameObject.FindGameObjectWithTag ("2UP");
        bool display = true;

        while (true) {
            yield return new WaitForSecondsRealtime (0.5f);
            display = !display;
            if (currentPlayer == 1) {
                oneUp.SetActive (!oneUp.activeSelf);
            } else {
                twoUp.SetActive (!twoUp.activeSelf);
            }
        }
    }

    void DisplayPlayerScore(int score, float startLoc, string key)
    {
        if (!scores.ContainsKey (key))
            scores [key] = new List<GameObject> ();

        foreach (GameObject digit in scores[key])
            Destroy (digit);

        string stringScore = score.ToString ();
        if (score == 0)
            stringScore = "00";
        int pos = 0;
        foreach (char c in Reverse(stringScore)) {
            GameObject prefab = null;
            switch (c) {
            case '0':
                prefab = zero;
                break;
            case '1':
                prefab = one;
                break;
            case '2':
                prefab = two;
                break;
            case '3':
                prefab = three;
                break;
            case '4':
                prefab = four;
                break;
            case '5':
                prefab = five;
                break;
            case '6':
                prefab = six;
                break;
            case '7':
                prefab = seven;
                break;
            case '8':
                prefab = eight;
                break;
            case '9':
                prefab = nine;
                break;
            }
            scores[key].Add(Instantiate (prefab, new Vector2 (startLoc - pos, 34.5f), Quaternion.identity));
            pos++;
        }
    }

    /// <summary>
    /// Reverse a string. Really? The String class doesn't have a fucking .Reverse method?!
    /// </summary>
    /// <param name="original">Original.</param>
    private string Reverse(string original)
    {
        char[] chars = original.ToCharArray ();
        Array.Reverse (chars);
        return new string (chars);
    }
}
