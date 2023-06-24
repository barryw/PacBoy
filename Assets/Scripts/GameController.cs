using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : BaseController 
{
    public GameObject ready;
    public GameObject gameOver;
    public GameObject extraPac;
    public GameObject pacMan;
    public GameObject blinky;
    public GameObject pinky;
    public GameObject inky;
    public GameObject clyde;
    public GameObject powerPellet;
    public GameObject dot;

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

    private bool _player1ExtraLifeAwarded;
    private bool _player2ExtraLifeAwarded;
    private bool _isCleared;
    private bool _startOfPlay = true;

    private readonly List<GameObject> _pacMen = new List<GameObject>();
    private readonly List<GameObject> _levelFruits = new List<GameObject>();

    private List<Vector2> _p1Dots = new List<Vector2>();
    private List<Vector2> _p2Dots = new List<Vector2>();
    private List<Vector2> _p1PowerPellets = new List<Vector2> ();
    private List<Vector2> _p2PowerPellets = new List<Vector2>();
    private List<GameObject> _dots = new List<GameObject> ();
    private List<GameObject> _powerPellets = new List<GameObject>();

    private const int CurrentPlayer = 1;
    private int _p1SmallDotsEaten;
    private int _p2SmallDotsEaten;
    private int _p1LargeDotsEaten;
    private int _p2LargeDotsEaten;
    private float _frightenStartTime;

    private AudioController _audio;
    private ScoreBoard _score;

    private bool _isReady;
    private GameObject _fruit;

    public float PacManAnimationSpeed = 2.0f;

    public enum PointSource {
        Smalldot = 10,
        PowerPellet = 50,
        FirstGhost = 200,
        SecondGhost = 400,
        ThirdGhost = 800,
        FourthGhost = 1600
    }

	// Use this for initialization
    private void Start () 
    {
        _audio = AudioController.Instance;
        _score = ScoreBoard.Instance;

        CurrentLevel = 0;

        // Get the movers
        PacManMover = pacMan.GetComponent<PacManMove> ();
        BlinkyMover = blinky.GetComponent<GhostMove> ();
        PinkyMover = pinky.GetComponent<GhostMove> ();
        InkyMover = inky.GetComponent<GhostMove> ();
        ClydeMover = clyde.GetComponent<GhostMove> ();

        pacMan.SetActive (false);

        _audio.PlayStartMusic ();
        StartNextLevel ();

        StartCoroutine (RemovePac ());
	}

    private void Update()
    {
        DisplayFruit ();
        DeFrightenGhosts ();
        if(!_isCleared)
            CheckClear ();
        AdjustSiren();
    }

    /// <summary>
    /// Reset and start the next level
    /// </summary>
    private void StartNextLevel()
    {
        _isCleared = false;
        CurrentLevel++;
        DrawDots ();
        if (CurrentPlayer == 1) {
            _p1SmallDotsEaten = 0;
            _p1LargeDotsEaten = 0;
        } else {
            _p2SmallDotsEaten = 0;
            _p2LargeDotsEaten = 0;
        }
        Ready ();
    }
    
    /// <summary>
    /// On your marks. Get set...
    /// </summary>
    public void Ready()
    {
        if (numberOfPacs >= 0)
        {
            RenderExtraPacs ();
            RenderFruitLevelDisplay ();
            LastDotEatenTime = 0.0f;
            InitGhosts ();
            InitPacMan ();
            
            var readyInst = Instantiate (ready);
            var waitLength = _startOfPlay ? _audio.StartMusicLength + .25f : 3.0f;
            
            Wait(waitLength, () => { 
                _startOfPlay = false;
                IsReady = true;
                Destroy(readyInst);
                PacManMover.AnimationSpeed = PacManAnimationSpeed;
                PacManMover.Animation = true;
            });
        }
        else
        {
            GameOver();
        }
    }

    /// <summary>
    /// Check to see if the maze is clear
    /// </summary>
    private void CheckClear()
    {
        if ((CurrentPlayer != 1 || _p1Dots.Count != 0 || _p1PowerPellets.Count != 0) &&
            (CurrentPlayer != 2 || _p2Dots.Count != 0 || _p2PowerPellets.Count != 0)) return;
        _isCleared = true;
        PacManMover.Animation = false;
        PacManMover.AnimationSpeed = 0.0f;
        _audio.SirenPlaying = false;
        _audio.BlueGhostsPlaying = false;
        _audio.GhostEyesPlaying = false;
        IsReady = false;
        Wait (2, StartNextLevel);
    }

    /// <summary>
    /// Adjust the pitch of the siren based on the number of dots remaining.
    /// </summary>
    private void AdjustSiren()
    {
        var totalDotsEaten = SmallDotsEaten + LargeDotsEaten;
        if (totalDotsEaten < 61)
        {
            _audio.CurrentSiren = 1;
        } else if (totalDotsEaten >= 61 && totalDotsEaten < 122)
        {
            _audio.CurrentSiren = 2;
        } else if (totalDotsEaten >= 122 && totalDotsEaten < 183)
        {
            _audio.CurrentSiren = 3;
        }
        else
        {
            _audio.CurrentSiren = 4;
        }
    }

    #region Dots and Power Pellets

    /// <summary>
    /// Draw the dots
    /// </summary>
    private void DrawDots()
    {
        var dotList = new List<Vector2> ();
        var powerPelletList = new List<Vector2> ();

        if (CurrentPlayer == 1) {
            if (_p1Dots.Count == 0) {
                _p1Dots = Maze.DotLocations ();
                dotList = _p1Dots;
                _p1PowerPellets = Maze.EnergizerLocations ();
                powerPelletList = _p1PowerPellets;
            }
        } else {
            if (_p2Dots.Count == 0) {
                _p2Dots = Maze.DotLocations ();
                dotList = _p2Dots;
                _p2PowerPellets = Maze.EnergizerLocations ();
                powerPelletList = _p2PowerPellets;
            }
        }

        foreach (var go in _dots) {
            Destroy (go);
        }
        foreach (var go in _powerPellets) {
            Destroy (go);
        }
        _dots = LocationsToGameObjects (dotList, "dot");
        _powerPellets = LocationsToGameObjects (powerPelletList, "power_pellet");
    }

    /// <summary>
    /// Eat some small dots
    /// </summary>
    /// <param name="location">Location.</param>
    public void EatSmallDot(Vector2 location)
    {
        if (CurrentPlayer == 1)
            _p1Dots.Remove (location);
        if (CurrentPlayer == 2)
            _p2Dots.Remove (location);

        _audio.EatDot();
        LastDotEatenTime = Time.fixedTime;
        PacManMover.EatingSmallDots = true;
        AddPoints (PointSource.Smalldot);
        UpdateGhostDotCounts ();
    }

    /// <summary>
    /// Eat some power pellets
    /// </summary>
    /// <param name="location">Location.</param>
    public void EatLargeDot(Vector2 location)
    {
        if (CurrentPlayer == 1)
            _p1PowerPellets.Remove (location);
        if (CurrentPlayer == 2)
            _p2PowerPellets.Remove (location);

        AddPoints (PointSource.PowerPellet);
        FrightenGhosts ();
    }

    public int SmallDotsEaten => CurrentPlayer == 1 ? 240 - _p1Dots.Count : 240 - _p2Dots.Count;

    public int SmallDotsLeft => CurrentPlayer == 1 ? _p1Dots.Count : _p2Dots.Count;

    public int LargeDotsEaten => CurrentPlayer == 1 ? 4 - _p1PowerPellets.Count : 4 - _p2PowerPellets.Count;

    public int LargeDotsLeft => CurrentPlayer == 1 ? _p1PowerPellets.Count : _p2PowerPellets.Count;

    /// <summary>
    /// Keep track of the time the last dot was eaten
    /// </summary>
    /// <value>The last dot eaten time.</value>
    public float LastDotEatenTime { get; set; }

    /// <summary>
    /// Keep track of how long it's been since the last dot was eaten
    /// </summary>
    /// <value>The time since last dot.</value>
    public float TimeSinceLastDot => Time.fixedTime - LastDotEatenTime;

    /// <summary>
    /// Convert the dot/powerpellet locations to game objects using their prefabs
    /// </summary>
    /// <returns>The to game objects.</returns>
    /// <param name="locations">Locations.</param>
    /// <param name="objs">Objects.</param>
    private List<GameObject> LocationsToGameObjects(List<Vector2> locations, string objs)
    {
        var objects = new List<GameObject> ();
        foreach (var location in locations) {
            if (objs == "dot") {
                objects.Add(Instantiate(dot, new Vector2(location.x - 0.5f, location.y - 0.5f), Quaternion.identity));
            } else if (objs == "power_pellet") {
                objects.Add (Instantiate (powerPellet, new Vector2(location.x - 0.5f, location.y - 0.5f), Quaternion.identity));
            }
        }

        return objects;
    }

    #endregion

    #region Ghosts

    private GhostMove BlinkyMover { get; set; }
    private GhostMove PinkyMover { get; set; }
    private GhostMove InkyMover { get; set; }
    private GhostMove ClydeMover { get; set; }
    public int GhostsEaten { get; set; }

    /// <summary>
    /// Initialize the ghosts
    /// </summary>
    private void InitGhosts()
    {
        BlinkyMover.CurrentMode = GhostStates.InitialPosition;
        PinkyMover.CurrentMode = GhostStates.InitialPosition;
        InkyMover.CurrentMode = GhostStates.InitialPosition;
        ClydeMover.CurrentMode = GhostStates.InitialPosition;
    }

    /// <summary>
    /// Make sure the ghosts know how many dots have been eaten.
    /// </summary>
    private void UpdateGhostDotCounts()
    {
        BlinkyMover.IncreaseDotCount ();
        PinkyMover.IncreaseDotCount ();
        InkyMover.IncreaseDotCount ();
        ClydeMover.IncreaseDotCount ();
    }

    #endregion

    private void InitPacMan()
    {
        PacManMover.AnimationSpeed = 0.0f;
        PacManMover.Animation = false;
        PacManMover.PacManInit ();
    }

    public PacManMove PacManMover { get; private set; }

    /// <summary>
    /// Display "Game Over" banner
    /// </summary>
    private void GameOver()
    {
        var gameOverInst = Instantiate (gameOver);
    }

    /// <summary>
    /// Which level is being played?
    /// </summary>
    /// <value>The current level.</value>
    public int CurrentLevel { get; private set; }

    /// <summary>
    /// Get/Set whether the game controller is ready for play
    /// </summary>
    /// <value><c>true</c> if this instance is ready; otherwise, <c>false</c>.</value>
    public bool IsReady
    {
        get {
            return _isReady;
        }
        set {
            _audio.SirenPlaying = value;
            _isReady = value;
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
        if (CurrentPlayer == 1) {
            AddPoints((int)source);
            if (source == PointSource.Smalldot)
                _p1SmallDotsEaten++;
            if (source == PointSource.PowerPellet)
                _p1LargeDotsEaten++;
        } else {
            AddPoints((int)source);
            if (source == PointSource.Smalldot)
                _p2SmallDotsEaten++;
            if (source == PointSource.PowerPellet)
                _p2LargeDotsEaten++;
        }
        CheckForExtraLife ();
    }

    private void CheckForExtraLife()
    {
        if (CurrentPlayer == 1 && _score.Player1Score >= extraPacScore && !_player1ExtraLifeAwarded) {
            _audio.PlayExtraLife ();
            _player1ExtraLifeAwarded = true;
            numberOfPacs++;
            RenderExtraPacs ();
        }
        if (CurrentPlayer == 2 && _score.Player2Score >= extraPacScore && !_player2ExtraLifeAwarded) {
            _audio.PlayExtraLife ();
            _player2ExtraLifeAwarded = true;
            numberOfPacs++;
            RenderExtraPacs ();
        }
    }

    private IEnumerator RemovePac()
    {
        yield return new WaitForSecondsRealtime (2);
        numberOfPacs--;
        RenderExtraPacs();
        pacMan.SetActive (true);
        PacManMover.AnimationSpeed = 0.0f;
        PacManMover.Animation = false;
    }

    /// <summary>
    /// Show the number of lives remaining
    /// </summary>
    private void RenderExtraPacs()
    {
        foreach(var live in _pacMen)
            Destroy(live);
        
        for (var i = 0; i < numberOfPacs; i++) {
            _pacMen.Add(Instantiate (extraPac, new Vector3 (3.0f + (2.0f * i), 1.0f, 0), Quaternion.identity));
        }
    }

    private void DisplayFruit()
    {
        if ((CurrentPlayer == 1 && (_p1SmallDotsEaten == 70 || _p1SmallDotsEaten == 170) || CurrentPlayer == 2 && (_p2SmallDotsEaten == 70 || _p2SmallDotsEaten == 170)) && _fruit == null) {
            _fruit = GetFruit ();
            Destroy (_fruit, Random.Range (9, 10));
        }
    }

    /// <summary>
    /// Check to see if we can de-frighten the ghosts
    /// </summary>
    private void DeFrightenGhosts()
    {
        // First, see if we've eaten all of the ghosts and they're all back in the maze
        var frightenedGhosts = GetFrightenedGhosts ();
        if (frightenedGhosts.Count == 0 && _audio.BlueGhostsPlaying) {
            _audio.BlueGhostsPlaying = false;
            _audio.GhostEyesPlaying = false;
            _audio.SirenPlaying = true;
        }
            
        // If we still have ghosts, see if time's up
        if (frightenedGhosts.Count > 0 && (Time.fixedTime - _frightenStartTime > TableOfValues.GhostFrightenedTime (CurrentLevel))) {
            foreach (var ghost in frightenedGhosts) {
                if (ghost.CurrentMode != GhostStates.Dead && !ghost.IsBlinking) {
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
        if (PacManMover.Frightened) return;
        BlinkyMover.Frightened = true;
        PinkyMover.Frightened = true;
        InkyMover.Frightened = true;
        ClydeMover.Frightened = true;
        PacManMover.Frightened = true;

        _frightenStartTime = Time.fixedTime;

        _audio.SirenPlaying = false;
        _audio.BlueGhostsPlaying = true;
    }

    /// <summary>
    /// Figure out which, if any ghosts are still in frightened mode
    /// </summary>
    /// <returns>The frightened ghosts.</returns>
    private List<GhostMove> GetFrightenedGhosts()
    {
        var frightened = new List<GhostMove> ();
        if (BlinkyMover.CurrentMode == GhostStates.Frightened)
            frightened.Add (BlinkyMover);
        if (PinkyMover.CurrentMode == GhostStates.Frightened)
            frightened.Add (PinkyMover);
        if (InkyMover.CurrentMode == GhostStates.Frightened)
            frightened.Add (InkyMover);
        if (ClydeMover.CurrentMode == GhostStates.Frightened)
            frightened.Add (ClydeMover);

        return frightened;
    }

    /// <summary>
    /// Get the fruit associated with a level
    /// </summary>
    /// <returns></returns>
    private GameObject GetFruit(int level = 0)
    {
        if (level == 0)
            level = CurrentLevel;
        
        switch (level) {
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
    /// <returns>The fruit points game object.</returns>
    public GameObject GetFruitPoints()
    {
        switch (CurrentLevel) {
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

    /// <summary>
    /// Get the number of points the fruit is worth on this level
    /// </summary>
    /// <returns>Point value</returns>
    public int GetBonusPoints()
    {
        return TableOfValues.BonusPoints (CurrentLevel);
    }

    /// <summary>
    /// Render the fruit display in the lower right corner of the screen to indicate
    /// what level PacMan is currently on.
    /// </summary>
    private void RenderFruitLevelDisplay()
    {
        foreach (var levelFruit in _levelFruits) {
            Destroy (levelFruit);
        }

        int startPos;
        int endPos;
        
        if (CurrentLevel < 7)
        {
            startPos = 1;
            endPos = CurrentLevel;
        }
        else
        {
            startPos = CurrentLevel - 6;
            endPos = CurrentLevel;
        }

        var x = 25.0f;
        
        for (var level = startPos; level <= endPos; level++)
        {
            var fruit = GetFruit(level);
            fruit.transform.position = new Vector3(x, 1.0f, 0);
            _levelFruits.Add(fruit);
            x -= 2.0f;
        }
    }
}
