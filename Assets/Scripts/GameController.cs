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
    public GameObject pinky;
    public GameObject inky;
    public GameObject clyde;

    private PacManMove pacManMover;
    private GhostMove blinkyMover;
    private GhostMove pinkyMover;
    private GhostMove inkyMover;
    private GhostMove clydeMover;

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

    private bool player1ExtraLifeAwarded;
    private bool player2ExtraLifeAwarded;

    private int currentLevel;
    private List<GameObject> pacMen = new List<GameObject>();
    private List<GameObject> levelFruits = new List<GameObject>();
    private TableOfValues _tov = TableOfValues.Instance ();

    private int player1Score = 0;
    private int player2Score = 0;
    private int highScore = 0;
    private int currentPlayer = 1;
    private int p1SmallDotsEaten = 0;
    private int p2SmallDotsEaten = 0;
    private int p1LargeDotsEaten = 0;
    private int p2LargeDotsEaten = 0;
    private int ghostsEaten = 0;
    private float frightenStartTime = 0.0f;

    private Dictionary<string, List<GameObject>> scores = new Dictionary<string, List<GameObject>> ();

    private AudioController _audio;

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
        _audio = AudioController.Instance;

        currentLevel = startLevel;

        // Get the movers
        pacManMover = pacMan.GetComponent<PacManMove> ();
        blinkyMover = blinky.GetComponent<GhostMove> ();
        pinkyMover = pinky.GetComponent<GhostMove> ();
        inkyMover = inky.GetComponent<GhostMove> ();
        clydeMover = clyde.GetComponent<GhostMove> ();

        pacMan.SetActive (false);

        _audio.PlayStartMusic();

        GameObject readyInst = Instantiate (ready);
        Destroy (readyInst, _audio.StartMusicLength);
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
        DeFrightenGhosts ();
        CheckClear ();
    }

    void CheckClear()
    {
        if ((currentPlayer == 1 && p1SmallDotsEaten == 240 && p1LargeDotsEaten == 4) || (currentPlayer == 2 && p2SmallDotsEaten == 240 && p2LargeDotsEaten == 4)) {
            IsReady = false;
        }
    }

    public int SmallDotsEaten
    {
        get {
            if (currentPlayer == 1)
                return p1SmallDotsEaten;
            else
                return p2SmallDotsEaten;
        }
    }

    public int SmallDotsLeft
    {
        get {
            if (currentPlayer == 1)
                return 240 - p1SmallDotsEaten;
            else
                return 240 - p2SmallDotsEaten;
        }
    }

    public int LargeDotsEaten
    {
        get {
            if (currentPlayer == 1)
                return p1LargeDotsEaten;
            else
                return p2LargeDotsEaten;
        }
    }

    public int LargeDotsLeft
    {
        get {
            if (currentPlayer == 1)
                return 4 - p1LargeDotsEaten;
            else
                return 4 - p2LargeDotsEaten;
        }
    }

    public int GhostsEaten
    {
        get {
            return ghostsEaten;
        }
        set{
            ghostsEaten = value;
        }
    }

    /// <summary>
    /// Make sure the ghosts know how many dots have been eaten.
    /// </summary>
    public void UpdateGhostDotCounts()
    {
        BlinkyMover.IncreaseDotCount ();
        PinkyMover.IncreaseDotCount ();
        InkyMover.IncreaseDotCount ();
        ClydeMover.IncreaseDotCount ();
    }

    public PacManMove PacManMover
    {
        get {
            return pacManMover;
        }
    }

    public GhostMove BlinkyMover
    {
        get {
            return blinkyMover;
        }
    }

    public GhostMove PinkyMover
    {
        get {
            return pinkyMover;
        }
    }

    public GhostMove InkyMover
    {
        get {
            return inkyMover;
        }
    }

    public GhostMove ClydeMover
    {
        get {
            return clydeMover;
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

    public int CurrentLevel
    {
        get {
            return currentLevel;
        }
    }

    public bool IsReady
    {
        get {
            return isReady;
        }
        set {
            _audio.SirenPlaying = value;
            isReady = value;
            if (!value)
                pacMan.GetComponent<PacManMove> ().ResetAnimation ();
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
            _audio.PlayExtraLife ();
            player1ExtraLifeAwarded = true;
            numberOfPacs++;
            RenderExtraPacs ();
        }
        if (currentPlayer == 2 && player2Score >= extraPacScore && !player2ExtraLifeAwarded) {
            _audio.PlayExtraLife ();
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
            yield return new WaitForSecondsRealtime (0.5f);
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
        yield return new WaitForSecondsRealtime (2);
        Destroy (pacMen [pacMen.Count - 1]);
        pacMan.SetActive (true);
        pacMan.GetComponent<Animator> ().speed = 0;
    }

    public IEnumerator StartInitialSiren()
    {
        yield return new WaitForSecondsRealtime (_audio.StartMusicLength + .25f);
        PacManMover.AnimationSpeed = 0.8f;
        IsReady = true;
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
    /// Check to see if we can de-frighten the ghosts
    /// </summary>
    void DeFrightenGhosts()
    {
        // First, see if we've eaten all of the ghosts and they're all back in the maze
        List<GhostMove> frightenedGhosts = GetFrightenedGhosts ();
        if (frightenedGhosts.Count == 0 && _audio.BlueGhostsPlaying) {
            _audio.BlueGhostsPlaying = false;
            _audio.GhostEyesPlaying = false;
            _audio.SirenPlaying = true;
        }
            
        // If we still have ghosts, see if time's up
        if (frightenedGhosts.Count > 0 && (Time.fixedTime - frightenStartTime > _tov.GhostFrightenedTime (CurrentLevel))) {
            foreach (GhostMove ghost in frightenedGhosts) {
                if (!ghost.IsEaten && !ghost.IsBlinking) {
                    ghost.DoBlinkGhost ();
                }
            }
            //_audio.BlueGhostsPlaying = false;
            //_audio.GhostEyesPlaying = false;
            //_audio.SirenPlaying = true;
        }
    }

    /// <summary>
    /// If the ghosts are frightened and the frightened time has elapsed, start to blink them
    /// </summary>
    void BlinkGhosts()
    {
        
    }

    /// <summary>
    /// Frighten the ghosts
    /// </summary>
    public void FrightenGhosts()
    {
        if (!PacManMover.Frightened) {
            BlinkyMover.Frightened = true;
            PinkyMover.Frightened = true;
            InkyMover.Frightened = true;
            ClydeMover.Frightened = true;
            PacManMover.Frightened = true;

            frightenStartTime = Time.fixedTime;

            _audio.SirenPlaying = false;
            _audio.BlueGhostsPlaying = true;
        }
    }

    /// <summary>
    /// Figure out which, if any ghosts are still in frightened mode
    /// </summary>
    /// <returns>The frightened ghosts.</returns>
    private List<GhostMove> GetFrightenedGhosts()
    {
        List<GhostMove> frightened = new List<GhostMove> ();
        if (BlinkyMover.CurrentMode == GhostMove.Mode.FRIGHTENED)
            frightened.Add (BlinkyMover);
        if (PinkyMover.CurrentMode == GhostMove.Mode.FRIGHTENED)
            frightened.Add (PinkyMover);
        if (InkyMover.CurrentMode == GhostMove.Mode.FRIGHTENED)
            frightened.Add (InkyMover);
        if (ClydeMover.CurrentMode == GhostMove.Mode.FRIGHTENED)
            frightened.Add (ClydeMover);

        return frightened;
    }

    /// <summary>
    /// Get the ghost that currently shares PacMan's tile
    /// </summary>
    /// <returns>The at pac man tile.</returns>
    public GhostMove GhostAtPacManTile()
    {
        GhostMove ghost = null;

        if (BlinkyMover.Tile == PacManMover.Tile)
            ghost = BlinkyMover;
        
        if(PinkyMover.Tile == PacManMover.Tile)
            ghost = PinkyMover;

        if (InkyMover.Tile == PacManMover.Tile)
            ghost = InkyMover;

        if (ClydeMover.Tile == PacManMover.Tile)
            ghost = ClydeMover;
        
        return ghost;
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
