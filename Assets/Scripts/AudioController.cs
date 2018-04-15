using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour 
{
    public float StartMusicLength;
    public float EatFruitLength;
    public float EatGhostLength;
    public float ExtraLifeLength;
    public float DeathLength;

    private static AudioController _instance;

    private bool _siren;
    private bool _blueGhosts;

    AudioSource _startMusicSource;
    AudioSource _sirenSource;
    AudioSource _blueGhostsSource;
    AudioSource _ghostEyesSource;

    AudioSource _eatFruitSource;
    AudioSource _eatGhostSource;
    AudioSource _extraLifeSource;
    AudioSource _deathSource;

    const string START_MUSIC = "start_music";
    const string SIREN = "siren";
    const string BLUE_GHOSTS = "blue_ghosts";
    const string GHOST_EYES = "ghost_eyes";
    const string EAT_FRUIT = "eat_fruit";
    const string EAT_GHOST = "eat_ghost";
    const string EXTRA_LIFE = "extra_life";
    const string DEATH = "death";

    public static AudioController Instance
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
	void Start () 
    {
        _startMusicSource = gameObject.AddComponent<AudioSource> ();
        _startMusicSource.clip = LoadAudioClip (START_MUSIC);

        _sirenSource = gameObject.AddComponent<AudioSource> ();
        _sirenSource.loop = true;
        _sirenSource.clip = LoadAudioClip (SIREN);

        _blueGhostsSource = gameObject.AddComponent<AudioSource> ();
        _blueGhostsSource.loop = true;
        _blueGhostsSource.clip = LoadAudioClip (BLUE_GHOSTS);

        _ghostEyesSource = gameObject.AddComponent<AudioSource> ();
        _ghostEyesSource.loop = true;
        _ghostEyesSource.clip = LoadAudioClip (GHOST_EYES);

        _eatFruitSource = gameObject.AddComponent<AudioSource> ();
        _eatFruitSource.clip = LoadAudioClip (EAT_FRUIT);

        _eatGhostSource = gameObject.AddComponent<AudioSource> ();
        _eatGhostSource.clip = LoadAudioClip (EAT_GHOST);

        _extraLifeSource = gameObject.AddComponent<AudioSource> ();
        _extraLifeSource.clip = LoadAudioClip (EXTRA_LIFE);

        _deathSource = gameObject.AddComponent<AudioSource> ();
        _deathSource.clip = LoadAudioClip (DEATH);

        StartMusicLength = _startMusicSource.clip.length;
        EatFruitLength = _eatFruitSource.clip.length;
        EatGhostLength = _eatGhostSource.clip.length;
        ExtraLifeLength = _extraLifeSource.clip.length;
        DeathLength = _deathSource.clip.length;	
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    public bool SirenPlaying
    {
        get {
            return _sirenSource.isPlaying;
        }
        set {
            if (value)
                _sirenSource.Play ();
            else
                _sirenSource.Stop ();
        }
    }

    public bool BlueGhostsPlaying
    {
        get {
            return _blueGhostsSource.isPlaying;
        }
        set {
            if (value)
                _blueGhostsSource.Play ();
            else
                _blueGhostsSource.Stop ();
        }
    }

    public bool GhostEyesPlaying
    {
        get {
            return _ghostEyesSource.isPlaying;
        }
        set {
            if (value)
                _ghostEyesSource.Play ();
            else
                _ghostEyesSource.Stop ();
        }
    }

    public void PlayStartMusic()
    {
        _startMusicSource.Play ();
    }

    public void PlayEatFruit()
    {
        _eatFruitSource.Play ();
    }

    public void PlayEatGhost()
    {
        _eatGhostSource.Play ();
    }

    public void PlayExtraLife()
    {
        _extraLifeSource.Play ();
    }

    public void PlayDeath()
    {
        _deathSource.Play ();
    }

    /// <summary>
    /// Pull a sound clip from assets
    /// </summary>
    /// <returns>The audio clip.</returns>
    /// <param name="name">Name.</param>
    private AudioClip LoadAudioClip(string name)
    {
        return (AudioClip)Resources.Load (name, typeof(AudioClip));
    }
}
