using UnityEngine;

public class Manager_Sound : MonoBehaviour
{
    public static Manager_Sound instance;

    [Header("========= Button Sounds =========")]
    public AudioClip normalButtonSound;
    public AudioClip checkMarkBTNSound;

    [Header("========= Music =========")]
    public AudioClip bossMusic;
    public AudioClip mainMenuMusic;
    public AudioClip normalCombatMusic;

    [Header("========= Enemy sounds =========")]
    public AudioClip piranahSound;
    public AudioClip eelSound;
    public AudioClip eelShootSound;
    public AudioClip crabSound;
    public AudioClip enemyDamaged;
    public AudioClip enemyDethSound;

    [Header("========= Player sounds =========")]
    public AudioClip playerDamaged;
    public AudioClip playerDethSound;
    public AudioClip playerShootSound;
    public AudioClip playerLevelUpSound;

    [Header("========= Other sounds =========")]
    public AudioClip bulletHit;
    public AudioClip barrelExplosion;

    AudioSource _audioSFX;
    AudioSource _audioMusic;

    #region ───────── Singleton ─────────
    private void Awake()
    {
        // Only one Manager_Sound allowed
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject);

        // Grab the two AudioSources from children
        _audioSFX = transform.GetChild(0).GetComponent<AudioSource>();
        _audioMusic = transform.GetChild(1).GetComponent<AudioSource>();
    }
    #endregion

    // ─────────────────────────────────────────────────────────────────────────────
    //  PUBLIC API
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Play a one‑shot sound effect (no interruption of any current SFX).
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || _audioSFX == null) return;
        _audioSFX.PlayOneShot(clip, Mathf.Clamp01(volume));
        Debug.Log("Playing SFX");
    }

    /// <summary>
    /// Force‑stop any looping or long SFX that happens to be playing.
    /// </summary>
    public void StopSFX()
    {
        if (_audioSFX == null) return;
        _audioSFX.Stop();
    }

    /// <summary>
    /// Start background music. The track auto‑loops unless you say otherwise.
    /// </summary>
    public void PlayMusic(AudioClip clip, bool loop = true, float volume = 1f)
    {
        if (clip == null || _audioMusic == null) return;

        // Avoid restarting the same track every frame
        if (_audioMusic.clip == clip && _audioMusic.isPlaying) return;

        _audioMusic.clip = clip;
        _audioMusic.loop = loop;
        _audioMusic.volume = Mathf.Clamp01(volume);
        _audioMusic.Play();
    }

    /// <summary>
    /// Stop whatever music is currently playing.
    /// </summary>
    public void StopMusic()
    {
        if (_audioMusic == null) return;
        _audioMusic.Stop();
    }

    /// <summary>
    /// Cross‑fade from the currently‑playing music clip to <paramref name="newClip"/>.
    /// If the same clip is already playing, nothing happens.
    /// </summary>
    /// <param name="newClip">The music track you want to play next.</param>
    /// <param name="fadeTime">
    /// How long the fade‑out and fade‑in should last (in seconds).
    /// Total transition time ≈ fadeTime * 2.
    /// </param>
    public void TransitionMusic(AudioClip newClip, float fadeTime = 1f)
    {
        if (newClip == null || _audioMusic == null) return;
        if (_audioMusic.clip == newClip) return;         // already playing

        StopAllCoroutines();                             // cancel any other fades
        StartCoroutine(TransitionRoutine(newClip, fadeTime));
    }
    #region Transition Logic
    private System.Collections.IEnumerator TransitionRoutine(AudioClip newClip, float fadeTime)
    {
        float startVol = _audioMusic.volume;

        // ── Fade out current track ──
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            _audioMusic.volume = Mathf.Lerp(startVol, 0f, t / fadeTime);
            yield return null;
        }

        _audioMusic.volume = 0f;
        _audioMusic.Stop();

        // ── Switch clip & and fade in ──
        _audioMusic.clip = newClip;
        _audioMusic.Play();

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            _audioMusic.volume = Mathf.Lerp(0f, startVol, t / fadeTime);
            yield return null;
        }

        _audioMusic.volume = startVol;                   // restore original volume
    }
    #endregion
}
