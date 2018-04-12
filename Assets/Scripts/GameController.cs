using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    public GameObject ready;
    public GameObject gameOver;
    public GameObject extraPac;
    public GameObject pacMan;
    public GameObject blinky;

    public GameObject cherry;
    public GameObject cherryPoints;
    public GameObject strawberry;
    public GameObject strawberryPoints;
    public GameObject orange;
    public GameObject orangePoints;
    public GameObject apple;
    public GameObject applePoints;
    public GameObject grapes;
    public GameObject grapesPoints;
    public GameObject galaxianBoss;
    public GameObject galaxianBossPoints;
    public GameObject bell;
    public GameObject bellPoints;
    public GameObject key;
    public GameObject keyPoints;

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

    public int numberOfPacs;
    public int startLevel;
    public int extraPacScore;

    private AudioSource startSound;
    private AudioSource siren;
    private AudioSource chomp;
    private AudioSource extraLife;
    private AudioSource eatFruit;
    private AudioSource blueGhosts;

    private bool player1ExtraLifeAwarded;
    private bool player2ExtraLifeAwarded;

    private int currentLevel;
    private List<GameObject> pacMen = new List<GameObject>();
    private List<GameObject> levelFruits = new List<GameObject>();

    private int player1Score = 0;
    private int player2Score = 0;
    private int highScore = 0;
    private int currentPlayer = 1;
    private int p1SmallDotsEaten = 0;
    private int p2SmallDotsEaten = 0;
    private int p1LargeDotsEaten = 0;
    private int p2LargeDotsEaten = 0;

    private Dictionary<string, List<GameObject>> scores = new Dictionary<string, List<GameObject>> ();

    private bool isReady = false;
    private GameObject fruit;

    public enum PointSource {
        SMALLDOT = 10,
        POWER_PELLET = 50,
        CHERRY = 100,
        STRAWBERRY = 300,
        ORANGE = 500,
        APPLE = 700,
        GRAPES = 1000,
        GALAXIAN_BOSS = 2000,
        BELL = 3000,
        KEY = 5000,
        FIRST_GHOST = 200,
        SECOND_GHOST = 400,
        THIRD_GHOST = 800,
        FOURTH_GHOST = 1600
    }

	// Use this for initialization
	void Start () 
    {
        currentLevel = startLevel;
        startSound = GetComponents<AudioSource> ()[0];
        siren = GetComponents<AudioSource> () [1];
        chomp = GetComponents<AudioSource> () [2];
        extraLife = GetComponents<AudioSource> () [3];
        eatFruit = GetComponents<AudioSource> () [4];
        blueGhosts = GetComponents<AudioSource> () [5];

        pacMan.SetActive (false);

        startSound.Play ();
        GameObject readyInst = Instantiate (ready);
        Destroy (readyInst, startSound.clip.length);
        RenderExtraPacs ();
        RenderLevel ();
        StartCoroutine (RemovePac ());
        StartCoroutine (StartInitialSiren ());
        StartCoroutine (FlashCurrentPlayer ());
	}

    void Update()
    {
        DisplayScore ();
        DisplayFruit ();
        CheckClear ();
    }

    void CheckClear()
    {
        if ((currentPlayer == 1 && p1SmallDotsEaten == 240 && p1LargeDotsEaten == 4) || (currentPlayer == 2 && p2SmallDotsEaten == 240 && p2LargeDotsEaten == 4)) {
            StopSiren ();
            StopGhosts ();
            isReady = false;
            pacMan.GetComponent<Animator> ().speed = 0.0f;
            pacMan.GetComponent<Animator> ().Play ("", 0, 0.0f);
        }
    }

    public Vector2 PacManTile
    {
        get {
            if (pacMan != null)
                return new Vector2 (Mathf.Ceil (pacMan.transform.position.x), Mathf.Ceil (pacMan.transform.position.y));
            else
                return Vector2.zero;
        }
    }

    public Vector2 BlinkyTile
    {
        get {
            return new Vector2 (Mathf.Ceil (blinky.transform.position.x), Mathf.Ceil (blinky.transform.position.y));
        }
    }

    /// <summary>
    /// Set up the game and get ready to rumble!!!
    /// </summary>
    public void Reset()
    {
        numberOfPacs--;
        if (numberOfPacs == 0) {
            GameOver ();
        } else {
            RemovePac ();
        }
    }

    void GameOver()
    {
        GameObject gameOverInst = Instantiate (gameOver);
    }

    public void Chomp()
    {
        if (!chomp.isPlaying)
            chomp.Play ();
    }

    public void NoChomp()
    {
        if(chomp.isPlaying)
            chomp.Stop ();
    }

    public void EatFruit()
    {
        if (!eatFruit.isPlaying)
            eatFruit.Play ();
    }

    public bool IsReady
    {
        get {
            return isReady;
        }
    }

    public void AddPoints(PointSource source)
    {
        if (currentPlayer == 1) {
            player1Score += (int)source;
            if (source == PointSource.SMALLDOT)
                p1SmallDotsEaten++;
            if (source == PointSource.POWER_PELLET)
                p1LargeDotsEaten++;
        } else {
            player2Score += (int)source;
            if (source == PointSource.SMALLDOT)
                p2SmallDotsEaten++;
            if (source == PointSource.POWER_PELLET)
                p2LargeDotsEaten++;
        }
        if (player1Score > highScore) {
            highScore = player1Score;
        }
        if (player2Score > highScore) {
            highScore = player2Score;
        }
        CheckForExtraLife ();
    }

    private void CheckForExtraLife()
    {
        if (currentPlayer == 1 && player1Score >= extraPacScore && !player1ExtraLifeAwarded) {
            extraLife.Play ();
            player1ExtraLifeAwarded = true;
            numberOfPacs++;
            RenderExtraPacs ();
        }
        if (currentPlayer == 2 && player2Score >= extraPacScore && !player2ExtraLifeAwarded) {
            extraLife.Play ();
            player2ExtraLifeAwarded = true;
            numberOfPacs++;
            RenderExtraPacs ();
        }
    }

    private void DisplayScore()
    {
        DisplayPlayerScore (player1Score, 6.25f, "1up");
        DisplayPlayerScore (player2Score, 25.25f, "2up");
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

    private string Reverse(string original)
    {
        char[] chars = original.ToCharArray ();
        Array.Reverse (chars);
        return new string (chars);
    }

    private IEnumerator FlashCurrentPlayer()
    {
        GameObject oneUp = GameObject.FindGameObjectWithTag ("1UP");
        GameObject twoUp = GameObject.FindGameObjectWithTag ("2UP");
        bool display = true;

        while (true) {
            yield return new WaitForSeconds (0.5f);
            display = !display;
            if (currentPlayer == 1) {
                oneUp.SetActive (!oneUp.activeSelf);
            } else {
                twoUp.SetActive (!twoUp.activeSelf);
            }
        }
    }

    public IEnumerator RemovePac()
    {
        yield return new WaitForSeconds (2);
        Destroy (pacMen [pacMen.Count - 1]);
        pacMan.SetActive (true);
        pacMan.GetComponent<Animator> ().speed = 0;
    }

    public IEnumerator StartInitialSiren()
    {
        yield return new WaitForSeconds (startSound.clip.length - 1);
        StartSiren ();
        pacMan.GetComponent<Animator> ().speed = 0.8f;
        isReady = true;
    }

    public void StartSiren()
    {
        if(!siren.isPlaying)
            siren.Play ();
    }

    public void StopSiren()
    {
        if(siren.isPlaying)
            siren.Stop ();
    }

    public void StopGhosts()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag ("Ghost");
        foreach (GameObject ghost in ghosts) {
            GhostMove m = ghost.GetComponent<GhostMove> ();
            m.Speed = 0.0f;
        }
    }

    private void RenderExtraPacs()
    {
        for (int i = 0; i < numberOfPacs; i++) {
            pacMen.Add(Instantiate (extraPac, new Vector3 (3.0f + (2.0f * i), 1.0f, 0), Quaternion.identity));
        }
    }

    void DisplayFruit()
    {
        if ((currentPlayer == 1 && (p1SmallDotsEaten == 70 || p1SmallDotsEaten == 170) || currentPlayer == 2 && (p2SmallDotsEaten == 70 || p2SmallDotsEaten == 170)) && fruit == null) {
            fruit = GetFruit ();
            Destroy (fruit, UnityEngine.Random.Range (9, 10));
        }
    }

    /// <summary>
    /// Frighten the ghosts
    /// </summary>
    public void FrightenGhosts()
    {
        blinky.GetComponent<GhostMove> ().Frighten ();

        blueGhosts.Play ();
    }

    public GameObject GetFruit()
    {
        switch (currentLevel) {
        case 1:
            return Instantiate (cherry);
        case 2:
            return Instantiate (strawberry);
        case 3:
        case 4:
            return Instantiate (orange);
        case 5:
        case 6:
            return Instantiate (apple);
        case 7:
        case 8:
            return Instantiate (grapes);
        case 9:
        case 10:
            return Instantiate (galaxianBoss);
        case 11:
        case 12:
            return Instantiate (bell);
        case 13-255:
            return Instantiate (key);
        default:
            return null;
        }
    }

    public GameObject GetFruitPoints()
    {
        switch (currentLevel) {
        case 1:
            return Instantiate (cherryPoints);
        case 2:
            return Instantiate (strawberryPoints);
        case 3:
        case 4:
            return Instantiate (orangePoints);
        case 5:
        case 6:
            return Instantiate (applePoints);
        case 7:
        case 8:
            return Instantiate (grapesPoints);
        case 9:
        case 10:
            return Instantiate (galaxianBossPoints);
        case 11:
        case 12:
            return Instantiate (bellPoints);
        case 13-255:
            return Instantiate (keyPoints);
        default:
            return null;
        }
    }

    public PointSource GetBonusPoints()
    {
        switch (currentLevel) {
        case 1:
            return PointSource.CHERRY;
        case 2:
            return PointSource.STRAWBERRY;
        case 3:
        case 4:
            return PointSource.ORANGE;
        case 5:
        case 6:
            return PointSource.APPLE;
        case 7:
        case 8:
            return PointSource.GRAPES;
        case 9:
        case 10:
            return PointSource.GALAXIAN_BOSS;
        case 11:
        case 12:
            return PointSource.BELL;
        case 13-255:
            return PointSource.KEY;
        default:
            return 0;
        }
    }

    private void RenderLevel()
    {
        foreach (GameObject levelFruit in levelFruits) {
            Destroy (levelFruit);
        }

        if (currentLevel >= 1)
            levelFruits.Add(Instantiate(cherry, new Vector3(25.0f, 1.0f, 0), Quaternion.identity));
        if (currentLevel >= 2)
            levelFruits.Add (Instantiate (strawberry, new Vector3 (23.0f, 1.0f, 0), Quaternion.identity));
        if (currentLevel >= 3)
            levelFruits.Add (Instantiate (orange, new Vector3 (21.0f, 1.0f, 0), Quaternion.identity));
        if (currentLevel >= 4)
            levelFruits.Add (Instantiate (orange, new Vector3 (19.0f, 1.0f, 0), Quaternion.identity));
        if (currentLevel >= 5)
            levelFruits.Add (Instantiate (apple, new Vector3 (17.0f, 1.0f, 0), Quaternion.identity));
        if (currentLevel >= 6)
            levelFruits.Add (Instantiate (apple, new Vector3 (15.0f, 1.0f, 0), Quaternion.identity));
    }
}
