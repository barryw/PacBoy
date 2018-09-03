using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreBoard : MonoBehaviour {
    [FormerlySerializedAs("zero")] public GameObject Zero;
    [FormerlySerializedAs("one")] public GameObject One;
    [FormerlySerializedAs("two")] public GameObject Two;
    [FormerlySerializedAs("three")] public GameObject Three;
    [FormerlySerializedAs("four")] public GameObject Four;
    [FormerlySerializedAs("five")] public GameObject Five;
    [FormerlySerializedAs("six")] public GameObject Six;
    [FormerlySerializedAs("seven")] public GameObject Seven;
    [FormerlySerializedAs("eight")] public GameObject Eight;
    [FormerlySerializedAs("nine")] public GameObject Nine;

    private int _highScore = 0;
    private readonly Dictionary<string, List<GameObject>> _scores = new Dictionary<string, List<GameObject>> ();

    public ScoreBoard()
    {
        Player1Score = 0;
        Player2Score = 0;
    }

    public static ScoreBoard Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy (this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad (this.gameObject);
    }

	// Use this for initialization
    private void Start () {
        StartCoroutine (FlashCurrentPlayer ());
	}

    public int CurrentPlayer { private get; set; } = 1;

    public int Player1Score { get; private set; }

    public int Player2Score { get; private set; }

    public void AddPoints(int points)
    {
        if (CurrentPlayer == 1) {
            Player1Score += points;
        } else {
            Player2Score += points;
        }
        if (Player1Score > _highScore)
            _highScore = Player1Score;
        if (Player2Score > _highScore)
            _highScore = Player2Score;

        DisplayScore ();
    }

    private void DisplayScore()
    {
        DisplayPlayerScore (Player1Score, 6.25f, "1up");
        DisplayPlayerScore (Player2Score, 25.25f, "2up");
    }

    /// <summary>
    /// Flash the 1UP or 2UP
    /// </summary>
    private IEnumerator FlashCurrentPlayer()
    {
        var oneUp = GameObject.FindGameObjectWithTag ("1UP");
        var twoUp = GameObject.FindGameObjectWithTag ("2UP");
        var display = true;

        while (true) {
            yield return new WaitForSecondsRealtime (0.5f);
            display = !display;
            if (CurrentPlayer == 1) {
                oneUp.SetActive (!oneUp.activeSelf);
            } else {
                twoUp.SetActive (!twoUp.activeSelf);
            }
        }
    }
    
    /// <summary>
    /// Display the player's score
    /// </summary>
    /// <param name="score">The player's score</param>
    /// <param name="startLoc">The location to display it</param>
    /// <param name="key"></param>
    private void DisplayPlayerScore(int score, float startLoc, string key)
    {
        if (!_scores.ContainsKey (key))
            _scores [key] = new List<GameObject> ();

        foreach (var digit in _scores[key])
            Destroy (digit);

        var stringScore = score.ToString ();
        if (score == 0)
            stringScore = "00";
        var pos = 0;
        foreach (var c in Reverse(stringScore)) {
            GameObject prefab = null;
            switch (c) {
            case '0':
                prefab = Zero;
                break;
            case '1':
                prefab = One;
                break;
            case '2':
                prefab = Two;
                break;
            case '3':
                prefab = Three;
                break;
            case '4':
                prefab = Four;
                break;
            case '5':
                prefab = Five;
                break;
            case '6':
                prefab = Six;
                break;
            case '7':
                prefab = Seven;
                break;
            case '8':
                prefab = Eight;
                break;
            case '9':
                prefab = Nine;
                break;
            }
            _scores[key].Add(Instantiate (prefab, new Vector2 (startLoc - pos, 34.5f), Quaternion.identity));
            pos++;
        }
    }

    /// <summary>
    /// Reverse a string. Really? The String class doesn't have a fucking .Reverse method?!
    /// </summary>
    /// <param name="original">Original.</param>
    private static string Reverse(string original)
    {
        var chars = original.ToCharArray ();
        Array.Reverse (chars);
        return new string (chars);
    }
}
