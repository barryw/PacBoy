using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    public GameObject ready;
    public GameObject gameOver;
    public GameObject extraPac;
    public GameObject pacMan;

    public GameObject cherry;
    public GameObject strawberry;
    public GameObject orange;
    public GameObject apple;
    public GameObject melon;
    public GameObject galaxianBoss;
    public GameObject bell;
    public GameObject key;

    public int numberOfPacs;
    public int startLevel;
    public int extraPacScore;

    private AudioSource startSound;
    private AudioSource siren;
    private AudioSource chomp;
    private AudioSource extraLife;
    private bool player1ExtraLifeAwarded;
    private bool player2ExtraLifeAwarded;

    private int currentLevel;
    private List<GameObject> pacMen = new List<GameObject>();
    private List<GameObject> levelFruits = new List<GameObject>();

    private int player1Score = 0;
    private int player2Score = 0;
    private int highScore = 0;
    private int currentPlayer = 1;

    private bool isReady = false;

    public enum PointSource {
        SMALLDOT = 10,
        POWER_PELLET = 50,
        CHERRY = 100,
        STRAWBERRY = 300,
        ORANGE = 500,
        APPLE = 700,
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

        pacMan.SetActive (false);

        startSound.Play ();
        GameObject readyInst = Instantiate (ready);
        Destroy (readyInst, startSound.clip.length);
        RenderExtraPacs ();
        RenderLevel ();
        StartCoroutine (RemovePac ());
        StartCoroutine (StartSiren ());
        StartCoroutine (FlashCurrentPlayer ());
	}

    void Update()
    {
        DisplayScore ();
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
        } else {
            player2Score += (int)source;
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
        GameObject p1score = GameObject.FindGameObjectWithTag ("Score1");
        GameObject p2score = GameObject.FindGameObjectWithTag ("Score2");
        GameObject highscore = GameObject.FindGameObjectWithTag ("HighScore");

        p1score.GetComponent<UnityEngine.UI.Text> ().text = player1Score.ToString ("000000");
        p2score.GetComponent<UnityEngine.UI.Text> ().text = player2Score.ToString ("000000");
        highscore.GetComponent<UnityEngine.UI.Text> ().text = highScore.ToString ("000000");
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
                if (display)
                    oneUp.GetComponent<UnityEngine.UI.Text> ().text = "1UP";
                else
                    oneUp.GetComponent<UnityEngine.UI.Text> ().text = "";
            } else {
                if (display)
                    oneUp.GetComponent<UnityEngine.UI.Text> ().text = "2UP";
                else
                    oneUp.GetComponent<UnityEngine.UI.Text> ().text = "";
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

    public IEnumerator StartSiren()
    {
        yield return new WaitForSeconds (startSound.clip.length);
        siren.Play ();
        pacMan.GetComponent<Animator> ().speed = 0.8f;
        isReady = true;
    }

    public void StopSiren()
    {
        siren.Stop ();
    }

    public void StopGhosts()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag ("Ghost");
        foreach (GameObject ghost in ghosts) {
            GhostMove m = ghost.GetComponent<GhostMove> ();
            m.SetSpeed (0.0f);
        }
    }

    private void RenderExtraPacs()
    {
        for (int i = 0; i < numberOfPacs; i++) {
            pacMen.Add(Instantiate (extraPac, new Vector3 (3.5f + (2.5f * i), -1.2f, 0), Quaternion.identity));
        }
    }

    private void RenderLevel()
    {
        foreach (GameObject levelFruit in levelFruits) {
            Destroy (levelFruit);
        }

        if (currentLevel >= 1)
            levelFruits.Add(Instantiate(cherry, new Vector3(25.0f, -1.2f, 0), Quaternion.identity));
        if (currentLevel >=2)
            levelFruits.Add (Instantiate (strawberry, new Vector3 (23.0f, -1.2f, 0), Quaternion.identity));
        if (currentLevel >= 3)
            levelFruits.Add (Instantiate (orange, new Vector3 (21.0f, -1.2f, 0), Quaternion.identity));
        if (currentLevel >= 4)
            levelFruits.Add (Instantiate (apple, new Vector3 (19.0f, -1.2f, 0), Quaternion.identity));
        if (currentLevel >= 5)
            levelFruits.Add (Instantiate (melon, new Vector3 (17.0f, -1.2f, 0), Quaternion.identity));
        if (currentLevel >= 6)
            levelFruits.Add (Instantiate (galaxianBoss, new Vector3 (15.0f, -1.2f, 0), Quaternion.identity));
    }
}
