using UnityEngine;

public class AudioController : MonoBehaviour 
{
    public float StartMusicLength;
    public float EatGhostLength;

    private bool _siren;
    private bool _blueGhosts;

    private AudioSource _startMusicSource;
    private AudioSource _sirenSource;
    private AudioSource _blueGhostsSource;
    private AudioSource _ghostEyesSource;

    private AudioSource _eatFruitSource;
    private AudioSource _eatGhostSource;
    private AudioSource _extraLifeSource;
    private AudioSource _deathSource;

    private AudioSource _eatDot1;
    private AudioSource _eatDot2;

    private const string StartMusic = "start_music";
    
    private const string Siren1 = "siren1";
    private const string Siren2 = "siren2";
    private const string Siren3 = "siren3";
    private const string Siren4 = "siren4";

    private const string EatDot1 = "chomp1";
    private const string EatDot2 = "chomp2";
    private bool _eatingDot1;
    
    private const string BlueGhosts = "blue_ghosts";
    private const string GhostEyes = "ghost-eyes";
    private const string EatFruit = "eat_fruit";
    private const string EatGhost = "eat_ghost";
    private const string ExtraLife = "extra_life";
    private const string Death = "death";

    public static AudioController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy (gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad (gameObject);
    }

	// Use this for initialization
    private void Start () 
    {
        _startMusicSource = gameObject.AddComponent<AudioSource> ();
        _startMusicSource.clip = LoadAudioClip (StartMusic);

        _sirenSource = gameObject.AddComponent<AudioSource> ();
        _sirenSource.loop = true;
        _sirenSource.clip = LoadAudioClip (Siren1);

        _blueGhostsSource = gameObject.AddComponent<AudioSource> ();
        _blueGhostsSource.loop = true;
        _blueGhostsSource.clip = LoadAudioClip (BlueGhosts);

        _ghostEyesSource = gameObject.AddComponent<AudioSource> ();
        _ghostEyesSource.loop = true;
        _ghostEyesSource.clip = LoadAudioClip (GhostEyes);

        _eatFruitSource = gameObject.AddComponent<AudioSource> ();
        _eatFruitSource.clip = LoadAudioClip (EatFruit);

        _eatGhostSource = gameObject.AddComponent<AudioSource> ();
        _eatGhostSource.clip = LoadAudioClip (EatGhost);

        _extraLifeSource = gameObject.AddComponent<AudioSource> ();
        _extraLifeSource.clip = LoadAudioClip (ExtraLife);

        _deathSource = gameObject.AddComponent<AudioSource> ();
        _deathSource.clip = LoadAudioClip (Death);

        _eatDot1 = gameObject.AddComponent<AudioSource>();
        _eatDot1.clip = LoadAudioClip(EatDot1);

        _eatDot2 = gameObject.AddComponent<AudioSource>();
        _eatDot2.clip = LoadAudioClip(EatDot2);
        
        StartMusicLength = _startMusicSource.clip.length;
        EatGhostLength = _eatGhostSource.clip.length;
    }

    public bool SirenPlaying
    {
        set {
            if (value)
                _sirenSource.Play ();
            else
                _sirenSource.Stop ();
        }
    }

    /// <summary>
    /// Alternate the chomp sounds
    /// </summary>
    public void EatDot()
    {
        if (_eatingDot1)
        {
            _eatDot1.Play();
        }
        else
        {
            _eatDot2.Play();
        }

        _eatingDot1 = !_eatingDot1;
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
    private static AudioClip LoadAudioClip(string name)
    {
        return (AudioClip)Resources.Load (name, typeof(AudioClip));
    }
}
