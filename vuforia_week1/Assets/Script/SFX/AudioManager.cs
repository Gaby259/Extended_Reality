using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;

    [Header("SFX Clips")]
    public AudioClip buttonSound;

    [Header("Volume")]
    [Range(0f, 1f)] public float menuMusicVolume = 0.5f;
    [Range(0f, 1f)] public float gameplayMusicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float ambientVolume = 0.7f;
    [Range(0f, 1f)] public float revealVolume = 1f;

    [Header("Settings")]
    [SerializeField] private float crossfadeDuration = 1.5f;

    private AudioSource menuMusicSource;
    private AudioSource gameplayMusicSource;
    private AudioSource sfxSource;
    private AudioSource ambientSource;
    private AudioSource revealSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        menuMusicSource = CreateAudioSource(true);
        gameplayMusicSource = CreateAudioSource(true);
        sfxSource = CreateAudioSource(false);
        ambientSource = CreateAudioSource(false);
        revealSource = CreateAudioSource(false);
    }

    private AudioSource CreateAudioSource(bool loop)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.loop = loop;
        source.playOnAwake = false;
        return source;
    }

    public void PlayMenuMusic()
    {
        if (menuMusic == null)
        {
            Debug.LogError("AudioManager: menuMusic clip is not assigned.");
            return;
        }

        gameplayMusicSource.Stop();
        menuMusicSource.clip = menuMusic;
        menuMusicSource.volume = menuMusicVolume;
        menuMusicSource.Play();
    }

    public void TransitionToGameplay()
    {
        if (gameplayMusic == null)
        {
            Debug.LogError("AudioManager: gameplayMusic clip is not assigned.");
            return;
        }

        gameplayMusicSource.clip = gameplayMusic;
        gameplayMusicSource.volume = 0f;
        gameplayMusicSource.Play();

        StartCoroutine(CrossfadeRoutine());
    }

    private IEnumerator CrossfadeRoutine()
    {
        float elapsed = 0f;
        float startMenuVolume = menuMusicSource.volume;

        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / crossfadeDuration;

            menuMusicSource.volume = Mathf.Lerp(startMenuVolume, 0f, t);
            gameplayMusicSource.volume = Mathf.Lerp(0f, gameplayMusicVolume, t);

            yield return null;
        }

        menuMusicSource.Stop();
        menuMusicSource.volume = menuMusicVolume;
        gameplayMusicSource.volume = gameplayMusicVolume;
    }

    public void PlayButtonSound()
    {
        if (buttonSound == null) return;
        sfxSource.PlayOneShot(buttonSound, sfxVolume);
    }

    public void PlayRevealSound(AudioClip clip)
    {
        if (clip == null) return;
        revealSource.clip = clip;
        revealSource.volume = revealVolume;
        revealSource.Play();
    }

    public void StopRevealSound()
    {
        revealSource.Stop();
    }

    public void PlayArtifactAmbient(AudioClip clip)
    {
        if (clip == null) return;
        ambientSource.clip = clip;
        ambientSource.volume = ambientVolume;
        ambientSource.Play();
    }

    public void StopArtifactAmbient()
    {
        ambientSource.Stop();
    }
}