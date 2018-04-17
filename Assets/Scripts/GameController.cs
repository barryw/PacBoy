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

    public int numberOfPacs;
    public int startLevel;
    public int extraPacScore;

    private bool player1ExtraLifeAwarded;
    private bool player2ExtraLifeAwarded;

    private int currentLevel;
    private List<GameObject> pacMen = new List<GameObject>();
    private List<GameObject> levelFruits = new List<GameObject>();
    private TableOfValues _tov = TableOfValues.Instance ();

    private int currentPlayer = 1;
    private int p1SmallDotsEaten = 0;
    private int p2SmallDotsEaten = 0;
    private int p1LargeDotsEaten = 0;
    private int p2LargeDotsEaten = 0;
    private int ghostsEaten = 0;
    private float frightenStartTime = 0.0f;
    private float lastDotEatenTime = 0.0f;

    private AudioController _audio;
    private ScoreBoard _score;

    private bool isReady = false;
    private GameObject fruit;

    public enum PointSource {
        SMALLDOT = 10,
        POWER_PELLET = 50,
        FIRST_GHOST = 200,
        SECOND_GHOST = 400,
        THIRD_GHOST = 800,
        FOURTH_GHOST = 1600
    }

	// Use this for initialization
	void Start () 
    {
        _audio = AudioController.Instance;
        _score = ScoreBoard.Instance;

        currentLevel = startLevel;

        // Get the movers
        pacManMover = pacMan.GetComponent<PacManMove> ();
        blinkyMover = blinky.GetComponent<GhostMove> ();
        pinkyMover = pinky.GetComponent<GhostMove> ();
        inkyMover = inky.GetComponent<GhostMove> ();
        clydeMover = clyde.GetComponent<GhostMove> ();

        pacMan.SetActive (false);

        _audio.PlayStartMusic ();
        GameObject readyInst = Instantiate (ready);
        Destroy (readyInst, _audio.StartMusicLength - 1.25f);

        RenderExtraPacs ();
        RenderLevel ();
        StartCoroutine (RemovePac ());
        StartCoroutine (StartInitialSiren ());
	}

    void Update()
    {
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
    /// Keep track of the time the last dot was eaten
    /// </summary>
    /// <value>The last dot eaten time.</value>
    public float LastDotEatenTime
    {
        get {
            return lastDotEatenTime;
        }
        set {
            lastDotEatenTime = value;
        }
    }

    /// <summary>
    /// Keep track of how long it's been since the last dot was eaten
    /// </summary>
    /// <value>The time since last dot.</value>
    public float TimeSinceLastDot
    {
        get {
            return Time.fixedTime - LastDotEatenTime;
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

    /// <summary>
    /// Display "Game Over" banner
    /// </summary>
    void GameOver()
    {
        GameObject gameOverInst = Instantiate (gameOver);
    }

    /// <summary>
    /// Which level is being played?
    /// </summary>
    /// <value>The current level.</value>
    public int CurrentLevel
    {
        get {
            return currentLevel;
        }
    }

    /// <summary>
    /// Get/Set whether the game controller is ready for play
    /// </summary>
    /// <value><c>true</c> if this instance is ready; otherwise, <c>false</c>.</value>
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

    /// <summary>
    /// Add integer points to the player's score
    /// </summary>
    /// <param name="points">Points.</param>
    public void AddPoints(int points)
    {
        _score.AddPoints (points);
    }

    /// <summary>
    /// Add points from a PointSource to the player's score
    /// </summary>
    /// <param name="source">Source.</param>
    public void AddPoints(PointSource source)
    {
        if (currentPlayer == 1) {
            AddPoints((int)source);
            if (source == PointSource.SMALLDOT)
                p1SmallDotsEaten++;
            if (source == PointSource.POWER_PELLET)
                p1LargeDotsEaten++;
        } else {
            AddPoints((int)source);
            if (source == PointSource.SMALLDOT)
                p2SmallDotsEaten++;
            if (source == PointSource.POWER_PELLET)
                p2LargeDotsEaten++;
        }
        CheckForExtraLife ();
    }

    private void CheckForExtraLife()
    {
        if (currentPlayer == 1 && _score.Player1Score >= extraPacScore && !player1ExtraLifeAwarded) {
            _audio.PlayExtraLife ();
            player1ExtraLifeAwarded = true;
            numberOfPacs++;
            RenderExtraPacs ();
        }
        if (currentPlayer == 2 && _score.Player2Score >= extraPacScore && !player2ExtraLifeAwarded) {
            _audio.PlayExtraLife ();
            player2ExtraLifeAwarded = true;
            numberOfPacs++;
            RenderExtraPacs ();
        }
    }

    public IEnumerator RemovePac()
    {
        yield return new WaitForSecondsRealtime (2);
        Destroy (pacMen [pacMen.Count - 1]);
        pacMan.SetActive (true);
        PacManMover.AnimationSpeed = 0.0f;
        PacManMover.Animation = false;
    }

    public IEnumerator StartInitialSiren()
    {
        yield return new WaitForSecondsRealtime (_audio.StartMusicLength + .25f);
        PacManMover.AnimationSpeed = 0.8f;
        PacManMover.Animation = true;
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
        }
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

    /// <summary>
    /// Return an instantiated prefab for a graphical representation of the points
    /// for the fruit on this level
    /// </summary>
    /// <returns>The fruit points.</returns>
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

    public int GetBonusPoints()
    {
        return _tov.BonusPoints (CurrentLevel);
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
